using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Entrega2_Equipo1
{
    [Serializable]
    public class OldFilmFilter : Tool
    {
        public OldFilmFilter() { } 

        public Bitmap ApplyFilter(Bitmap image, int noise = 60)
        {
            SepiaFilter filter = new SepiaFilter();
            Merger mergeTool = new Merger();
            BlackNWhiteFilter blackNwhite = new BlackNWhiteFilter();
            Bitmap sepiaImage = mergeTool.Merge(blackNwhite.ApplyFilter(image), filter.ApplyFilter(image), 25);
            Random TempRandom = new Random();
            Random WhitePixelRandom = new Random();
            for (int x = 0; x < sepiaImage.Width; ++x)
            {
                for (int y = 0; y < sepiaImage.Height; ++y)
                {
                    int whitePixel = WhitePixelRandom.Next(1, 60);
                    if (whitePixel == 5)
                    {
                        sepiaImage.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        continue;
                    }
                    Color CurrentPixel = sepiaImage.GetPixel(x, y);
                    int R = CurrentPixel.R + TempRandom.Next(-noise, noise+1);
                    int G = CurrentPixel.G + TempRandom.Next(-noise, noise+1);
                    int B = CurrentPixel.B + TempRandom.Next(-noise, noise+1);
                    if (R > 255)
                    {
                        R = 255;
                    }
                    else if (R < 0)
                    {
                        R = 0;
                    }
                    if (G > 255)
                    {
                        G = 255;
                    }
                    else if (G < 0)
                    {
                        G = 0;
                    }
                    if (B > 255)
                    {
                        B = 255;
                    }
                    else if (B < 0)
                    {
                        B = 0;
                    }
                    Color TempValue = Color.FromArgb(R, G, B);
                    sepiaImage.SetPixel(x, y, Color.FromArgb(R, G, B));
                }
            }
            return sepiaImage;
        }
    }
}
