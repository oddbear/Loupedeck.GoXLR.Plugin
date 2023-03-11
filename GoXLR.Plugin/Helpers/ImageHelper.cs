using System.IO;
using Loupedeck.GoXLR.Plugin.GoXLR.Enums;
using Loupedeck.GoXLR.Plugin.GoXLR.Extensions;
using Loupedeck.GoXLR.Plugin.GoXLR.Models;
using SkiaSharp;

namespace Loupedeck.GoXLR.Plugin.Helpers
{
    public static class ImageHelper
    {
        //PluginImageSize.Width90: Button 80
        //PluginImageSize.Width60: Knob 50

        /// <summary>
        /// Create a profile image with given variables to dynamically generate new images.
        /// Basic functionality for image styles later, if there can be more then a list etc.
        /// (Own image from Path, choose colors, choose text)
        /// </summary>
        /// <param name="widthHeight">Image size</param>
        /// <param name="text">Displayed text</param>
        /// <param name="fontColor">Font color</param>
        /// <param name="selectedColor">Selected profile color</param>
        /// <param name="notSelectedColor">Not selected profile color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="notAvailable">Not available color</param>
        /// <param name="isSelected">Whether profile is selected</param>
        /// <param name="selectedPath">Path to selected profile image</param>
        /// <param name="unSelectedPath">Path to not selected profile image</param>
        /// <returns></returns>
        public static byte[] GetProfileImage(int widthHeight,
            string text,
            SKColor fontColor,
            SKColor selectedColor,
            SKColor notSelectedColor,
            SKColor backgroundColor,
            SKColor notAvailable,
            bool? isSelected,
            string selectedPath,
            string unSelectedPath)
        {
            var notConnected = isSelected == null;
            string imagePath;

            if (isSelected != null)
                imagePath = (bool)isSelected ? selectedPath : unSelectedPath;
            else
                imagePath = unSelectedPath;

            var image = GetImageStream(imagePath);
            var sectionHeight = widthHeight / 3f;
            var imageWidthHeight = sectionHeight * 2.75F;

            using (var bitmap = new SKBitmap(widthHeight, widthHeight))
            using (var skImage = SKImage.FromEncodedData(image))
            using (var scaledSkImage = SKImage.Create(new SKImageInfo((int)imageWidthHeight, (int)imageWidthHeight)))
            using (var canvas = new SKCanvas(bitmap))
            using (var paint = new SKPaint())
            using (var paintFont = new SKPaint())
            {
                paint.IsAntialias = true;
                paintFont.Color = fontColor;
                paintFont.IsAntialias = true;
                paintFont.TextSize = 13f; // sectionheight * 1.4f = fontSize
                paintFont.TextAlign = SKTextAlign.Center;
                paintFont.Typeface = SKTypeface.FromFamilyName(
                    SKTypeface.Default.FamilyName,
                    SKFontStyleWeight.Normal,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Italic);
                
                //Fill background
                paint.Color = backgroundColor;
                canvas.DrawRect(0, 0, widthHeight, widthHeight, paint);
                
                //Part 1/3 - 2/3
                var centerX = (widthHeight - imageWidthHeight) / 2;
                var centerY = (imageWidthHeight - sectionHeight * 2) / 2;

                if (!notConnected)
                {
                    skImage.ScalePixels(scaledSkImage.PeekPixels(), SKFilterQuality.None);
                    canvas.DrawImage(scaledSkImage, centerX, -centerY);
                }
                else
                {
                    paint.Color = notAvailable;
                    canvas.DrawLine(widthHeight / 3f, sectionHeight, widthHeight / 1.5f, sectionHeight, paint);
                }

                //Part 3/3
                canvas.Translate(0, widthHeight - sectionHeight);
                paint.Color = imagePath == selectedPath ? selectedColor : notSelectedColor;
                canvas.DrawRect(new SKRect(0, 0, widthHeight, sectionHeight), paint);
                canvas.DrawText(text, widthHeight / 2f, sectionHeight / 2f * 1.3f, paintFont);

                return bitmap.Encode(SKEncodedImageFormat.Png, 75).ToArray();
            }
        }

