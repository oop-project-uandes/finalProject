using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Entrega2_Equipo1
{
    public class WindowsFilter : Tool
    {
        public Bitmap ApplyFilter(Bitmap image)
        {
            ColorFilter filter = new ColorFilter();
            Bitmap redImage = filter.ApplyFilter(image, EColorFilterTypes.Red);
            Bitmap blueImage = filter.ApplyFilter(image, EColorFilterTypes.Blue);
            Bitmap greenImage = filter.ApplyFilter(image, EColorFilterTypes.Green);
            Bitmap yellowImage = filter.ApplyFilter(image, EColorFilterTypes.Yellow);

            Bitmap whiteCanvas = new Bitmap(image.Width*2, image.Height*2);
            for (int i = 0; i < image.Height; i++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    whiteCanvas.SetPixel(x, i, redImage.GetPixel(x,i));
                }
            }

            for (int i = 0; i < image.Height; i++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    whiteCanvas.SetPixel(x+image.Width, i, greenImage.GetPixel(x, i));
                }
            }

            for (int i = 0; i < image.Height; i++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    whiteCanvas.SetPixel(x, i+image.Height, blueImage.GetPixel(x, i));
                }
            }

            for (int i = 0; i < image.Height; i++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    whiteCanvas.SetPixel(x+image.Width, i + image.Height, yellowImage.GetPixel(x, i));
                }
            }
            return whiteCanvas;
        }
    }
}
