// ReSharper disable ClassNeverInstantiated.Global
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Fleck;
using Loupedeck.GoXLR.Plugin.Config.Models;
using Loupedeck.GoXLR.Plugin.Enums;
using Loupedeck.GoXLR.Plugin.GoXLR;
using Loupedeck.GoXLR.Plugin.GoXLR.Enums;
using Loupedeck.GoXLR.Plugin.GoXLR.EventArgs;
using Loupedeck.GoXLR.Plugin.GoXLR.Models;
using Loupedeck.GoXLR.Plugin.Helpers;
using EventHandler = Loupedeck.GoXLR.Plugin.GoXLR.EventHandler;

namespace Loupedeck.GoXLR.Plugin
{
    public class GoXlrPlugin : Loupedeck.Plugin
    {
        private WebSocketServer _wss;

        protected internal static PluginSettings PluginSettings;

        protected internal static IWebSocketConnection WsConnection;
        protected internal static List<string> Profiles = new List<string>();
        protected internal static Profile ActiveProfile = new Profile();
        protected internal static readonly Dictionary<string, State?> RoutingStates = new Dictionary<string, State?>();

        public GoXlrPlugin() => PluginLog.Init(Log);
        public override bool UsesApplicationApiOnly => true;
        public override bool HasNoApplication => true;

        /// <summary>
        /// Once the plugin loaded, open a WebSocket server and subscribe to all events
        /// </summary>
        public override void Load()
        {
            SetPluginState();
            LoadPluginIcons();
            EventHandler.OnRoutingChanged += OnRoutingChanged;
            EventHandler.OnProfilesChanged += OnProfilesChanged;
            EventHandler.OnProfileSelected += OnProfileSelected;

            _wss = new WebSocketServer("ws://127.0.0.1:6805");
            _wss.Start(wsConnection =>
            {
                WsConnection = wsConnection;
                WsConnection.OnOpen += OnOpen;
                WsConnection.OnMessage += OnMessage;
                WsConnection.OnClose += OnClose;
            });
        }

        private void LoadPluginIcons()
        {
            Log.Info("Start loading Icons.");
            Info.Icon16x16 = EmbeddedResources.ReadImage("Loupedeck.GoXLR.Plugin.metadata.Icon16x16.png");
            Info.Icon32x32 = EmbeddedResources.ReadImage("Loupedeck.GoXLR.Plugin.metadata.Icon32x32.png");
            Info.Icon48x48 = EmbeddedResources.ReadImage("Loupedeck.GoXLR.Plugin.metadata.Icon48x48.png");
            Info.Icon256x256 = EmbeddedResources.ReadImage("Loupedeck.GoXLR.Plugin.metadata.Icon256x256.png");
            Log.Info("Finished loading Icons.");
        }

        /// <summary>
        /// Once the plugin got unloaded, dispose the WebSocket server and unsubscribe from all events
        /// </summary>
        public override void Unload()
        {
            EventHandler.OnRoutingChanged -= OnRoutingChanged;
            EventHandler.OnProfilesChanged -= OnProfilesChanged;
            EventHandler.OnProfileSelected -= OnProfileSelected;
            WsConnection.OnOpen -= OnOpen;
            WsConnection.OnMessage -= OnMessage;
            WsConnection.OnClose -= OnClose;

            _wss?.Dispose();
        }
        
        /// <summary>
        /// Once routing has changed, if RoutingStates already contains the routing set the new state
        /// else add it to the directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnRoutingChanged(object sender, OnRoutingChangedEventArgs e)
        {
            if (RoutingStates.ContainsKey(e.Routing.ToString()))
                RoutingStates[e.Routing.ToString()] = e.State;
            else
                RoutingStates.Add(e.Routing.ToString(), e.State);
        }

        /// <summary>
        /// Once profiles has changed, replace the old profile list with the new one
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnProfilesChanged(object sender, OnProfilesChangedEventArgs e)
        {
            Profiles = e.Profiles.ToList();
        }

        /// <summary>
        /// Once profile is selected, set the ActiveProfile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnProfileSelected(object sender, OnProfileSelectedEventArgs e)
        {
            ActiveProfile = e.Profile;
        }

        /// <summary>
        /// Once the WebSocket connection is opened, set the plugin state to connected
        /// </summary>
        private void OnOpen()
        {
            SetPluginState(PluginState.Connected);
            EventHandler.RefreshIcons?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Once the WebSocket receives a message, handle it
        /// </summary>
        /// <param name="message"></param>
        private static async void OnMessage(string message)
        {
            await MessageHandler.Handle(WsConnection, message);
        }
        
        /// <summary>
        /// Once the WebSocket connection is closed, set the plugin state to not connected
        /// </summary>
        private void OnClose()
        {
            SetPluginState(PluginState.NotConnected);
        }

        /// <summary>
        /// Depending whether the port is already used or not, set the state
        /// </summary>
        private void SetPluginState()
        {
            var portInUse = !CheckIfPortFree();

            var state = portInUse
                ? PluginState.PortInUse
                : PluginState.NotConnected;
            
            SetPluginState(state);
        }

        /// <summary>
        /// Set the plugin state
        /// </summary>
        /// <param name="state">The state to set</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void SetPluginState(PluginState state)
        {
            switch (state)
            {
                case PluginState.Connected:
                    OnPluginStatusChanged(Loupedeck.PluginStatus.Normal, "GoXLR connected", "", "");
                    break;
                
                case PluginState.NotConnected:
                    OnPluginStatusChanged(Loupedeck.PluginStatus.Warning, "GoXLR not connected", "", "");
                    break;
                
                case PluginState.PortInUse:
                    OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "Port 6805 is already in use", "", "");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        /// <summary>
        /// Since GetActiveTcpListeners() isn't implemented in Mono try connect to Port
        /// On Error -> nothing open, otherwise something on port
        /// </summary>
        /// <returns></returns>
        private static bool CheckIfPortFree()
        {
            try
            {
                using (var client = new TcpClient())
                {
                    client.Connect("127.0.0.1", 6805);
                }
            }
            catch
            {
                return true;
            }

            return false;
        }
    }
}
