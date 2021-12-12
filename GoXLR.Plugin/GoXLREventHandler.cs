namespace Loupedeck.GoXLRPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using GoXLR.Server.Models;

    public class GoXLREventHandler : IGoXLREventHandler
    {
        private readonly GoXLRPlugin _plugin;

        public GoXLREventHandler(GoXLRPlugin plugin)
        {
            this._plugin = plugin;
        }

        public void ConnectedClientChangedEvent(ConnectedClient client)
        {
            if (client == ConnectedClient.Empty)
            {
                _plugin.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "GoXLR App not connected.", "https://github.com/oddbear/Loupedeck.GoXLR.Plugin/");
            }
            else
            {
                _plugin.OnPluginStatusChanged(Loupedeck.PluginStatus.Normal, "GoXLR App not connected.", "https://github.com/oddbear/Loupedeck.GoXLR.Plugin/");
            }
        }

        public void ProfileListChangedEvent(Profile[] profiles)
        {
            //
        }

        public void ProfileSelectedChangedEvent(Profile profile)
        {
            //
        }

        public void RoutingStateChangedEvent(Routing routing, global::GoXLR.Server.Enums.State state)
        {
            //
        }
    }
}
