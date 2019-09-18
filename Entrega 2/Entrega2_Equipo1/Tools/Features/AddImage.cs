using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    // TODO: IMPLEMENTAR
    [Serializable]
    public class AddImage : Tool
    {
        public Bitmap InsertImage(Bitmap baseImage, Bitmap insertImage, int xAxis, int yAxis, int widthImage = 100, int heightImage = 100)
        {
            Bitmap baseIm = (Bitmap)baseImage.Clone();
            Bitmap insertIm = (Bitmap)insertImage.Clone();
            Graphics gr = Graphics.FromImage(baseIm);
            
                Rectangle rect = new Rectangle(xAxis, yAxis,
             widthImage, heightImage);
                gr.DrawImage(insertIm, rect);

            return baseIm;
        }

        public Bitmap ImageCollage(List<Image> images, List<int[]> positions, int widthImage = 1000, int heightImage = 1000,
            int widthInsertImage = 100, int heightInsertImage = 100)
        {
            Bitmap baseImage = new Bitmap(widthImage, heightImage);
            Graphics gr = Graphics.FromImage(baseImage);

            for (int i= 0; i<images.Count;i++)
            {
                Rectangle rect = new Rectangle(positions[i][0], positions[i][1],
             widthImage, heightImage);
                gr.DrawImage(images[i].BitmapImage, rect);
            }

            return baseImage;
        }


    }
}
