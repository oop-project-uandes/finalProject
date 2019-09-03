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
            WatsonFilter filter = new WatsonFilter();
            string path = "C:\\Users\\Gianfranco Lacasella\\Desktop\\Prueba c# watson\\descarga.jpg";
            
            filter.ClassifyFaces(path);
            filter.Classify(path);
            Console.ReadKey();
        }
    }
}
