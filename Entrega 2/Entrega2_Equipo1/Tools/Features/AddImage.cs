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


		public Bitmap ReadColors(Image image, int width =10, int height = 10)
		{
			//Converting loaded image into bitmap
			Resizer resizer = new Resizer();
			Bitmap imageBit = image.BitmapImage;
			Bitmap bmp = (Bitmap)imageBit.Clone();
			bmp = resizer.ResizeImage(bmp, 1000, 1000);


			List<int[]> TotalRGB= new List<int[]>();
			for (int i = 0; i < bmp.Height; i++)
			{

				for (int j = 0; j < bmp.Width; j++)
				{
					Color now_color = bmp.GetPixel(j, i);

					int[] RGB = { (int)now_color.R, (int)now_color.G, (int)now_color.B };
					TotalRGB.Add(RGB);

				}
			}

			int wCont = 0;

			List<List<int[]>> RGBbyBlocks = new List<List<int[]>>();
			List<int[]> subBlock = new List<int[]>();
			for (int i=0; i<TotalRGB.Count; i++)
			{
				if ((wCont % width)==0 && wCont != 0)
				{
					List<int[]> cloneList = subBlock.ToList();
					RGBbyBlocks.Add(cloneList);
					subBlock.Clear();
					wCont = 0;
					
				}
				
					subBlock.Add(TotalRGB[i]);
					wCont++;
				
			}

			int hCont = 0;
			List<List<List<int[]>>> Cells = new List<List<List<int[]>>>();
			List<List<int[]>> temp = new List<List<int[]>>();
			foreach (List<int[]> subList in RGBbyBlocks)
			{
				if ((hCont % height) == 0 && hCont != 0)
				{
					List<List<int[]>> cloneList = temp.ToList();
					Cells.Add(cloneList);
					temp.Clear();
					hCont = 0;
				}
				temp.Add(subList);
				hCont++;

			}

			int R = 0;
			int G = 0;
			int B = 0;

			List<int[]> avgRGB = new List<int[]>();
			foreach (List<List<int[]>> Altura in Cells)
			{
				foreach(List<int[]> Ancho in Altura)
				{
					foreach (int[] rgb in Ancho)
					{ 
						R += rgb[0];
						G += rgb[1];
						B += rgb[2];

					}
				}
				R = R / (width * height);
				G = G / (width * height);
				B = B / (width * height);
				int[] rgbs = { R, G, B };
				avgRGB.Add(rgbs);
				R = 0;
				G = 0;
				B = 0;
			}

			foreach(int[] avgRGBS in avgRGB)
			{
				Console.WriteLine("{0},{1},{2}", avgRGBS[0], avgRGBS[1], avgRGBS[2]);
			}
			return null;
		}

    }
}
