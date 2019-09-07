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
            int cont = 0;
            Searcher search = new Searcher();
            List<List<List<string>>> x = search.Declaration("Hair: Marron and Nationality: Chile or Name: Matias and Sex: Female and Name: Max or EyeColor: Blue and Sex: Male and Selfie: True and EyeColor: Green");
            foreach (List<List<string>> subDec in x)
            {
                Console.WriteLine("(Program) Total {0}, La sublista contiene {1} items", cont, subDec.Count);
                foreach (List<string> atributes in subDec)
                {
                    Console.WriteLine("Type: {0}, Value: {1}", atributes[0], atributes[1]);
                }
                cont++;
            }
        }
    }
}
