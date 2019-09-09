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
            Library h = new Library();
            h.LoadLibrary();
            h.RemoveLabel("stock2.jpg",2);

        }
    }
}
