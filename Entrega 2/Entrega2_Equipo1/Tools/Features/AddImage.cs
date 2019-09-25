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

        public Bitmap ImageCollage(List<Image> images, int widthImage = 1000, int heightImage = 1000,
            int widthInsertImage = 100, int heightInsertImage = 100, Bitmap backgroundImage= null)
        {
            Random rnd = new Random();
            Resizer resizer = new Resizer();
            Bitmap Collage = new Bitmap(widthImage, heightImage);
            if (backgroundImage != null)
            {
                backgroundImage = resizer.ResizeImage(backgroundImage, widthImage, heightImage);
                Collage = backgroundImage;
            }
            List<int[]> positions = new List<int[]>();
            int position = 0;
            for (int i = 0; i < images.Count; i++)
            {
                int[] coordRandom = { rnd.Next(0, widthImage-widthInsertImage), rnd.Next(0, heightImage-heightInsertImage) };
                positions.Add(coordRandom);
            }
            foreach (Image image in images) {
                Collage = InsertImage(Collage, resizer.ResizeImage(image.BitmapImage, widthInsertImage, heightInsertImage),
                    positions[position][0], positions[position][1],widthInsertImage,heightInsertImage);
                position++;
                    }
            return Collage;
        }


    }
}
