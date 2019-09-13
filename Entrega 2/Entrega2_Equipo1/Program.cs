using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Entrega2_Equipo1
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            string pathproperties = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files";
            List<Label> labels = new List<Label>();
            Library library = new Library();
            Image image = new Image("stock2.jpg", labels, 4);
            Image image1 = new Image("stock1.jpg", labels, 2);
            library.Agregarimage(image);
            library.Agregarimage(image1);
            Save();*/
            Library library = new Library();
            Descere();
            /*
            void Save()
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream("MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, library.Imagens);
                stream.Close();
            }
            */
            void Descere()
            {
                IFormatter formatter = new BinaryFormatter();
                Stream strea = new FileStream("MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
                List<Image> miimage = (List<Image>)formatter.Deserialize(strea);
                strea.Close();
                library.Imagens = miimage;
                Console.WriteLine(library.CambiarCalificacion(miimage[0].Name,7));
                Console.WriteLine(library.Imagens[0].Calification);

            }
        }
    }
}
