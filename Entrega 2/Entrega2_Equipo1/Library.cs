using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    public class Library
    {
        private List<Image> imagens;
        private Dictionary <string, List<Image>> smartList;  //string es un patron


        
        public Library() //List<Image> images, Dictionary<string, List<Image>> smartList
        { 
            this.Imagens = new List<Image>();
            this.SmartList = SmartList;
        }

        public List<Image> Imagens { get => imagens; set => imagens = value; }
        public Dictionary<string, List<Image>> SmartList { get => smartList; set => smartList = value; }

        public void LoadLibrary()
        {
            string pathproperties = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\csvtest.txt";
            Console.WriteLine(pathproperties);
            StreamReader st = new StreamReader(pathproperties);

            while (!st.EndOfStream)
            {
                string line = st.ReadLine();
                List<Label> label = new List<Label>();
                string[] lin = line.Split(',');
                int values = Convert.ToInt32(lin[1])*2 + 1;
                //guardado simple label
                for (int i = 2; i <= values; i+=2)
                {
                    Console.WriteLine("parte1");
                    SimpleLabel tag = new SimpleLabel(lin[i+1],Convert.ToInt32(lin[i]));
                    label.Add(tag);
                    
                }

                //guardado personal label
                int numPersonalLabel = Convert.ToInt32(lin[values + 1]);
                int j;
                for (j = values + 2; j < (numPersonalLabel * 9) + numPersonalLabel+values-1; j += 9)
                {
                    Console.WriteLine("parte2");
                    //Aqui cada uno  de los aux representa un atributo de PersonLabel
                    int serial = Convert.ToInt32(lin[j]);
                    
                    string aux = lin[j+1];
                    string[] dou = lin[j + 2].Split('.'); //Aqui tengo que quitar el ( y ) del final
                    double[] aux1 = new double[4] { Convert.ToDouble(dou[0].TrimStart('(')), Convert.ToDouble(dou[1]), Convert.ToDouble(dou[2]), Convert.ToDouble(dou[3].TrimEnd(')')) };
                    string aux2 = lin[j + 3];
                    ENationality aux3 = (ENationality)Enum.Parse(typeof(ENationality), lin[j + 4]);
                    EColor aux4 = (EColor)Enum.Parse(typeof(EColor), lin[j + 5]);
                    EColor aux5 = (EColor)Enum.Parse(typeof(EColor), lin[j + 6]);
                    ESex aux6 = (ESex)Enum.Parse(typeof(ESex), lin[j + 7]);
                    string aux7 = lin[j + 8];
                    
                    /*
                    PersonLabel tag1 = new PersonLabel(aux, aux1, aux2, aux3, aux4, aux5, aux6,aux7,serial);    //GF tiene que poner el serialnumber de ultimo
                    label.Add(tag1);
                    */
                }
                 int numSpecialLabel = Convert.ToInt32(lin[j]);
                 for (int d = numPersonalLabel*9+values+3; d<lin.Length-1; d+=6)
                 {
                    Console.WriteLine("parte3");
                    string[] dou1 = lin[d].Split('.');
                    double[] p1 = new double[2] { Convert.ToDouble(dou1[0].TrimStart('{')),Convert.ToDouble(dou1[1].TrimEnd('}'))};
                    string p2 = lin[d + 1];
                    string p3 = lin[d + 2];
                    string p4 = lin[d + 3];
                    bool p5 = Convert.ToBoolean(lin[d + 4]);
                    int p6 = Convert.ToInt32(lin[d + 5]);
                    SpecialLabel tag2 = new SpecialLabel(p1, p2, p3, p4, p5, p6);
                    label.Add(tag2);
                 }
   
                  int nlist = lin.Length;
                  //AQUI TENGO UN PROBLEMA CON EL CODIGO DE IMAGEN
                  Image ima = new Image(lin[0], label, Convert.ToInt32(lin[nlist-1]));
                  imagens.Add(ima);
                  
                            
            }
            string pathpatterns = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\patterns.txt";
            StreamReader patterns = new StreamReader(pathpatterns);

            while (!patterns.EndOfStream)
            {
                string line1 = patterns.ReadLine();
                string[] lin1 = line1.Split(';');
                for (int size = 0; size < lin1.Length; size++)
                {

                }


            }

            //return true;




        }

        public void UpdateLibrary()
        {
            throw new Exception("falta");
        }

        public void AddLabel(string nameImage, Label label)
        {
            throw new Exception("no esta listo");
        }

        public void RemoveLabel(string nameImage, int serialNumber)
        {
               
        }

        public void ChangeRating(string nameImage, int rating)
        {
            string pathproperties = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\csvtest.txt";
            string patho = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\repositorio.txt";
            StreamReader st = new StreamReader(pathproperties);
            StreamWriter p = new StreamWriter(patho);
            while (!st.EndOfStream)
            {
                string line = st.ReadLine();
                if (line != "#")
                {
                    string[] lin = line.Split(',');
                    if (lin[0] == nameImage)
                    {
                        for (int i = 0; i <= lin.Length - 1; i++)
                        {
                            if (lin.Length - 1 == i)
                            {
                                p.Write(Convert.ToString(rating) + "\n");
                            }
                            else
                            {
                                p.Write(lin[i] + ",");
                            }
                        }
                    }
                    else
                    {
                        p.WriteLine(line);
                    }
                }
            }

            st.Close();
            p.Close();
            File.Delete(pathproperties);
            File.Copy(patho, pathproperties);
            File.Delete(patho);
        }

        public void AddSearchPatterm(string patron)
        {
            throw new Exception("blum");
        }

        public void UpdateSmartList()
        {
            throw new Exception("ja");
        }
    }
}

