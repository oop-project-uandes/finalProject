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
        specifications. The values of the dictionary, double[4] {left, top, width, height), are the coordinates
        of where to cut the given picture (the key).*/
        public List<Bitmap> Crop(Dictionary<Bitmap, double[]> images)
        {

            this.Verification(images);
            List<Bitmap> returnValue = new List<Bitmap>();
            int x, y, width, height;
            // For each pair in the Dictionary, we create a croppedImage, and add them to the return list
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


        // Same as before, but crops only one image
        public Bitmap Crop(Bitmap image, double[] coordinates)
        {

            this.Verification(image, coordinates);
            int x, y, width, height;
            x = Convert.ToInt32(coordinates[0]);
            y = Convert.ToInt32(coordinates[1]);
            width = Convert.ToInt32(coordinates[2]);
            height = Convert.ToInt32(coordinates[3]);
            Bitmap returnValue = image.Clone(new Rectangle(x, y, width, height), image.PixelFormat);
            // We return the return list
            return returnValue;
        }


        /* Method used by Crop to know if there are some problems with the data received by param
         * Throws exceptions saying what's wrong*/
        private void Verification(Dictionary<Bitmap, double[]> images)
        {

            int counter = 0;
            foreach (KeyValuePair<Bitmap, double[]> pair in images)
            {
                int imageWidth = pair.Key.Width;
                int imageHeight = pair.Key.Height;
                // First, we verify that the length of the array is 4
                if (pair.Value.Length != 4)
                {
                    throw new Exception($"The value of the {counter}th entry of the dictionary doesn't have 4 elements. Didn't cut anything");
                }
                if ((pair.Value[1] + pair.Value[3] > imageHeight || pair.Value[0] + pair.Value[2] > imageWidth) || pair.Value[0] < 0 || pair.Value[1] < 0 || pair.Value[2] < 0 || pair.Value[3] < 0)
                {
                    throw new Exception($"The {counter}th entry of the dictionary have incorrect crop bounds. Didn't cut anything");
                }
                counter++;
            }
        }

        // Same as before, but with only one Bitmap
        private void Verification(Bitmap image, double[] coordinates)
        {
            int counter = 0;
            int imageWidth = image.Width;
            int imageHeight = image.Height;
            // First, we verify that the length of the array is 4
            if (coordinates.Length != 4)
            {
                throw new Exception($"The value of the {counter}th entry of the dictionary doesn't have 4 elements. Didn't cut anything");
            }
            if ((coordinates[1] + coordinates[3] > imageHeight || coordinates[0] + coordinates[2] > imageWidth) || coordinates[0] < 0 || coordinates[1] < 0 || coordinates[2] < 0 || coordinates[3] < 0)
            {
                throw new Exception($"The {counter}th entry of the dictionary have incorrect crop bounds. Didn't cut anything");
            }
            counter++;
        }
    }
}
