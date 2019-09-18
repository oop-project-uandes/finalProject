using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Entrega2_Equipo1
{
    [Serializable]
    public class ColorFilter : Tool
    {
        public Bitmap ApplyFilter(Bitmap image, EColorFilterTypes type)
        {
            Bitmap copy = (Bitmap)image.Clone();
            Color color;
            int new_red;
            int new_green;
            int new_blue;
            for (int i = 0; i < copy.Height; i++)
            {
                for (int x = 0; x < copy.Width; x++)
                {
                    color = copy.GetPixel(x, i);
                    if (type == EColorFilterTypes.Red)
                    {
                        new_red = color.R;
                        new_green = 0;
                        new_blue = 0;
                        copy.SetPixel(x, i, Color.FromArgb(new_red, new_green, new_blue));
                    }
                    else if (type == EColorFilterTypes.Green)
                    {
                        new_red = 0;
                        new_green = color.G;
                        new_blue = 0;
                        copy.SetPixel(x, i, Color.FromArgb(new_red, new_green, new_blue));
                    }
                    else if (type == EColorFilterTypes.Blue)
                    {
                        new_red = 0;
                        new_green = 0;
                        new_blue = color.B;
                        copy.SetPixel(x, i, Color.FromArgb(new_red, new_green, new_blue));
                    }
                    else if (type == EColorFilterTypes.Yellow)
                    {
                        new_red = color.R;
                        new_green = color.G;
                        new_blue = 0;
                        copy.SetPixel(x, i, Color.FromArgb(new_red, new_green, new_blue));
                    }
                }
            }
            return copy;
        }

        public Bitmap ApplyFilter(Bitmap image, Color usrColor)
        {
            Bitmap copy = (Bitmap)image.Clone();
            Color imgColor;
            int new_red;
            int new_green;
            int new_blue;

            for (int i = 0; i < copy.Height; i++)
            {
                for (int x = 0; x < copy.Width; x++)
                {
                    imgColor = copy.GetPixel(x, i);
                    new_red = (usrColor.R * imgColor.R) / 255;
                    new_green = (usrColor.G * imgColor.G) / 255;
                    new_blue = (usrColor.B * imgColor.B) / 255;
                    copy.SetPixel(x, i, Color.FromArgb(new_red, new_green, new_blue));
                }
            }
            return copy;
        }
    }
}
