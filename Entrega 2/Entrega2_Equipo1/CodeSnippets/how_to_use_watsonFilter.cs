using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace random_namespace
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creating a filter
            WatsonFilter myFilter = new WatsonFilter();
            // Open the bitmap
            Bitmap fileToAnalize = new Bitmap("path to file. Can be bmp or jpg");

            // Analyze
            Dictionary<int, Dictionary<string, object>> resultadoRostros = myFilter.FindFaces(fileToAnalize);
            Dictionary<int, Dictionary<string, double>> resultadoClasificacion = myFilter.FindClassifiers(fileToAnalize);

            // pair.Key are the classes detected by watson and pair.Value are their score
            foreach (Dictionary<string, double> dic in resultadoClasificacion.Values)
            {
                foreach (KeyValuePair<string, double> pair in dic)
                {
                    Console.WriteLine($"{pair.Key}: {pair.Value}");
                }
            }

            // same as before, but accessing through objects 
            foreach (Dictionary<string, object> dic in resultadoRostros.Values)
            {
                Age edad = (Age)dic["age"];
                Gender genero = (Gender)dic["gender"];
                FacePosition posicion = (FacePosition)dic["position"];
                Console.WriteLine($"{edad.MinAge}, {edad.MaxAge}, {edad.ScoreAge}; {genero.GenderType}, {genero.GenderScore}; {posicion.Top}, {posicion.Left}, {posicion.Width}, {posicion.Height}");
            }

            Console.ReadKey();

        }
    }
}

