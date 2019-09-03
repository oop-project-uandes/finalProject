using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    public class FacePosition
    {
        double width;
        double height;
        double top;
        double left;
        public FacePosition(double width, double height, double top, double left)
        {
            this.width = width;
            this.height = height;
            this.top = top;
            this.left = left;
        }
        public double Width { get => width; set => width = value; }
        public double Height { get => height; set => height = value; }
        public double Top { get => top; set => top = value; }
        public double Left { get => left; set => left = value; }
    }
}
