using Loupedeck.GoXLR.Plugin.GoXLR.Enums;
using Loupedeck.GoXLR.Plugin.GoXLR.Models;

namespace Loupedeck.GoXLR.Plugin.GoXLR.EventArgs
{
    public class OnRoutingChangedEventArgs : System.EventArgs
    {
        public Routing Routing { get; set; }
        public State State { get; set; }
    }
}