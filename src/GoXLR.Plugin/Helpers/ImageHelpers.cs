namespace Loupedeck.GoXLRPlugin.Helpers
{
    using System;
    using GoXLR.Server.Enums;
    using GoXLR.Server.Models;

    public static class ImageHelpers
    {
        private static BitmapColor DarkSlateGray = new (0x2F, 0x4F, 0x4F);
        private static BitmapColor OrangeRed = new (0xFF, 0x45, 0x00);
        private static BitmapColor Red = new (0xFF, 0x00, 0x00);

        public static int GetImageSize(PluginImageSize imageSize)
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

        public static byte[] GetProfileImage2(PluginImageSize imageSize, string profileName, bool isSelected)
        {
            var checkedImage = BitmapImage.FromResource(typeof(GoXlrPlugin).Assembly,"Loupedeck.GoXLRPlugin.Resources.Commands.checked-80.png");

            var isLarge = imageSize == PluginImageSize.Width90;

            var width = isLarge ? 80 : 50;
            var height = isLarge ? 80 : 50;
            var fontSize = isLarge ? 11 : 6;

            using var bitmapBuilder = new BitmapBuilder(width, height);

            // Fill background:
            bitmapBuilder.Clear(BitmapColor.Black);

            // Part 1:
            var sectionHeightFloat = height / 3f;
            var sectionHeightInt = (int)Math.Round(sectionHeightFloat);

            if (isSelected)
            {
                // x -> sectionHeightInt -> one third
                bitmapBuilder.DrawImage(checkedImage, sectionHeightInt, 0, sectionHeightInt, sectionHeightInt);
            }

            // Part 2:
            bitmapBuilder.Translate(0, sectionHeightFloat);
            
            bitmapBuilder.FillRectangle(0, 0, width, sectionHeightInt * 2, DarkSlateGray);
            bitmapBuilder.DrawLine(0, 0, width, 0, OrangeRed, 1);

            //Draw Input string:
            bitmapBuilder.DrawText(profileName, 0, 0, width, (int)(sectionHeightInt * 1.6f), BitmapColor.White, fontSize);

            return bitmapBuilder.ToArray();
        }

        public static byte[] GetRoutingImage2(PluginImageSize imageSize, Routing routing, State? state)
        {
            var checkedImage = BitmapImage.FromResource(typeof(GoXlrPlugin).Assembly, "Loupedeck.GoXLRPlugin.Resources.Commands.checked-80.png");

            var textInput = routing.Input.ToString();
            var textOutput = routing.Output.ToString();

            var isLarge = imageSize == PluginImageSize.Width90;

            var width = isLarge ? 80 : 50;
            var height = isLarge ? 80 : 50;
            var fontSize = isLarge ? 11 : 6;

            // TODO: We should probably calculate this:
            var fontSkip = isLarge ? 20 : 8;

            using var bitmapBuilder = new BitmapBuilder(width, height);

            //Fill background:
            bitmapBuilder.Clear(BitmapColor.Black);

            var sectionHeightFloat = height / 3f;
            var sectionHeightInt = (int)Math.Round(sectionHeightFloat);

            //Part 1:
            bitmapBuilder.FillRectangle(0, 0, width, sectionHeightInt, DarkSlateGray);

            //Draw Input string:
            bitmapBuilder.DrawText(textInput, 0, 0, width, fontSkip, BitmapColor.White, fontSize);

            //Part 2:
            bitmapBuilder.Translate(0, sectionHeightFloat);

            bitmapBuilder.DrawLine(0, 0, width, 0, OrangeRed, 1);
            if (state == State.On)
            {
                bitmapBuilder.DrawImage(checkedImage, (int)(width / 3f), 0, (int)(width / 3f), sectionHeightInt);
            }
            else if (state == null)
            {
                // If not connected.
                bitmapBuilder.DrawLine(width / 3f, sectionHeightFloat / 2, width / 1.5f, sectionHeightFloat / 2, Red, 3);
            }
            bitmapBuilder.DrawLine(0, sectionHeightFloat - 1, width, sectionHeightFloat - 1, OrangeRed, 1);

            //Part 3:
            bitmapBuilder.Translate(0, sectionHeightFloat);
            bitmapBuilder.FillRectangle(0, 0, width, sectionHeightInt, DarkSlateGray); // sectionArea

            //Draw Output string:
            bitmapBuilder.DrawText(textOutput, 0, 0, width, fontSkip, BitmapColor.White, fontSize);

            return bitmapBuilder.ToArray();
        }
    }
}
