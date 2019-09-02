using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Entrega2_Equipo1
{
    public class Scissors
    {
        // Scissors's builder
        public Scissors() { }

        /*Method to cut the pictures passed as parameters with the double[4]
        specifications. The values of the dictionary, double[4], are the coordinates
        of where to cut the given picture (the key).*/
        public List<Bitmap> Cut(Dictionary<Bitmap, double[]> images)
        {
            int counter = 0;
            foreach (KeyValuePair<Bitmap, double[]> pair in images)
            {
                if (pair.Value.Length != 4)
                {
                    throw new Exception($"The {counter}th entry of the dictionary have an invalid value. Didn't cut anything");
                }
                counter++;
            }

            List<Bitmap> returnValue = new List<Bitmap>();
        }
    }
}
