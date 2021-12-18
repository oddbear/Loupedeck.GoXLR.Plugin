namespace Loupedeck.GoXLRPlugin.Helpers
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    using GoXLR.Server.Enums;
    using GoXLR.Server.Models;

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

        public static Byte[] GetProfileImage2(PluginImageSize imageSize, String profileName, Boolean isSelected)
        {
            var checkedImage = EmbeddedResources.GetStream("Loupedeck.GoXLRPlugin.Resources.Commands.checked-80.png");

            var isLarge = imageSize == PluginImageSize.Width90;

            var width = isLarge ? 80 : 50;
            var height = isLarge ? 80 : 50;
            var fontSize = isLarge ? 9 : 6;

            var fontColor = Color.White;
            var stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            using (var bitmap = new Bitmap(width, height))
            using (var checkedBitmap = new Bitmap(checkedImage))
            using (var graphics = Graphics.FromImage(bitmap))
            using (var font = new Font(SystemFonts.DefaultFont.FontFamily, fontSize, FontStyle.Regular))
            using (var memoryStream = new MemoryStream())
            {
                //Fill background:
                graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, width, height);

                //Part 1:
                var sectionHeight = height / 3f;

                if (isSelected)
                {
                    graphics.DrawImage(checkedBitmap, sectionHeight, 0, sectionHeight, sectionHeight);
                }
                
                //Part 2:
                graphics.TranslateTransform(0, sectionHeight);
                sectionHeight *= 2;

                graphics.FillRectangle(new SolidBrush(Color.DarkSlateGray), 0, 0, width, sectionHeight);
                graphics.DrawLine(new Pen(Color.OrangeRed), 0, 0, width, 0);

                //Draw Input string:
                var rectangleF = new RectangleF(0, 0, width, sectionHeight);
                graphics.DrawString(profileName, font, new SolidBrush(fontColor), rectangleF, stringFormat);
                
                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;

                return memoryStream.ToArray();
            }
        }

        public static Byte[] GetProfileImage(PluginImageSize imageSize, String profileName, Boolean isSelected)
        {
            var checkedImage = EmbeddedResources.GetStream("Loupedeck.GoXLRPlugin.Resources.Commands.checked-80.png");

            var isLarge = imageSize == PluginImageSize.Width90;

            var width = isLarge ? 80 : 50;
            var height = isLarge ? 80 : 50;
            var fontSize = isLarge ? 9 : 6;

            var fontColor = Color.White;
            var stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            using (var bitmap = new Bitmap(width, height))
            using (var checkedBitmap = new Bitmap(checkedImage))
            using (var graphics = Graphics.FromImage(bitmap))
            using (var font = new Font(SystemFonts.DefaultFont.FontFamily, fontSize, FontStyle.Regular))
            using (var memoryStream = new MemoryStream())
            {
                //Fill background:
                graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, width, height);

                //Part 1:
                var sectionHeight = height / 1.5f;

                graphics.FillRectangle(new SolidBrush(Color.DarkSlateGray), 0, 0, width, sectionHeight);
                graphics.DrawLine(new Pen(Color.OrangeRed), 0, sectionHeight, width, sectionHeight);

                //Draw Input string:
                var rectangleF = new RectangleF(0, 0, width, sectionHeight);
                graphics.DrawString(profileName, font, new SolidBrush(fontColor), rectangleF, stringFormat);

                //Part 2:
                graphics.TranslateTransform(0, sectionHeight);
                sectionHeight /= 2;
                if (isSelected)
                {
                    graphics.DrawImage(checkedBitmap, sectionHeight, 0, sectionHeight, sectionHeight);
                }
                
                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;

                return memoryStream.ToArray();
            }
        }

        public static Byte[] GetRoutingImage2(PluginImageSize imageSize, Routing routing, State? state)
        {
            var checkedImage = EmbeddedResources.GetStream("Loupedeck.GoXLRPlugin.Resources.Commands.checked-80.png");

            var textInput = routing.Input.ToString();
            var textOutput = routing.Output.ToString();

            var isLarge = imageSize == PluginImageSize.Width90;

            var width = isLarge ? 80 : 50;
            var height = isLarge ? 80 : 50;
            var fontSize = isLarge ? 9 : 6;

            var lineColor = Color.OrangeRed;
            var fontColor = Color.White;

            using (var bitmap = new Bitmap(width, height))
            using (var checkedBitmap = new Bitmap(checkedImage))
            using (var graphics = Graphics.FromImage(bitmap))
            using (var font = new Font(SystemFonts.DefaultFont.FontFamily, fontSize, FontStyle.Regular))
            using (var memoryStream = new MemoryStream())
            {
                //Fill background:
                graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, width, height);

                //graphics.DrawImage(xBitmap, 40, 0, 40, 40);
                var sectionHeight = height / 3f;
                var sectionArea = new RectangleF(0, 0, width, sectionHeight);

                //Part 1:
                graphics.FillRectangle(new SolidBrush(Color.DarkSlateGray), sectionArea);

                //Draw Input string:
                var measureInput = graphics.MeasureString(textInput, font);
                var centerXInput = (width - measureInput.Width) / 2f;
                var centerYInput = (sectionHeight - measureInput.Height) / 2f;
                graphics.DrawString(textInput, font, new SolidBrush(fontColor), centerXInput, centerYInput);

                //Part 2:
                graphics.TranslateTransform(0, sectionHeight);

                graphics.DrawLine(new Pen(lineColor), 0, 0, width, 0);
                if (state == State.On)
                {
                    graphics.DrawImage(checkedBitmap, width / 3f, 0, width / 3f, sectionHeight);
                }
                else if(state == null)
                {
                    graphics.DrawLine(new Pen(Color.Red, 3), width / 3f, sectionHeight / 2, width / 1.5f, sectionHeight / 2);
                }
                graphics.DrawLine(new Pen(lineColor), 0, height / 3, width, height / 3);

                //Part 3:
                graphics.TranslateTransform(0, sectionHeight);
                graphics.FillRectangle(new SolidBrush(Color.DarkSlateGray), sectionArea);

                //Draw Output string:
                var measureOutput = graphics.MeasureString(textOutput, font);
                var centerXOutput = (width - measureOutput.Width) / 2f;
                var centerYOutput = (sectionHeight - measureOutput.Height) / 2f;
                graphics.DrawString(textOutput, font, new SolidBrush(fontColor), centerXOutput, centerYOutput);

                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;

                return memoryStream.ToArray();
            }
        }

        private static Image BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (var ms = new MemoryStream(bitmapImage.ToArray()))
            {
                return Bitmap.FromStream(ms);
            }
        }

        public static Byte[] GetRoutingImage(PluginImageSize imageSize, Routing routing, State? state)
        {
            var isLarge = imageSize == PluginImageSize.Width90;
            var width = isLarge ? 80 : 50;
            var height = isLarge ? 80 : 50;
            var fontSize = isLarge ? 10 : 7;

            var textInput = routing.Input.ToString();
            var textOutput = routing.Output.ToString();

            var fontColor = Color.White;
            var lineColor = Color.OrangeRed;
            var crossColor = Color.Red;

            var lineWidth = 2;
            var crossWidth = 2;
            var marginBottom = 3;

            using (var bitmap = new Bitmap(width, height))
            using (var memoryStream = new MemoryStream())
            using (var graphics = Graphics.FromImage(bitmap))
            using (var font = new Font(SystemFonts.DefaultFont.FontFamily, fontSize, FontStyle.Regular))
            {
                //Fill background:
                graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, width, height);

                //Draw Input string:
                var measureInput = graphics.MeasureString(textInput, font);
                graphics.DrawString(textInput, font, new SolidBrush(fontColor), (width / 2) - measureInput.Width / 2, 0);

                //Draw Output string:
                var measureOutput = graphics.MeasureString(textOutput, font);
                graphics.DrawString(textOutput, font, new SolidBrush(fontColor), (width / 2) - measureOutput.Width / 2, height - measureOutput.Height - marginBottom);

                //Set drawspace:
                var topTextHeight = measureInput.Height;
                var bottomTextHeight = measureOutput.Height + marginBottom;
                var canvasHeight = height - topTextHeight - bottomTextHeight;

                //Set canvas (area to paint on that is not text):
                graphics.TranslateTransform(0, topTextHeight);
                var canvas = new RectangleF(0, 0, width, canvasHeight);

                //Draw top and bottom line:
                graphics.DrawLine(new Pen(lineColor, lineWidth), canvas.Left, canvas.Top, canvas.Right, canvas.Top);
                graphics.DrawLine(new Pen(lineColor, lineWidth), canvas.Left, canvas.Bottom, canvas.Right, canvas.Bottom);

                var lineLeft = (canvas.Width / 2f) - 3f;
                var lineRight = canvas.Width / 2f + 3f;

                var lineTop = canvas.Height / 2f - 7f;
                var lineBottom = canvas.Height / 2f + 7f;

                var crossLeft = canvas.Width / 3f;
                var crossRight = (canvas.Width / 3f) * 2f;
                var crossTop = canvasHeight / 2 + (crossRight - crossLeft) / 2;
                var crossBottom = canvasHeight / 2 - (crossRight - crossLeft) / 2;

                switch (state)
                {
                    case State.Off:

                        //Line top:
                        graphics.DrawLine(new Pen(lineColor, lineWidth), lineLeft, canvas.Top, lineLeft, lineTop);
                        graphics.DrawLine(new Pen(lineColor, lineWidth), lineRight, canvas.Top, lineRight, lineTop);

                        //Line bottom:
                        graphics.DrawLine(new Pen(lineColor, lineWidth), lineLeft, lineBottom, lineLeft, canvas.Bottom);
                        graphics.DrawLine(new Pen(lineColor, lineWidth), lineRight, lineBottom, lineRight, canvas.Bottom);

                        //Cross:
                        graphics.DrawLine(new Pen(crossColor, crossWidth), crossLeft, crossTop, crossRight, crossBottom);
                        graphics.DrawLine(new Pen(crossColor, crossWidth), crossLeft, crossBottom, crossRight, crossTop);

                        break;
                    case State.On:

                        graphics.DrawLine(new Pen(lineColor, lineWidth), lineLeft, canvas.Top, lineLeft, canvas.Bottom);
                        graphics.DrawLine(new Pen(lineColor, lineWidth), lineRight, canvas.Top, lineRight, canvas.Bottom);

                        break;
                    case null:

                        graphics.DrawLine(new Pen(crossColor, crossWidth), crossLeft, canvas.Height / 2, crossRight, canvas.Height / 2);

                        break;
                }
                
                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;

                return memoryStream.ToArray();
            }
        }
    }
}
