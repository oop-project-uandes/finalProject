using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Entrega2_Equipo1
{
    [Serializable]
    public class AddCensorship : Tool
    {
        private Brush brush;

        public AddCensorship()
        {
            this.brush = new Brush();
        }

        // Coordinates must be given as (X,Y,TOP,LEFT)
        public Bitmap blackCensorship(Bitmap image, int[] coordinates)
        {
            Color black = Color.FromArgb(0, 0, 0);
            Bitmap returningImage = brush.paintRectangle(image, black, coordinates);
            return returningImage;
        }

        public Bitmap pixelCensorship(Image image, int[] coordinates)
        {
            Bitmap bitmap = image.BitmapImage;
            double[] coordinatesDouble = new double[coordinates.Length];
            for (int i=0; i < coordinates.Length; i++)
            {
                coordinatesDouble[i] = Convert.ToDouble(coordinates[i]);
            }
            Scissors scissors = new Scissors();
            Resizer resizer = new Resizer();
            AddImage AI = new AddImage();
            Bitmap cropped = scissors.Crop(bitmap, coordinatesDouble);
            Bitmap ResizedSmall = resizer.ResizeImage(cropped, 10, 10);
            Bitmap ResizedNormal = resizer.ResizeImage(ResizedSmall, cropped.Width, cropped.Height);
            Bitmap Final = AI.InsertImage(bitmap, ResizedNormal, coordinates[0], coordinates[1], coordinates[2], coordinates[3]);
            return Final;
        }
    }
}
