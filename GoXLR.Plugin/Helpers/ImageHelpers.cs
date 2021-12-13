namespace Loupedeck.GoXLRPlugin.Helpers
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    public static class ImageHelpers
    {
        public static Int32 GetImageSize(PluginImageSize imageSize)
        {
            switch (imageSize)
            {
                //PluginImageSize.Width90: Button
                case PluginImageSize.Width90:
                    return 80;
                //PluginImageSize.Width60: Knob
                case PluginImageSize.Width60:
                    return 50;
                default:
                    return 80;
            }
        }

        public static Byte[] GetImageBackground(PluginImageSize imageSize, Color color)
        {
            var size = GetImageSize(imageSize);

            using (var bitmap = new Bitmap(size, size))
            using (var memoryStream = new MemoryStream())
            {
                for (var y = 0; y < size; y++)
                {
                    for (var x = 0; x < size; x++)
                    {
                        bitmap.SetPixel(x, y, color);
                    }
                }

                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;

                return memoryStream.ToArray();
            }
        }
    }
}
