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
            MirrorFilter filter = new MirrorFilter();
            Bitmap SP = filter.ApplyFilter(imagen1);
            SP.Save("C:\\Users\\Gianfranco Lacasella\\Desktop\\Prueba c# watson\\mirror.jpg");
        }
    }
}
