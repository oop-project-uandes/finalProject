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
            //Bitmap baseIm = (Bitmap)baseImage.Clone();
            //Bitmap insertIm = (Bitmap)insertImage.Clone();
            Graphics gr = Graphics.FromImage(baseImage);

            Rectangle rect = new Rectangle(xAxis, yAxis,
         widthImage, heightImage);
            gr.DrawImage(insertImage, rect);

            return baseImage;
        }

        public Bitmap ImageCollage(List<Image> images, int widthImage = 1000, int heightImage = 1000,
            int widthInsertImage = 100, int heightInsertImage = 100, Bitmap backgroundImage = null, int R = 0, int G= 0, int B = 0)
        {
            Random rnd = new Random();
            Resizer resizer = new Resizer();
            Bitmap Collage = new Bitmap(widthImage, heightImage);
            if (backgroundImage != null)
            {
                backgroundImage = resizer.ResizeImage(backgroundImage, widthImage, heightImage);
                Collage = backgroundImage;
            }
            else
            {
                for (int x = 0; x < Collage.Width; x++)
                {
                    for (int y = 0; y < Collage.Height; y++)
                    {
                        Collage.SetPixel(x,y, Color.FromArgb(R, G, B));
                    }
                }
            }
            List<int[]> positions = new List<int[]>();
            int position = 0;
            for (int i = 0; i < images.Count; i++)
            {
                int[] coordRandom = { rnd.Next(0, widthImage - widthInsertImage), rnd.Next(0, heightImage - heightInsertImage) };
                positions.Add(coordRandom);
            }
            foreach (Image image in images) {
                Collage = InsertImage(Collage, resizer.ResizeImage(image.BitmapImage, widthInsertImage, heightInsertImage),
                    positions[position][0], positions[position][1], widthInsertImage, heightInsertImage);
                position++;
            }
            return Collage;
        }


        public Bitmap Mosaic(Image image, List<Image> images, int width = 10, int height = 10)
        {
            Console.WriteLine("Loading images, please wait");
            //Converting loaded image into bitmap
            Resizer resizer = new Resizer();
            Bitmap imageBit = image.BitmapImage;
            Bitmap bmp = (Bitmap)imageBit.Clone();
            Scissors scissors = new Scissors();
            bmp = resizer.ResizeImage(bmp, 80 * width, 80 * height);

            int bmpWidth = bmp.Width / (bmp.Width / width);
            int bmpHeight = bmp.Height / (bmp.Height / height);
            Console.WriteLine("Dividing Image");
            List<int[]> coords = new List<int[]>();
            Dictionary<Bitmap, int[]> dict = new Dictionary<Bitmap, int[]>();
            for (int i = 0; i < (bmp.Width / width); i++)
            {
                for (int y = 0; y < (bmp.Height / height); y++)
                {
                    Graphics gr = Graphics.FromImage(bmp);
                    Rectangle r = new Rectangle(i * bmpWidth,
                                                y * bmpHeight,
                                                bmpWidth,
                                                bmpHeight);
                    double[] coord = { r.X, r.Y, r.Width, r.Height };
                    int[] bitCoord = { (i * bmpWidth),
                            (y * bmpHeight),
                            (bmpWidth),
                            (bmpHeight)};
                    dict.Add(scissors.Crop(bmp, coord), bitCoord);
                    coords.Add(bitCoord);

                }
            }
            List<Bitmap> list = new List<Bitmap>();
            foreach (KeyValuePair<Bitmap, int[]> keys in dict)
            {
                list.Add(keys.Key);
            }

            Console.WriteLine("Getting Avg RGBS");
            List<int[]> rgbAVG = avgRGB(list);


            List<Bitmap> imagesList = new List<Bitmap>();

            foreach (Image imageInsert in images)
            {
                imagesList.Add(imageInsert.BitmapImage);
            }
            Bitmap baseImage = new Bitmap(bmp.Width, bmp.Height);
            int AvgCont = 0;
            int max = list.Count;
            ColorFilter CF = new ColorFilter();
            while (true)
            {
                double porcentage = ((double)AvgCont / (double)max);
                BarraCarga("Creando Mosaico", porcentage);
                if (AvgCont < max)
                {
                    Color color = Color.FromArgb(rgbAVG[AvgCont][0], rgbAVG[AvgCont][1], rgbAVG[AvgCont][2]);
                    baseImage = InsertImage(baseImage, CF.ApplyFilter(Random(images), color)
                    , coords[AvgCont][0], coords[AvgCont][1], coords[AvgCont][2], coords[AvgCont][3]);
                }
                else
                {
                    return baseImage;
                }
                AvgCont++;
                GC.Collect();
            }

        }

        private List<int[]> avgRGB(List<Bitmap> list)
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


        private Bitmap Random(List<Image> images)
        {
            Random rnd = new Random();
            return images[rnd.Next(0, images.Count)].BitmapImage;
        }


        private void BarraCarga(string title, double porcentaje)
        {
            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth - title.Length) / 2, Console.CursorTop);
            Console.WriteLine(title);
            int cantidadLlaves = Convert.ToInt32(porcentaje * 20);
            int cantidadEspacios = 20 - cantidadLlaves;
            double porcentajefinal = porcentaje * 100;
            Console.SetCursorPosition((Console.WindowWidth - 30) / 2, Console.CursorTop);
            Console.Write("[");
            Console.BackgroundColor = ConsoleColor.White;
            for (int i = 0; i < cantidadLlaves; i++)
            {
                Console.Write("#");
            }

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            for (int b = 0; b < cantidadEspacios; b++)
            {
                Console.Write(" ");
            }
            Console.Write($"] {porcentajefinal}%");
            if ((int)porcentajefinal == 30)
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("\n");
                }
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Please be patient, I've autism");
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
        }
    }
}



