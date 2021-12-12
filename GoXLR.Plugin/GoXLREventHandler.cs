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
        public void ConnectedClientChangedEvent(ConnectedClient client)
        {
            //
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
