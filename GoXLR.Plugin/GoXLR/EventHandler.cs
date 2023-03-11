using System;
using Loupedeck.GoXLR.Plugin.GoXLR.EventArgs;

namespace Loupedeck.GoXLR.Plugin.GoXLR
{
    public static class EventHandler
    {
        /// <summary>
        /// Event once the profiles has changed
        /// </summary>
        public static EventHandler<OnProfilesChangedEventArgs> OnProfilesChanged;
        
        /// <summary>
        /// Event once the profile selection has changed
        /// </summary>
        public static EventHandler<OnProfileSelectedEventArgs> OnProfileSelected;
        
        /// <summary>
        /// Event once the routing has changed
        /// </summary>
        public static EventHandler<OnRoutingChangedEventArgs> OnRoutingChanged;
        
        /// <summary>
        /// Event once the GoXLR is connected to invoke an ImageChanged
        /// </summary>
        public static EventHandler<System.EventArgs> RefreshIcons;
    }
}