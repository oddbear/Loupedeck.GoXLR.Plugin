using SkiaSharp;

namespace Loupedeck.GoXLR.Plugin.Config
{
    public static class GoXlrConfig
    {
        public const string SettingsFile = "PluginSettings.json";
        public const string PluginSettingsFile = "Loupedeck.GoXLR.Plugin.EmbeddedResources.PluginSettings.json";
        public static readonly RoutingDefaultSettings Routing = new RoutingDefaultSettings();
        public static readonly ProfileDefaultSettings Profile = new ProfileDefaultSettings();
    }

    public class RoutingDefaultSettings
    {
        public readonly string ActiveIconPath = "Loupedeck.GoXLR.Plugin.EmbeddedResources.VolumeMain.png";
        public readonly string InActiveIconPath = "Loupedeck.GoXLR.Plugin.EmbeddedResources.VolumeMuteMain.png";
        public readonly RoutingDefaultColors Color = new RoutingDefaultColors();
    }

    public class ProfileDefaultSettings
    {
        public readonly string SelectedIconPath = "Loupedeck.GoXLR.Plugin.EmbeddedResources.UserProfile.png";
        public readonly string NotSelectedIconPath = "Loupedeck.GoXLR.Plugin.EmbeddedResources.ProfileLock.png";
        public readonly ProfileDefaultColors Color = new ProfileDefaultColors();
    }

    public class RoutingDefaultColors
    {
        public readonly SKColor Font = SKColors.White;
        public readonly SKColor Active = SKColors.Green;
        public readonly SKColor InActive = SKColors.DarkSlateGray;
        public readonly SKColor Secondary = SKColors.DarkSlateGray;
        public readonly SKColor Background = SKColors.Black;
        public readonly SKColor NotAvailable = SKColors.Red;
    }

    public class ProfileDefaultColors
    {
        public readonly SKColor Font = SKColors.White;
        public readonly SKColor Selected = SKColors.Green;
        public readonly SKColor NotSelected = SKColors.DarkSlateGray;
        public readonly SKColor Background = SKColors.Black;
        public readonly SKColor NotAvailable = SKColors.Red;
    }
}