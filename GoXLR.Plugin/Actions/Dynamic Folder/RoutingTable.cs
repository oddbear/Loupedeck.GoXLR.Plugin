using Loupedeck.GoXLR.Plugin.Actions.Dynamic_Folder.Base_Dynamic_Folder;
using Loupedeck.GoXLR.Plugin.GoXLR.Enums;

namespace Loupedeck.GoXLR.Plugin.Actions.Dynamic_Folder
{
    public class ChatMicRoutingTable : BaseRoutingTable
    {
        public ChatMicRoutingTable()
        {
            DisplayName = "Chat Mic";
            GroupName = "Dynamic Folder";
            Description = "Description";
            Output = RoutingOutput.ChatMic;
        }
    }

    public class HeadphonesRoutingTable : BaseRoutingTable
    {
        public HeadphonesRoutingTable()
        {
            DisplayName = "Headphones";
            GroupName = "Dynamic Folder";
            Description = "Description";
            Output = RoutingOutput.Headphones;
        }
    }

    public class LineOutRoutingTable : BaseRoutingTable
    {
        public LineOutRoutingTable()
        {
            DisplayName = "Line Out";
            GroupName = "Dynamic Folder";
            Description = "Description";
            Output = RoutingOutput.LineOut;
        }
    }

    public class SamplerRoutingTable : BaseRoutingTable
    {
        public SamplerRoutingTable()
        {
            DisplayName = "Sampler";
            GroupName = "Dynamic Folder";
            Description = "Description";
            Output = RoutingOutput.Sampler;
        }
    }

    public class StreamMixRoutingTable : BaseRoutingTable
    {
        public StreamMixRoutingTable()
        {
            DisplayName = "Stream Mix";
            GroupName = "Dynamic Folder";
            Description = "Description";
            Output = RoutingOutput.BroadcastMix;
        }
    }
}