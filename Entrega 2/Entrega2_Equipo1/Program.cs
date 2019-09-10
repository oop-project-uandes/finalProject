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
            Library library = new Library();
            library.LoadLibrary();
            SimpleLabel simple = new SimpleLabel("nene");
            library.AddLabel("stock1.jpg",simple);
        }
    }
}
