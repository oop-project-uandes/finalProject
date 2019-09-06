using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    public class Brush : Tool
    {
        public Brush() { }

        // Method to paint a rectangle over an image
        // Coordinates must be given as (X,Y,TOP,LEFT)
        public Bitmap paintRectangle(Bitmap image, Color color, int[] coordinates)
        {
            if ((coordinates[0] + coordinates[3] > image.Width) || (coordinates[1] + coordinates[2] > image.Height)) throw new Exception("Not valid coordinates");
            Bitmap copy = (Bitmap)image.Clone();
            for (int i = coordinates[2]; i < coordinates[2] +coordinates[1]; i++)
            {
                for (int x = coordinates[3]; x < coordinates[3] +coordinates[0]; x++)
                {
                    copy.SetPixel(x, i, color);
                }
            }
            return copy;
        }
    }
}
