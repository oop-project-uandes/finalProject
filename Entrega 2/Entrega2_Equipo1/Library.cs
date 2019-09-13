using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    [Serializable]
    public class Library
    {
        private List<Image> images;
        private Dictionary<string, List<Image>> smartList;

        
        public Library(List<Image> images, Dictionary<string, List<Image>> smartlist )
        {
            this.images = images;
            this.smartList = smartlist;    
        }

        public Library() : this(new List<Image>(), new Dictionary<string, List<Image>>()) { }

        public List<Image> Images { get => images; set => images = value; }
        public Dictionary<string, List<Image>> SmartList { get => smartList; set => smartList = value; }

        public bool AddImage(Image image)
        {
            images.Add(image);
            return true;
        }
        public bool RemoveImage(string nameImage)
        {
            foreach (Image imag in images)
            {
                if (imag.Name == nameImage)
                {
                    images.Remove(imag);
                    return true;
                }
            }
            return false;
        }

        public bool AddLabel(string nameImage, Label label)
        {
            foreach (Image imag in images)
            {
                if (imag.Name == nameImage)
                {
                    imag.Labels.Add(label);
                    return true;
                }
            }
            return false;
        }

        public bool RemoveLabel(string nameImage, int serialNumber)
        {
            foreach (Image imag in images)
            {
                if (imag.Name == nameImage)
                {
                    foreach(Label label in imag.Labels)
                    {
                        if (label.SerialNumber == serialNumber)
                        {
                            imag.Labels.Remove(label);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool AddSmartList(string patron, List<Image> images)

        {
            try
            {
                Searcher searcher = new Searcher();
                List<Image> imagePatron = searcher.Search(images , patron);
                smartList.Add(patron, imagePatron);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool ChageRating (string nameImage, int rating)

        {
            foreach(Image image in images)
            {
                if (image.Name == nameImage)
                {
                    image.Calification = rating;
                    return true;
                }
            }
            return false;

        }

    }
}
