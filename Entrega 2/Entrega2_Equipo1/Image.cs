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

        public List<Label> Labels { get => this.labels; set => this.labels = value; }
        public string Name { get => this.name; set => this.name = value; }
        public Bitmap Image { get => this.image; set => this.image = value; }
        public int Calification { get => this.calification; set => this.calification = value; }
        public int[] Resolution { get => this.resolution; set => this.resolution = value; }
        public int[] AspectRatio { get => this.aspectRatio; set => this.aspectRatio = value; }
        public double Saturation { get => this.saturation; set => this.saturation = value; }
        public bool DarkClear { get => this.darkClear; set => this.darkClear = value; }
        public Dictionary<string, string> Exif { get => this.exif; set => this.exif = value; }

        public Image(string name, List<Label> labels, int calification)
        {
            this.Name = name;
            this.Labels = labels;
            this.Calification = calification;
            this.Image = LoadImage(name);
            this.Resolution = LoadResolution();
            this.AspectRatio = LoadAspectRatio();
            this.Saturation = LoadSaturation();
            this.DarkClear = LoadDarkClear();
        }

        public Image()
        { }

        private Bitmap LoadImage(string name)
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\" + name;
            Bitmap returningImage = new Bitmap(path);
            return returningImage;
        }

        private int[] LoadResolution()
        {
            int v1 = this.Image.Width;
            int v2 = this.Image.Height;
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
            int[] returningAspect = new int[] { this.Image.Width, this.Image.Height };
            Simplify(returningAspect);
            return returningAspect;
        }


        private double LoadSaturation()
        {
            double[] aux = new double[Image.Width*Image.Width];
            Color color;
            int counter = 0;
            for (int i = 0; i < this.Image.Height; i++)
            {
                for (int x = 0; x < this.Image.Width; x++)
                {
                    color = this.Image.GetPixel(x, i);
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
            for (int i = 0; i < this.Image.Height; i++)
            {
                for (int x = 0; x < this.Image.Width; x++)
                {
                    color = this.Image.GetPixel(x, i);
                    brightness = (color.R * 299 + color.G * 587 + color.B * 114) / 1000;
                    totalBrightness += brightness;
                }
            }

            double result = totalBrightness / (this.Image.Width * this.Image.Height);
            if (result < 0.5)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void LoadExif(Bitmap image)
        {
            PropertyItem[] items = image.PropertyItems;
            string id;
            string type;
            string len;
            string value;
            foreach (PropertyItem item in items)
            {
                ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                id = item.Id.ToString();
                type = item.Id.ToString();
                len = item.Len.ToString();
                value = encoding.GetString(item.Value);
                Console.WriteLine($"0x{id}, {type}, {len}, {value}");

            }
        }

    }
}
