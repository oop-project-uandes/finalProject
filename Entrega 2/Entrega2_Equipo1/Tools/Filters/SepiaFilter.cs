using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Entrega2_Equipo1
{
    public class SepiaFilter : Tool
    {
        public SepiaFilter() { }

        public Bitmap ApplyFilter(Bitmap image)
        {
            Bitmap copy = (Bitmap)image.Clone();
            int new_red;
            int new_blue;
            int new_green;
            Color pixelColor;

            for (int i = 0; i < copy.Height; i++)
            {
                for (int x = 0; x < copy.Width; x++)
                {
                    
                    pixelColor = copy.GetPixel(x, i);
                    new_red = (int)Math.Round(.393 * pixelColor.R + .769 * pixelColor.G + .189 * pixelColor.B);
                    new_green = (int)Math.Round(.349 * pixelColor.R + .686 * pixelColor.G + .168 * pixelColor.B);
                    new_blue = (int)Math.Round(.272 * pixelColor.R + .534 * pixelColor.G + .131 * pixelColor.B);

                    if (new_red > 255)
                    {
                        new_red = 255;
                    }

                    if (new_green > 255)
                    {
                        new_green = 255;
                    }

                    if (new_blue > 255)
                    {
                        new_blue = 255;
                    }

                    copy.SetPixel(x, i, Color.FromArgb(new_red, new_green, new_blue));
                }
            }
            return copy;
        }
    }
}
