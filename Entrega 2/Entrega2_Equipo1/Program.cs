using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Entrega2_Equipo1
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap imagen1 = new Bitmap("C:\\Users\\Gianfranco Lacasella\\Desktop\\Prueba c# watson\\imagen1.jpg");
            Brush brush = new Brush();
            Color color = Color.FromArgb(255, 0, 0);
            Bitmap SP = brush.paintRectangle(imagen1, color, new int[] { 400,600,20,20});
            SP.Save("C:\\Users\\Gianfranco Lacasella\\Desktop\\Prueba c# watson\\painted.jpg");
        }
    }
}
