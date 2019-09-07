using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    public class WorkingArea
    {
        //Atributes
        private Dictionary<string, Bitmap> images;
        private string sourcePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files";
        private string targetPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Temp";

        //Builder
        public WorkingArea()
        {
            Images = new Dictionary<string, System.Drawing.Bitmap> { };
        }

        //Get set
        public Dictionary<string, Bitmap> Images { get => images; set => images = value; }
        
        //Methods
        public bool LoadImage(List<string> imageNames)
        {
            for (int i = 0; i < imageNames.Count; i++)
            {
                string sourceName = sourcePath + @"\"+imageNames[i];
                string targetName = targetPath + @"\" + imageNames[i];
                System.IO.File.Copy(sourceName, targetName,true);
                Bitmap BitmapImage = new Bitmap(targetName);
                images.Add(imageNames[i],BitmapImage);
            }
            Console.WriteLine(Images.Keys);
            return true;
        }

        public bool SaveImage(List<string> imageName)
        {
            foreach (string item in imageName)
            {
                Images[item].Save(targetPath + @"\" + imageName);
            }
            return true;
        }

        public bool AddImage(string imageName, Bitmap image)
        {
            images.Add(imageName, image);
            return true;
        }

        public bool BacktoLibrary(List<string> imageNamesToSendBack)
        {
            foreach (string item in imageNamesToSendBack)
            {
                string sourceName = sourcePath + @"\" + item;
                string targetName = targetPath + @"\" + item;
                File.Move(targetName, sourceName);
                Images.Remove(item);
            }
            return true;
        }

        public bool RemoveFromListOfImages(List<string> ListOfImagesToRemove)
        {
            for (int i = 0; i < ListOfImagesToRemove.Count; i++)
            {
                Images.Remove(ListOfImagesToRemove[i]);
            }
            return true;
        }
        //El metodo delete como que no funca
        public void NukeTemp() {
            Images.Clear();
            string[] files = Directory.GetFiles(targetPath);
            foreach (string file in files)
            {
                File.Delete(file);
                Console.WriteLine($"{file} is deleted.");
            }
            
        }
    }
}
