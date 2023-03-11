using System.ComponentModel;

namespace Loupedeck.GoXLR.Plugin.GoXLR.Enums
{
    public enum RoutingOutput
    {
        [Description("Headphones")]
        Headphones,
        
        [Description("Broadcast Mix")]
        BroadcastMix,
        
        [Description("Line Out")]
        LineOut, 
        
        [Description("Chat Mic")]
        ChatMic,
        
        [Description("Sampler")]
        Sampler
    }
}