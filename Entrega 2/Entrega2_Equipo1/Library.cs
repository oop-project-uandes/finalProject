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
        private List<Image> imagens;
        private Dictionary<string, List<Image>> smartList;


        public Library(List<Image> images, Dictionary<string, List<Image>> smarlist )
        {
            this.Imagens = images;        // images es una lista que entrega la descerealizacion
            this.SmartList = smarlist;    //Diccionario con los patrones e imagenes que corresponde de la descerealizacion
        }

        public List<Image> Imagens { get => imagens; set => imagens = value; }
        public Dictionary<string, List<Image>> SmartList { get => smartList; set => smartList = value; }

        public void AddImage(Image image)
        {
            imagens.Add(image);
        }
        public void RemoveImage(string nameImage)
        {
            foreach (Image imag in imagens)
            {
                if (imag.Name == nameImage)
                {
                    imagens.Remove(imag);
                }
            }
        }

        public void AddLabel(string nameImage, Label label)
        {
            foreach (Image imag in imagens)
            {
                if (imag.Name == nameImage)
                {
                    imag.Labels.Add(label);
                }
            }

        }

        public void RemoveLabel(string nameImage, int serialNumber)
        {
            foreach (Image imag in imagens)
            {
                if (imag.Name == nameImage)
                {
                    foreach(Label label in imag.Labels)
                    {
                        if (label.SerialNumber == serialNumber)
                        {
                            imag.Labels.Remove(label);
                        }
                    }
                }
            }
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
            foreach(Image image in imagens)
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
