namespace Loupedeck.GoXLRPlugin
{
    using Commands;

    using Enums;

    using GoXLR.Server.Enums;
    using GoXLR.Server.Models;

    public class GoXLREventHandler : IGoXLREventHandler
    {
        private readonly GoXLRPlugin _plugin;
        private readonly DynamicActionProvider _actionProvider;

        public GoXLREventHandler(GoXLRPlugin plugin, DynamicActionProvider actionProvider)
        {
            this._plugin = plugin;
            this._actionProvider = actionProvider;
        }

        public void ConnectedClientChangedEvent(in ConnectedClient client)
        {
            var pluginState = client == ConnectedClient.Empty
                ? PluginState.AppNotConnected
                : PluginState.AppConnected;

            this._plugin.SetPluginState(pluginState);
        }

        public void ProfileListChangedEvent(Profile[] profiles)
        {
            this._actionProvider.Provide<SetProfileCommand>()
                .UpdateProfileList(profiles);
        }

        public void ProfileSelectedChangedEvent(in Profile profile)
        {
            this._actionProvider.Provide<SetProfileCommand>()
                .UpdateSelectedProfile(profile);
        }

        public void RoutingStateChangedEvent(in Routing routing, in State state)
        {
            //How to pass this event?
            this._actionProvider.Provide<SetRoutingCommand>()
                .UpdateState(routing, state);

        }
    }
}
