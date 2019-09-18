using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Entrega2_Equipo1
{
    [Serializable]
    public class InvertFilter : Tool
    {
        public InvertFilter() { }

        public Bitmap ApplyFilter(Bitmap image)
        {
            Bitmap copy = (Bitmap)image.Clone();
            Color color;
            for (int i = 0; i < copy.Height; i++)
            {
                for (int x = 0; x < copy.Width; x++)
                {
                    color = copy.GetPixel(x, i);
                    copy.SetPixel(x, i, Color.FromArgb(255-color.R, 255-color.G, 255-color.B));
                }
            }
            return copy;
        }
    }
}
