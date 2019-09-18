using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Entrega2_Equipo1
{
    [Serializable]
    public class MirrorFilter : Tool
    {
        public Bitmap ApplyFilter(Bitmap image)
        {
            Bitmap copy = (Bitmap)image.Clone();
            RotateFlipFilter filter = new RotateFlipFilter();
            Bitmap rotated = filter.RotateFlip(copy, RotateFlipType.RotateNoneFlipX);
            Bitmap whiteCanvas = new Bitmap(image.Width*2, image.Height);

            for (int i = 0; i < image.Height; i++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    whiteCanvas.SetPixel(x, i, copy.GetPixel(x, i));
                }
            }

            for (int i = 0; i < image.Height; i++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    whiteCanvas.SetPixel(x + image.Width, i, rotated.GetPixel(x, i));
                }
            }
            return whiteCanvas;
        }
    }
}
