using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    [Serializable]
    public class Merger : Tool
    {
        public Merger() { }

        // By default this method merges at 50% the images. p1 is the percentage of img1
        public Bitmap Merge(Bitmap img1, Bitmap img2, int p1=50)
        {
            if (p1 < 0 || p1 > 100)
            {
                throw new Exception("The given merging percentage is not valid. Must be between 1 and 100");
            }

            Color color1, color2;
            double pImg1 = p1 / 100.0;
            double pImg2 = 1 - pImg1;
            for (int i = 0; i < img1.Height; i++)
            {
                for (int x = 0; x < img1.Width; x++)
                {
                    color1 = img1.GetPixel(x, i);
                    color2 = img2.GetPixel(x, i);

                    int new_red = (int)Math.Round(pImg1 * color1.R + pImg2 * color2.R);
                    int new_green = (int)Math.Round(pImg1 * color1.G + pImg2 * color2.G);
                    int new_blue = (int)Math.Round(pImg1 * color1.B + pImg2 * color2.B);
                    img1.SetPixel(x, i, Color.FromArgb(new_red, new_green, new_blue));
                }
            }
            return img1;
        }
    }
}
