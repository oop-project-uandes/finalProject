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
            List<string> name;
            name = new List<string>() { "stock1.jpg", "stock2.jpg" };
            WorkingArea workingArea = new WorkingArea();
            workingArea.LoadImage(name);
        }
    }
}
