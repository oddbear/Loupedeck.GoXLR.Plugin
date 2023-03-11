using System.IO;
using SkiaSharp;

namespace Loupedeck.GoXLR.Plugin.Config.Models
{
    public class BaseProfileSettings
    {
        public string SelectedIconPath { get; }
        public string NotSelectedIconPath { get; }
        public SKColor FontColor  { get; }
        public SKColor SelectedProfileColor { get; }
        public SKColor NotSelectedProfileColor { get; }
        public SKColor BackgroundColor  { get; }
        public SKColor NotAvailable { get; }
        
        public BaseProfileSettings(
            string selectedIconPath,
            string notSelectedIconPath,
            string fontColor,
            string selectedProfileColor,
            string notSelectedProfileColor,
            string backgroundColor,
            string notAvailable)
        {
            SelectedIconPath = !string.IsNullOrWhiteSpace(selectedIconPath) && File.Exists(selectedIconPath) ? selectedIconPath : GoXlrConfig.Profile.SelectedIconPath;
            NotSelectedIconPath = !string.IsNullOrWhiteSpace(notSelectedIconPath) && File.Exists(notSelectedIconPath) ? notSelectedIconPath : GoXlrConfig.Profile.NotSelectedIconPath;
            
            FontColor = SKColor.TryParse(fontColor.Replace("#", "ff"), out var result1)
                ? result1
                : GoXlrConfig.Profile.Color.Font;
            SelectedProfileColor = SKColor.TryParse(selectedProfileColor.Replace("#", "ff"), out var result2)
                ? result2
                : GoXlrConfig.Profile.Color.Selected;
            NotSelectedProfileColor = SKColor.TryParse(notSelectedProfileColor.Replace("#", "ff"), out var result3)
                ? result3
                : GoXlrConfig.Profile.Color.NotSelected;
            BackgroundColor = SKColor.TryParse(backgroundColor.Replace("#", "ff"), out var result4)
                ? result4
                : GoXlrConfig.Profile.Color.Background;
            NotAvailable = SKColor.TryParse(notAvailable.Replace("#", "ff"), out var result5)
                ? result5
                : GoXlrConfig.Profile.Color.NotAvailable;
        }
    }
}