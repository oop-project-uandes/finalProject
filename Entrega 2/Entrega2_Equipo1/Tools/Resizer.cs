using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Entrega2_Equipo1
{
    public class Resizer : Tool
    {
        public Resizer() { }

        public Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            Bitmap returningImage = new Bitmap(width, height);
            Rectangle rect = new Rectangle(0, 0, width, height);
            returningImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            Graphics graphics = Graphics.FromImage(returningImage);
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            ImageAttributes wrapMode = new ImageAttributes();
            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
            graphics.DrawImage(image, rect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            return returningImage;
        }
    }
}
