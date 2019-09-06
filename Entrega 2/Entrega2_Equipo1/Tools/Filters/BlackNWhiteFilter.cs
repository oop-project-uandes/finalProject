using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Entrega2_Equipo1
{
    public class BlackNWhiteFilter : Tool
    {
        public BlackNWhiteFilter() { }

        public Bitmap ApplyFilter(Bitmap image)
        {
            Bitmap copy = (Bitmap)image.Clone();
            int aux;
            Color color;

            for (int i = 0; i < copy.Height; i++)
            {
                for (int x = 0; x < copy.Width; x++)
                {
                    color = copy.GetPixel(x, i);
                    aux = (int)(0.59 * color.G + 0.11 * color.B + 0.29 * color.R);
                    copy.SetPixel(x, i, Color.FromArgb(aux, aux, aux));
                }
            }
            return copy;
        }
    }
}
