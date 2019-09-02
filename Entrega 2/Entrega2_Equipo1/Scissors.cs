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
        public List<Bitmap> Crop(Dictionary<Bitmap, double[]> images)
        {
            // Verify that all the Dictionary values are ok
            int counter = 0;
            foreach (double[] value in images.Values)
            {
                if (value.Length != 4)
                {
                    throw new Exception($"The {counter}th entry of the dictionary have an invalid value. Didn't cut anything");
                }
                counter++;
            }

            // Creating the return list
            List<Bitmap> returnValue = new List<Bitmap>();
            int x, y, width, height;
            
            // For each pair in the Dictionary, we create a croppedImage, and add them to the return list
            // TODO: No se si funciona este metodo
            foreach (KeyValuePair<Bitmap, double[]> pair in images)
            {
                x = Convert.ToInt32(pair.Value[0]);
                y = Convert.ToInt32(pair.Value[1]);
                width = Convert.ToInt32(pair.Value[2]);
                height = Convert.ToInt32(pair.Value[3]);
                Bitmap croppedImage = pair.Key.Clone(new Rectangle(x, y, width, height), pair.Key.PixelFormat);
                returnValue.Add(croppedImage);
            }
            // We return the return list
            return returnValue;
        }
    }
}
