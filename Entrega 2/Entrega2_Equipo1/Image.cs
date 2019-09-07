using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Entrega2_Equipo1
{
    public class Image
    {
        private List<Label> labels;
        private string name;
        private Bitmap image;
        private int calification;
        private int[] resolution;
        private int[] aspectRatio;
        private double saturation;
        private bool darkClear;
        private Dictionary<string, string> exif;
        


        public Image(string name, List<Label> labels, int calification)
        {
            this.name = name;
            this.labels = labels;
            this.calification = calification;
            this.image = LoadImage(name);
            this.resolution = LoadResolution();
            this.aspectRatio = LoadAspectRatio();
            this.saturation = LoadSaturation();
            this.darkClear = LoadDarkClear();
        }

        private Bitmap LoadImage(string name)
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\" + name;
            Bitmap returningImage = new Bitmap(path);
            return returningImage;
        }

        private int[] LoadResolution()
        {
            int v1 = this.image.Width;
            int v2 = this.image.Height;
            int[] returningArray = new int[] { v1, v2 };
            return returningArray;
        }

        private int MCD(int a, int b)
        {
            while (b > 0)
            {
                int rem = a % b;
                a = b;
                b = rem;
            }
            return a;
        }

        private void Simplify(int[] numbers)
        {
            int mcd = this.MCD(numbers[0],numbers[1]);
            for (int i = 0; i < numbers.Length; i++) numbers[i] /= mcd;
            return;
        }

        private int[] LoadAspectRatio()
        {
            int[] returningAspect = new int[] { this.image.Width, this.image.Height };
            Simplify(returningAspect);
            return returningAspect;
        }


        private double LoadSaturation()
        {
            double[] aux = new double[image.Width*image.Width];
            Color color;
            int counter = 0;
            for (int i = 0; i < this.image.Height; i++)
            {
                for (int x = 0; x < this.image.Width; x++)
                {
                    color = this.image.GetPixel(x, i);
                    double v1 = Math.Max(Math.Max(color.R, color.G), color.B);
                    double v2 = Math.Min(Math.Min(color.R, color.G), color.B);
                    aux[counter] = Math.Acos((v1 + v2) / v1);
                    counter++;
                }
            }
            return aux.Average();
        }

        private bool LoadDarkClear()
        {
            Color color;
            double brightness, totalBrightness = 0;
            for (int i = 0; i < this.image.Height; i++)
            {
                for (int x = 0; x < this.image.Width; x++)
                {
                    color = this.image.GetPixel(x, i);
                    brightness = (color.R * 299 + color.G * 587 + color.B * 114) / 1000;
                    totalBrightness += brightness;
                }
            }

            double result = totalBrightness / (this.image.Width * this.image.Height);
            if (result < 0.5)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Dictionary<string, string> LoadExif()
        {
            throw new NotImplementedException();
            
        }

    }
}
