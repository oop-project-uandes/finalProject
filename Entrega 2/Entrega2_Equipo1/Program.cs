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
            filter.pruebaClasificar("C:\\Users\\Gianfranco Lacasella\\Desktop\\Prueba c# watson\\descarga.jpg");
            Console.ReadKey();

        }
    }
}
