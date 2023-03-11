using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fleck;
using Loupedeck.GoXLR.Plugin.GoXLR.Commands;
using Loupedeck.GoXLR.Plugin.GoXLR.Enums;
using Loupedeck.GoXLR.Plugin.GoXLR.EventArgs;
using Loupedeck.GoXLR.Plugin.GoXLR.Models;
using Newtonsoft.Json;

namespace Loupedeck.GoXLR.Plugin.GoXLR
{
    public abstract class MessageHandler
    {
        private static string[] _profiles = Array.Empty<string>();
        private static readonly Profile Profile = new Profile();
        private static readonly Dictionary<string, State> Routing = new Dictionary<string, State>();

        /// <summary>
        /// Handle incoming messages from the GoXLR Software
        /// </summary>
        /// <param name="wsConnection">The WebSocket connection</param>
        /// <param name="message">The message that needs to be handled</param>
        public static async Task Handle(IWebSocketConnection wsConnection, string message)
        {
            var json = JsonConvert.DeserializeObject<NotificationMessage>(message);

            switch (json.Event)
            {
                case "goxlrConnectionEvent":
                    await CommandHandler.Send(new SubscribeToRouting(), wsConnection);
                    await CommandHandler.Send(new GetProfiles(), wsConnection);
                    break;

                case "sendToPropertyInspector" when json.Action == "com.tchelicon.goxlr.profilechange":
                    await HandleProfileEvents(wsConnection, json);
                    break;

                case "getSettings" when json.Context != "fetchingProfiles" && json.Action == "com.tchelicon.goxlr.profilechange":
                    await CommandHandler.Send(new RespondCanReceiveProfile(json.Context), wsConnection);
                    break;

                case "getSettings" when Models.Routing.TryParseContext(json.Context, out var routing) && json.Action == "com.tchelicon.goxlr.routingtable":
                    await CommandHandler.Send(new RespondCanReceiveRouting(json.Context, routing), wsConnection);
                    break;

                case "setState" when json.Action == "com.tchelicon.goxlr.profilechange" && json.GetState() == State.On:
                    Profile.SetProfile(json.Context);
                    EventHandler.OnProfileSelected?.Invoke(null, new OnProfileSelectedEventArgs { Profile = Profile });
                    break;

                case "setState" when json.Action == "com.tchelicon.goxlr.routingtable" && Models.Routing.TryParseContext(json.Context, out var routing2):
                    HandleRoutingEvents(json, routing2);
                    break;
            }
        }

        /// <summary>
        /// Handle the routing message and only invoke the event if it really changed
        /// </summary>
        /// <param name="message">The message that needs to be handled</param>
        /// <param name="routing">The routing</param>
        private static void HandleRoutingEvents(NotificationMessage message, Routing routing)
        {
            var state = message.GetState();
            var routingContext = routing.ToString();

            if (!Routing.ContainsKey(routingContext))
            {
                Routing.Add(routingContext, state);
                EventHandler.OnRoutingChanged?.Invoke(null, new OnRoutingChangedEventArgs { Routing = routing, State = state });
                return;
            }

            var oldState = Routing[routingContext];
            
            if (oldState == state)
                return;
            
            Routing[routingContext] = state;
            EventHandler.OnRoutingChanged?.Invoke(null, new OnRoutingChangedEventArgs { Routing = routing, State = state });
        }

        /// <summary>
        /// Handle the profile message and invoke the event with the new profile list.
        /// Also subscribe and unsubscribe to new and old profiles
        /// </summary>
        /// <param name="wsConnection">The WebSocket connection</param>
        /// <param name="message">The message</param>
        private static async Task HandleProfileEvents(IWebSocketConnection wsConnection, NotificationMessage message)
        {
            var profiles = message.GetProfiles();
            var current = _profiles;
            var added = profiles.Except(current).ToArray();
            var removed = current.Except(profiles).ToArray();

            if (!added.Any() && !removed.Any())
                return;
                    
            _profiles = profiles;

            EventHandler.OnProfilesChanged?.Invoke(null, new OnProfilesChangedEventArgs { Profiles = profiles });

            foreach (var profile in added)
            {
                await CommandHandler.Send(new SubscribeToProfiles(profile), wsConnection);
            }

            foreach (var profile in removed)
            {
                await CommandHandler.Send(new UnsubscribeToProfiles(profile), wsConnection);
            }
        }
    }
}