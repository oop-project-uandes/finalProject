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
        private Dictionary<System.Drawing.Bitmap, string> images;
        private string sourcePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName+ @"\Files";
        private string targetPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName+ @"\Temp";
        //Builder
        public WorkingArea() {

        }
        //Get set
        public Dictionary<Bitmap, string> Images { get => images; set => images = value; }
        //Methods





        public bool LoadImage(List<string> imageNames) {
            for (int i = 0; i < imageNames.Count; i++)
            {
                string sourceName = sourcePath + @"\"+imageNames[i];
                string targetName = targetPath + @"\" + imageNames[i];
                System.IO.File.Copy(sourceName, targetName,true);
                //Bitmap BitmapImage = new Bitmap(targetName);
                //images.Add(BitmapImage,imageNames[i]);
            }
            for (int i = 0; i < imageNames.Count; i++)
            {   string targetName = targetPath + @"\" + imageNames[i];
                Bitmap BitmapImage = new Bitmap(targetName);

            }
            return true;
        }








        public bool SaveImage() {
            throw new Exception("exeptionsaveimage"); 
        }
        public bool AddImage()
        {
            throw new Exception("exeption addimage");
        }
        public bool BacktoLibrary()
        {
            throw new Exception("exeptionsbacktolibrary");
        }
        public bool RemoveFromWorkingArea()
        {
            throw new Exception("exeptionremovefrom");
        }

    }
}
