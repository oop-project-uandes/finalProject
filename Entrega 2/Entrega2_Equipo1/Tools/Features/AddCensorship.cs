using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Entrega2_Equipo1
{
    public class AddCensorship : Tool
    {
        private Brush brush;

        public AddCensorship()
        {
            this.brush = new Brush();
        }

        // Coordinates must be given as (X,Y,TOP,LEFT)
        public Bitmap blackCensorship(Bitmap image, int[] coordinates)
        {
            Color black = Color.FromArgb(0, 0, 0);
            Bitmap returningImage = brush.paintRectangle(image, black, coordinates);
            return returningImage;
        }
    }
}