        /// <summary>
        /// Create a routing image with given variables to dynamically generate new images. (Has input and output displayed)
        /// Basic functionality for image styles later, if there can be more then a list etc.
        /// (Own image from Path, choose colors, choose text)
        /// </summary>
        /// <param name="widthHeight">Image size</param>
        /// <param name="routing">Routing</param>
        /// <param name="state">Routing state</param>
        /// <param name="fontColor">Font color</param>
        /// <param name="selectedColor">Selected routing color</param>
        /// <param name="notSelectedColor">Not selected routing color</param>
        /// <param name="secondaryColor">The secondary color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="notAvailable">Not available color</param>
        /// <param name="selectedPath">Path to selected routing image</param>
        /// <param name="unSelectedPath">Path to not selected profile image</param>
        /// <returns></returns>
        public static byte[] GetRoutingImage(int widthHeight,
            Routing routing,
            State? state,
            SKColor fontColor,
            SKColor selectedColor,
            SKColor notSelectedColor,
            SKColor secondaryColor,
            SKColor backgroundColor,
            SKColor notAvailable,
            string selectedPath,
            string unSelectedPath)
        {
            var imagePath = state == State.On ? selectedPath : unSelectedPath;
            var image = GetImageStream(imagePath);
            var sectionHeight = widthHeight / 3f;
            var imageWidthHeight = sectionHeight * 1.75f;

            using (var bitmap = new SKBitmap(widthHeight, widthHeight))
            using (var skImage = SKImage.FromEncodedData(image))
            using (var scaledSkImage = SKImage.Create(new SKImageInfo((int)imageWidthHeight, (int)imageWidthHeight)))
            using (var canvas = new SKCanvas(bitmap))
            using (var paint = new SKPaint())
            using (var paintFont = new SKPaint())
            {
                paint.IsAntialias = true;
                paintFont.Color = fontColor;
                paintFont.IsAntialias = true;
                paintFont.TextSize = 13f; // sectionheight * 1.4f = fontSize
                paintFont.TextAlign = SKTextAlign.Center;
                paintFont.Typeface = SKTypeface.FromFamilyName(
                    SKTypeface.Default.FamilyName,
                    SKFontStyleWeight.Normal,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Italic);
                
                //Fill background
                paint.Color = backgroundColor;
                canvas.DrawRect(0, 0, widthHeight, widthHeight, paint);
                
                //Part 1/3
                paint.Color = secondaryColor;
                canvas.DrawRect(new SKRect(0, 0, widthHeight, sectionHeight), paint);
                canvas.DrawText(routing.Input.GetDescription(), widthHeight / 2f, sectionHeight / 2f * 1.3f, paintFont);
                
                //Part 2/3
                canvas.Translate(0, sectionHeight);
                var centerX = (widthHeight - imageWidthHeight) / 2;
                var centerY = (widthHeight - sectionHeight * 2) / 3;

                if (state != null)
                {
                    skImage.ScalePixels(scaledSkImage.PeekPixels(), SKFilterQuality.None);
                    canvas.DrawImage(scaledSkImage, centerX, -centerY);
                }
                else
                {
                    paint.Color = notAvailable;
                    canvas.DrawLine(widthHeight / 3f, sectionHeight / 2f, widthHeight / 1.5f, sectionHeight / 2f, paint);
                }

                //Part 3/3
                canvas.Translate(0, sectionHeight);
                paint.Color = imagePath == selectedPath ? selectedColor : notSelectedColor;
                canvas.DrawRect(new SKRect(0, 0, widthHeight, sectionHeight), paint);
                if (routing.Output == RoutingOutput.BroadcastMix)
                    canvas.DrawText("Stream Mix", widthHeight / 2f, sectionHeight / 2f * 1.3f, paintFont);
                else
                    canvas.DrawText(routing.Output.GetDescription(), widthHeight / 2f, sectionHeight / 2f * 1.3f, paintFont);
                

                return bitmap.Encode(SKEncodedImageFormat.Png, 75).ToArray();
            }
        }
        
