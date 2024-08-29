using GoXLR.Server.Enums;

namespace GoXLR.Server.Models
{
    public interface IGoXLREventHandler
    {
        /// <summary>
        /// Notified that a GoXLR App has connected or disconnected.
        /// </summary>
        void ConnectedClientChangedEvent(in ConnectedClient client);

        /// <summary>
        /// Notifies that the list of profiles has been modified in the GoXLR App.
        /// </summary>
        /// <param name="profiles"></param>
        void ProfileListChangedEvent(Profile[] profiles);

        /// <summary>
        /// Notifies that the selected profile has changed in the GoXLR App.
        /// </summary>
        /// <param name="profile"></param>
        void ProfileSelectedChangedEvent(in Profile profile);

        /// <summary>
        /// Notifies that a specific routing in the routing table of the GoXLR App has changed.
        /// </summary>
        /// <param name="routing"></param>
        /// <param name="state"></param>
        void RoutingStateChangedEvent(in Routing routing, in State state);
    }
}