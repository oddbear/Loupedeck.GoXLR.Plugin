namespace Loupedeck.GoXLR.Plugin.Config.Models
{
    public class PluginSettings
    {
        public string IconNote { get; set; }
        public string ColorNote { get; set; }
        public RoutingSettings Routing { get; set; }
        public BaseProfileSettings Profile { get; set; }
        public RoutingFolderSettings RoutingFolder { get; set; }
        public BaseProfileSettings ProfileFolder { get; set; }
    }
}