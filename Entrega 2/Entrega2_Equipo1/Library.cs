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
        private Dictionary <string, List<Image>> smartList;  


        public Library() 
        { 
            this.Imagens = new List<Image>();
            this.SmartList = new Dictionary<string, List<Image>>();
        }

        public List<Image> Imagens { get => imagens; set => imagens = value; }
        public Dictionary<string, List<Image>> SmartList { get => smartList; set => smartList = value; }

        public void LoadLibrary()
        {
            string pathproperties = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\csvtest.txt";
            StreamReader st = new StreamReader(pathproperties);

            while (!st.EndOfStream)
            {
                string line = st.ReadLine();
                List<Label> label = new List<Label>();
                if (line != "#")
                {
                    string[] lin = line.Split(',');
                    int values = Convert.ToInt32(lin[1]) * 2 + 1;
                    for (int i = 2; i <= values; i += 2)             //Se cargan las SimpleLabel
                    {
                        SimpleLabel tag = new SimpleLabel(lin[i + 1], Convert.ToInt32(lin[i]));
                        label.Add(tag);
                        
                    }
                    int numPersonalLabel = Convert.ToInt32(lin[values + 1]);      //Se cargan las PersonLabel
                    int j;
                    for (j = values + 2; j <= (numPersonalLabel * 9) + numPersonalLabel + values - 2; j += 9)
                    {
                        //Aqui cada uno  de los aux representa un atributo de PersonLabel
                        int serial = Convert.ToInt32(lin[j]);
                        string aux = lin[j + 1];
                        string[] dou = lin[j + 2].Split('.');
                        double[] aux1 = new double[4] { Convert.ToDouble(dou[0].TrimStart('(')), Convert.ToDouble(dou[1]), Convert.ToDouble(dou[2]), Convert.ToDouble(dou[3].TrimEnd(')')) };
                        string aux2 = lin[j + 3];
                        ENationality aux3 = (ENationality)Enum.Parse(typeof(ENationality), lin[j + 4]);                        
                        EColor aux4 = (EColor)Enum.Parse(typeof(EColor), lin[j + 5]);           
                        EColor aux5 = (EColor)Enum.Parse(typeof(EColor), lin[j + 6]);                        
                        ESex aux6 = (ESex)Enum.Parse(typeof(ESex), lin[j + 7]);                     
                        string aux7 = lin[j + 8];
                        PersonLabel tag1 = new PersonLabel(aux, aux1, aux2, aux3, aux4, aux5, aux6, aux7, serial);
                        label.Add(tag1);
                    }
                    
                    int numSpecialLabel = Convert.ToInt32(lin[j]);
                    for (int d = j+1 ; d < lin.Length-1; d += 6)       // Se cargan las SpeciaLabel
                    {
                        int p6 = Convert.ToInt32(lin[d]);
                        string[] dou1 = lin[d+1].Split('.');
                        double[] p1 = new double[2] { Convert.ToDouble(dou1[0].TrimStart('{')), Convert.ToDouble(dou1[1].TrimEnd('}')) };
                        string p2 = lin[d + 2];
                        string p3 = lin[d + 3];
                        string p4 = lin[d + 4];
                        bool p5 = Convert.ToBoolean(lin[d + 5]);
                        
                        SpecialLabel tag2 = new SpecialLabel(p1, p2, p3, p4, p5, p6);
                        label.Add(tag2);
                    }

                    int nlist = lin.Length;
                    Image ima = new Image(lin[0], label, Convert.ToInt32(lin[nlist - 1]));
                    imagens.Add(ima);
                }
                else    //aqui empiezan los patrones
                {
                    while (!st.EndOfStream)
                    {
                        string lin2 = st.ReadLine();
                        List<Image> imagePatter = new List<Image>();      //EN ESTE MOMENTO LOS LIST ESTARAN VACIOS HASTA QUE HABLE CON MATIAS
                        smartList.Add(lin2, imagePatter);
                    }
                    
                }
                   

            }
            st.Close();


        }   

        public void UpdateLibrary()
        {
            throw new Exception("falta");
        }

        public void AddLabel(string nameImage, Label label)
        {
            //TENER UN METODO QUE SEA RANDOM Y VEA SI EXISTE UN NUMERO QUE NO ESTE OCUPADO POR SERIAL NUMBER
            // LUEGO NADA DARLE ESTE NUMERO Y AGREGAR LA ETIQUETA:D
        }

        public void RemoveLabel(string nameImage, int serialNumber)
        {
            //Eliminar de list<image>
            string pathproperties = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\csvtest.txt";
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\repositorio.txt";
            StreamReader st = new StreamReader(pathproperties);
            StreamWriter x = new StreamWriter(path);
            foreach (Image imag in imagens )
            {
                if (imag.Name == nameImage)
                {
                    Image var = new Image(imag.Name, imag.Labels, imag.Calification);
                    foreach(Label label in var.Labels)
                    {
                        if (label.SerialNumber == serialNumber)
                        {
                            imag.Labels.Remove(label);

                            while (!st.EndOfStream)
                            {
                                string line = st.ReadLine();
                                string[] lin = line.Split(',');
                                if (lin[0] == nameImage)
                                {
                                    
                                    switch (label.labelType)
                                    {

                                        case "SimpleLabel":

                                            x.Write(lin[0] + ",");
                                            x.Write(Convert.ToString(Convert.ToInt32(lin[1]) - 1) + ",");
                                            int values = Convert.ToInt32(lin[1]) * 2 + 1;
                                            for (int i = 2; i <= values;)
                                            {
                                                if (Convert.ToInt32(lin[i]) == serialNumber)
                                                {
                                                    i += 2;
                                                }
                                                else
                                                {
                                                    x.Write(lin[i] + ",");
                                                    i += 2;
                                                }
                                            }
                                            for (int s = values; s < lin.Length; s++)
                                            {
                                                if (s == lin.Length - 1)
                                                {
                                                    x.Write(lin[s]);
                                                }
                                                else
                                                {
                                                    x.Write(lin[s] + ",");
                                                }
                                            }
                                            x.WriteLine();
                                            break;
                                        case "PersonLabel":
                                            Console.WriteLine("p");
                                            x.Write(lin[0] + ",");
                                            int v = Convert.ToInt32(lin[1]) * 2 + 1;

                                            int numPersonalLabel = Convert.ToInt32(lin[v + 1]);     
                                            int j;
                                            for (int i = 1; i <= v; i++)
                                            {
                                                x.Write(lin[i] + ",");
                                            }
                                            x.Write(Convert.ToString(Convert.ToInt32(lin[v + 1]) - 1 + ","));
                                            for (j = v + 2; j <= (numPersonalLabel * 9) + numPersonalLabel + v - 1; j += 9)
                                            {
                                                if (Convert.ToInt32(lin[j]) == serialNumber)
                                                {

                                                }
                                                else
                                                {
                                                    x.Write(lin[j] + "," + lin[j + 1] + "," + lin[j + 2] + "," + lin[j + 3] + "," + lin[j + 4] + "," + lin[j + 5] + "," + lin[j + 6] + "," + lin[j + 7] + "," + lin[j + 8] + ",");

                                                }
                                            }
                                            for (int s = (numPersonalLabel * 9) + numPersonalLabel + v - 1; s < lin.Length; s++)
                                            {
                                                if (s == lin.Length - 1)
                                                {
                                                    x.Write(lin[s]);
                                                }
                                                else
                                                {
                                                    x.Write(lin[s] + ",");
                                                }
                                            }
                                            x.WriteLine();
                                            break;
                                        case "SpecialLabel":  //tengo que decirle a gf que le ponga type en la clase
                                            Console.WriteLine("estoy aqui");
                                            int va = Convert.ToInt32(lin[1]) * 2 + 1;
                                            int numPersonalLabe = Convert.ToInt32(lin[va + 1]);
                                            for (int d = 0; d < (numPersonalLabe * 9) + numPersonalLabe + va - 1; d++)
                                            {
                                                x.Write(lin[d] + ",");
                                            }
                                            x.Write(lin[(numPersonalLabe * 9) + numPersonalLabe + va - 1]);
                                            for (int c = (numPersonalLabe * 9) + numPersonalLabe + va - 2; c < lin.Length; c += 6)
                                            {
                                                if (c == lin.Length - 1)
                                                {
                                                    x.Write(lin[c]);
                                                }
                                                else
                                                {
                                                    x.Write(lin[c] + "," + lin[c + 1] + "," + lin[c + 2] + "," + lin[c + 3] + "," + lin[c + 4] + lin[c + 5] + ",");
                                                }
                                            }
                                            x.WriteLine();
                                            break;

                                    }
                                } 
                                else
                                { 
                                    x.WriteLine(line);
                                }
                                
                            }
                            x.Close();
                            st.Close();
                            File.Delete(pathproperties);
                            File.Copy(path, pathproperties);
                            File.Delete(path);


                            break;
                        }
                        else
                        {
                            continue;
                        }
                        
                    }
                }
                else
                {
                }
            }
        }  

        public bool ChangeRating(string nameImage, int rating)
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
                return true;
            }
            st.Close();
            p.Close();
            File.Delete(pathproperties);
            File.Copy(patho, pathproperties);
            File.Delete(patho);
            return false;

            
            
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

