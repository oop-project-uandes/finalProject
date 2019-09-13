using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Entrega2_Equipo1
{
    class Program
    {
        static void Main(string[] args)
        {
            Image testImage1 = new Image("stock1.jpg", new List<Label>(), 3);
            Image testImage2 = new Image("stock2.jpg", new List<Label>(), 4);
            Image testImage3 = new Image("stock3.jpg", new List<Label>(), 3);
            List<Image> imageList = new List<Image>(){ testImage1,testImage2,testImage3};
            string Declaration = "Calificaction: 3";
            Searcher search = new Searcher();
            List < Image >  resultado = search.Search(imageList, Declaration);
            foreach(Image image in resultado)
            {
                Console.WriteLine(image.Name);
            }

        }
    }
}
