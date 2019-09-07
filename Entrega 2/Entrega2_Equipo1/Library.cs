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
        private List<Image> foto;

        /*
        private Dictionary <string patron, List<Image> image> smartList;
        */


        public Library()
        {
            this.foto = new List<Image>();

        }

        public void LoadLibrary()
        {
            string pathproperties = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\csvtest.txt";
            Console.WriteLine(pathproperties);
            StreamReader st = new StreamReader(pathproperties);

            while (!st.EndOfStream)
            {
                string line = st.ReadLine();
                /*
                string[] subdivitions=line.Split(',');
                for (int i = 0; i < subdivitions.Length; i++)
                {
                    //Console.WriteLine(subdivitions[i]);
                }
                for (int i = 2; i <= Convert.ToInt32(subdivitions[1])+1; i++)
                {
                    //Console.WriteLine(subdivitions[i]);
                }
                for (int i = Convert.ToInt32(subdivitions[1]) + 3; i <= (Convert.ToInt32(subdivitions[(Convert.ToInt32(subdivitions[1]) + 2)])*8)+ Convert.ToInt32(subdivitions[1]) + 2; i++)
                {
                    //Console.WriteLine(subdivitions[i]);
                }
                int aux=(Convert.ToInt32(subdivitions[(Convert.ToInt32(subdivitions[1]) + 2)]) * 8) + Convert.ToInt32(subdivitions[1]) + 3;
                for (int i = aux+1; i <=Convert.ToInt32(subdivitions[aux])+aux ; i++)
                {
                    //Console.WriteLine(subdivitions[i]);
                }
                */
                List<Label> label = new List<Label>();
                if (line != "#")
                {
                    string[] lin = line.Split(',');
                    int values = Convert.ToInt32(lin[1]) + 1;
                    
                    //guardado simple label
                    for (int i = 2; i <= values; i++)
                    {
                        SimpleLabel tag = new SimpleLabel(lin[i]);
                        label.Add(tag);
                    }
                    //guardado personal label
                    int numPersonalLabel = Convert.ToInt32(lin[values + 1]);
                    int j;
                    for (j = values + 2; j < numPersonalLabel * 8; j += 7)
                    {

                        //Aqui cada uno  de los aux representa un atributo de PersonLabel
                        string aux = lin[j];
                        Console.WriteLine(aux);
                        string[] dou = lin[j + 1].Split('.'); //Aqui tengo que quitar el ( y ) del final
                        Console.WriteLine(lin[j+1]);
                        Console.WriteLine(dou[0]);
                        double[] aux1 = new double[4] { Convert.ToDouble(dou[0]), Convert.ToDouble(dou[1]), Convert.ToDouble(dou[2]), Convert.ToDouble(dou[3]) };
                        Console.WriteLine(aux1);
                        string aux2 = lin[j + 2];
                        Console.WriteLine(aux2);
                        ENationality aux3 = (ENationality)Enum.Parse(typeof(ENationality), lin[j + 3]);
                        Console.WriteLine(aux3);
                        EColor aux4 = (EColor)Enum.Parse(typeof(EColor), lin[j + 4]);
                        Console.WriteLine(aux4);
                        EColor aux5 = (EColor)Enum.Parse(typeof(EColor), lin[j + 5]);
                        Console.WriteLine(aux5);
                        ESex aux6 = (ESex)Enum.Parse(typeof(ESex), lin[j + 6]);
                        Console.WriteLine(aux6);
                        string aux7 = lin[j + 7];
                        Console.WriteLine(aux7);
                        //PersonLabel tag1 = new PersonLabel(aux,aux1,aux2, aux3, aux4, aux5, aux6, aux7);
                        //label.Add(tag1);

                       
                        


                    }
                    
                }
                else
                {

                }
                
                //return true;
                
            }
            st.Close();
        }
    }
}

