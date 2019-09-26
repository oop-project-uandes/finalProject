using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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


		public Bitmap ReadColors(Image image, List<Image> images, int width =8, int height = 3)
		{
			//Converting loaded image into bitmap
			Resizer resizer = new Resizer();
			Bitmap imageBit = image.BitmapImage;
			Bitmap bmp = (Bitmap)imageBit.Clone();
            Scissors scissors = new Scissors();
			bmp = resizer.ResizeImage(bmp, width*100, height*100);

            List<Bitmap> list = new List<Bitmap>();
            for (int i = 0; i < (bmp.Width/width); i++)
            {
                for (int y = 0; y < (bmp.Height/height); y++)
                {
                    Graphics gr = Graphics.FromImage(bmp);
                    Rectangle r = new Rectangle(i * (bmp.Width / (bmp.Width/width)),
                                                y * (bmp.Height / (bmp.Height/height)),
                                                bmp.Width / (bmp.Width/width),
                                                bmp.Height / (bmp.Height/height));
                    double[] coord = { r.X, r.Y, r.Width, r.Height };
                    list.Add(scissors.Crop(bmp, coord)); 

                }
            }
            List<int[]> rgbAVG = avgRGB(list);
            
            Console.WriteLine(rgbAVG.Count);

            List<Bitmap> imagesList = new List<Bitmap>();
            if (imagesList.Count !=0)
            {
                foreach (Image imageInsert in images)
                {
                    imagesList.Add(imageInsert.BitmapImage);
                }
            }



            
            return null;
		}

        private List<int[]> avgRGB  (List<Bitmap> list)
        {
            List<int[]> rgbAVG = new List<int[]>();
            foreach (Bitmap bitmap in list)
            {
                int R = 0;
                int G = 0;
                int B = 0;
                int cont = 0;
                for (int i = 0; i < bitmap.Height; i++)
                {

                    for (int j = 0; j < bitmap.Width; j++)
                    {
                        Color now_color = bitmap.GetPixel(j, i);
                        R += (int)now_color.R;
                        G += (int)now_color.G;
                        B += (int)now_color.B;
                        cont++;
                    }
                }
                int[] RGB = { R / cont, G / cont, B / cont };
                rgbAVG.Add(RGB);
            }
            return rgbAVG;
        }

        private int[] avgRGB(Bitmap bitmap)
        {

                int R = 0;
                int G = 0;
                int B = 0;
                int cont = 0;
                for (int i = 0; i < bitmap.Height; i++)
                {

                    for (int j = 0; j < bitmap.Width; j++)
                    {
                        Color now_color = bitmap.GetPixel(j, i);
                        R += (int)now_color.R;
                        G += (int)now_color.G;
                        B += (int)now_color.B;
                        cont++;
                    }
                }
                int[] RGB = { R / cont, G / cont, B / cont };

            
            return RGB;
        }

    }
}

