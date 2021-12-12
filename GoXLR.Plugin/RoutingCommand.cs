namespace Loupedeck.GoXLRPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using GoXLR.Server.Enums;
    using GoXLR.Server.Models;

    class RoutingCommand : PluginDynamicCommand
    {
        private GoXLRPlugin _plugin;

        public RoutingCommand() : base()
        {
            this.AddParameter("Mic|Headphones", "Mic", "Headphones Routing");
            this.AddParameter("Chat|Headphones", "Chat", "Headphones Routing");
            this.AddParameter("Music|Headphones", "Music", "Headphones Routing");
            this.AddParameter("Game|Headphones", "Game", "Headphones Routing");
            this.AddParameter("Console|Headphones", "Console", "Headphones Routing");
            this.AddParameter("LineIn|Headphones", "Line In", "Headphones Routing");
            this.AddParameter("System|Headphones", "System", "Headphones Routing");
            this.AddParameter("Samples|Headphones", "Samples", "Headphones Routing");

            this.AddParameter("Mic|Broadcast", "Mic", "Broadcast Stream Mix Routing");
            this.AddParameter("Chat|Broadcast", "Chat", "Broadcast Stream Mix Routing");
            this.AddParameter("Music|Broadcast", "Music", "Broadcast Stream Mix Routing");
            this.AddParameter("Game|Broadcast", "Game", "Broadcast Stream Mix Routing");
            this.AddParameter("Console|Broadcast", "Console", "Broadcast Stream Mix Routing");
            this.AddParameter("LineIn|Broadcast", "Line In", "Broadcast Stream Mix Routing");
            this.AddParameter("System|Broadcast", "System", "Broadcast Stream Mix Routing");
            this.AddParameter("Samples|Broadcast", "Samples", "Broadcast Stream Mix Routing");

            this.AddParameter("Mic|LineOut", "Mic", "Line Out Routing");
            this.AddParameter("Chat|LineOut", "Chat", "Line Out Routing");
            this.AddParameter("Music|LineOut", "Music", "Line Out Routing");
            this.AddParameter("Game|LineOut", "Game", "Line Out Routing");
            this.AddParameter("Console|LineOut", "Console", "Line Out Routing");
            this.AddParameter("LineIn|LineOut", "Line In", "Line Out Routing");
            this.AddParameter("System|LineOut", "System", "Line Out Routing");
            this.AddParameter("Samples|LineOut", "Samples", "Line Out Routing");

            this.AddParameter("Mic|ChatMic", "Mic", "Chat Mic Routing");
            this.AddParameter("Music|ChatMic", "Music", "Chat Mic Routing");
            this.AddParameter("Game|ChatMic", "Game", "Chat Mic Routing");
            this.AddParameter("Console|ChatMic", "Console", "Chat Mic Routing");
            this.AddParameter("LineIn|ChatMic", "Line In", "Chat Mic Routing");
            this.AddParameter("System|ChatMic", "System", "Chat Mic Routing");
            this.AddParameter("Samples|ChatMic", "Samples", "Chat Mic Routing");

            this.AddParameter("Mic|Sampler", "Mic", "Sampler Routing");
            this.AddParameter("Chat|Sampler", "Chat", "Sampler Routing");
            this.AddParameter("Music|Sampler", "Music", "Sampler Routing");
            this.AddParameter("Game|Sampler", "Game", "Sampler Routing");
            this.AddParameter("Console|Sampler", "Console", "Sampler Routing");
            this.AddParameter("LineIn|Sampler", "Line In", "Sampler Routing");
            this.AddParameter("System|Sampler", "System", "Sampler Routing");
        }

        protected override Boolean OnLoad()
        {
            _plugin = (GoXLRPlugin)base.Plugin;
            return true;
        }

        protected override void RunCommand(String actionParameter)
        {
            var split = actionParameter.Split('|');

            if (!Routing.TryParseDescription(split[0], split[1], out var routing))
                return;

            var server = _plugin.Server;
            server.SetRouting(RoutingAction.Toggle, routing, CancellationToken.None)
                .GetAwaiter()
                .GetResult();

            //TODO: Should be done from event handler:
            this.ActionImageChanged(actionParameter);
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {
            return actionParameter;
        }
    }
}
