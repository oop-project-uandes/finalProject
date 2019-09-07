using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    public class Library
    {
        private List<Image> imagen;

        /*
        private Dictionary <string patron, List<Image> image> smartList;
        */


        public Library()
        {
            this.imagen = new List<Image>();

        }

        private bool LoadLibrary()
        {
            string pathproperties = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\properties.txt";
            string pathfiles = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files";
            StreamReader st = new StreamReader(pathproperties);

            while (!st.EndOfStream)
            {
                string line = st.ReadLine();
                if (line != "#")
                {
                    string[] lin = line.Split(',');
                    List<Label> label = new List<Label>();
                    for (int i = 1; i < lin.Count(); i++)
                    {
                        if (lin[i].GetType() == int)
                    }

                }
                else
                {

                }



            }

            st.Close();
        }



    }
}

