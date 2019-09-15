using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Entrega2_Equipo1
{
    public class RotateFlipFilter : Tool
    {
        public Bitmap RotateFlip(Bitmap image, RotateFlipType type)
        {
            Bitmap copy = (Bitmap)image.Clone();
            copy.RotateFlip(type);
            return copy;
        }
    }
}
