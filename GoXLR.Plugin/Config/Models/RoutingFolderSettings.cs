using System.IO;
using SkiaSharp;

namespace Loupedeck.GoXLR.Plugin.Config.Models
{
    public class RoutingFolderSettings
    {
        public string ActiveIconPath { get; }
        public string InActiveIconPath { get; }
        public SKColor FontColor  { get; }
        public SKColor ActiveRoutingColor { get; }
        public SKColor InActiveRoutingColor { get; }
        public SKColor BackgroundColor  { get; }
        public SKColor NotAvailable  { get; }
        
        public RoutingFolderSettings(
            string activeIconPath,
            string inActiveIconPath,
            string fontColor,
            string activeRoutingColor,
            string inActiveRoutingColor,
            string backgroundColor,
            string notAvailable)
        {
            ActiveIconPath = !string.IsNullOrWhiteSpace(activeIconPath) && File.Exists(activeIconPath) ? activeIconPath : GoXlrConfig.Routing.ActiveIconPath;
            InActiveIconPath = !string.IsNullOrWhiteSpace(inActiveIconPath) && File.Exists(inActiveIconPath) ? inActiveIconPath : GoXlrConfig.Routing.InActiveIconPath;
            
            FontColor = SKColor.TryParse(fontColor.Replace("#", "ff"), out var result1)
                ? result1
                : GoXlrConfig.Routing.Color.Font;
            ActiveRoutingColor = SKColor.TryParse(activeRoutingColor.Replace("#", "ff"), out var result2)
                ? result2
                : GoXlrConfig.Routing.Color.Active;
            InActiveRoutingColor = SKColor.TryParse(inActiveRoutingColor.Replace("#", "ff"), out var result3)
                ? result3
                : GoXlrConfig.Routing.Color.InActive;
            BackgroundColor = SKColor.TryParse(backgroundColor.Replace("#", "ff"), out var result4)
                ? result4
                : GoXlrConfig.Routing.Color.Background;
            NotAvailable = SKColor.TryParse(notAvailable.Replace("#", "ff"), out var result5)
                ? result5
                : GoXlrConfig.Routing.Color.NotAvailable;
        }
    }
}