        /// <summary>
        /// Create a routing image with given variables to dynamically generate new images. (Has only input displayed)
        /// Basic functionality for image styles later, if there can be more then a list etc.
        /// (Own image from Path, choose colors, choose text)
        /// </summary>
        /// <param name="size">Image size</param>
        /// <param name="routing">Routing</param>
        /// <param name="state">Routing state</param>
        /// <param name="fontColor">Font color</param>
        /// <param name="selectedColor">Selected routing color</param>
        /// <param name="notSelectedColor">Not selected routing color</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="notAvailable">Not available color</param>
        /// <param name="selectedPath">Path to selected routing image</param>
        /// <param name="unSelectedPath">Path to not selected profile image</param>
        /// <returns></returns>
        public static byte[] GetRoutingImageForFolder(
            PluginImageSize size,
            Routing routing,
            State? state,
            SKColor fontColor,
            SKColor selectedColor,
            SKColor notSelectedColor,
            SKColor backgroundColor,
            SKColor notAvailable,
            string selectedPath,
            string unSelectedPath)
        {
            var imagePath = state == State.On ? selectedPath : unSelectedPath;
            var image = GetImageStream(imagePath);
            var isLarge = size == PluginImageSize.Width90;
            var widthHeight = isLarge ? 80 : 50;
            var sectionHeight = widthHeight / 3f;
            var imageWidthHeight = sectionHeight * 2.75F;

            using (var bitmap = new SKBitmap(widthHeight, widthHeight))
            using (var skImage = SKImage.FromEncodedData(image))
            using (var scaledSkImage = SKImage.Create(new SKImageInfo((int)imageWidthHeight, (int)imageWidthHeight)))
            using (var canvas = new SKCanvas(bitmap))
            using (var paint = new SKPaint())
            using (var paintFont = new SKPaint())
            {
                paint.IsAntialias = true;
                paintFont.Color = fontColor;
                paintFont.IsAntialias = true;
                paintFont.TextSize = 13f; // sectionheight * 1.4f = fontSize
                paintFont.TextAlign = SKTextAlign.Center;
                paintFont.Typeface = SKTypeface.FromFamilyName(
                    SKTypeface.Default.FamilyName,
                    SKFontStyleWeight.Normal,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Italic);
                //Fill background
                paint.Color = backgroundColor;
                canvas.DrawRect(0, 0, widthHeight, widthHeight, paint);
                
                //Part 1/3 - 2/3
                var centerX = (widthHeight - imageWidthHeight) / 2;
                var centerY = (imageWidthHeight - sectionHeight * 2) / 2;

                if (state != null)
                {
                    skImage.ScalePixels(scaledSkImage.PeekPixels(), SKFilterQuality.None);
                    canvas.DrawImage(scaledSkImage, centerX, -centerY);
                }
                else
                {
                    paint.Color = notAvailable;
                    canvas.DrawLine(widthHeight / 3f, sectionHeight, widthHeight / 1.5f, sectionHeight, paint);
                }

                //Part 3/3
                canvas.Translate(0, widthHeight - sectionHeight);
                paint.Color = imagePath == selectedPath ? selectedColor : notSelectedColor;
                canvas.DrawRect(new SKRect(0, 0, widthHeight, sectionHeight), paint);
                canvas.DrawText(routing.Input.GetDescription(), widthHeight / 2f, sectionHeight / 2f * 1.3f, paintFont);

                return bitmap.Encode(SKEncodedImageFormat.Png, 75).ToArray();
            }
        }

        private static Stream GetImageStream(string path)
        {
            return path.Contains("Loupedeck.GoXLR.Plugin.EmbeddedResources.")
                ? EmbeddedResources.GetStream(path)
                : File.OpenRead(path);
        }
    }
}