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
            Image myImage = new Image();
            Bitmap image = new Bitmap("C:\\Users\\Gianfranco Lacasella\\Desktop\\Prueba c# watson\\stock2.jpg");
            myImage.LoadExif(image);
            Console.ReadKey();
        }
    }
}
