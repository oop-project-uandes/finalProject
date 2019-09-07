using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    public class Searcher
    {
        private Scissors scissors;

        public Searcher() { }

        public List<Image> Search(List<Image> images, string searchDeclaration)
        {
            List<List<List<string>>> Declarations = Declaration(searchDeclaration);

            return null;
        }

        public List<System.Drawing.Bitmap> FaceSearcher(string PersonName, List<Image> images)
        {
            return null;
        }
    
        public List<List<List<string>>> Declaration(string Declaration)
        {
            List<List<List<string>>> Total = new List<List<List<string>>>();
            
            if (Declaration.Contains("or"))
            {
                foreach (string SubDeclaration0 in Declaration.Split(new string[] { " or " }, StringSplitOptions.None))
                {
                    List<List<string>> subDec = new List<List<string>>();
                    string SubDeclaration = SubDeclaration0.Replace(" ", "");
                    if (SubDeclaration.Contains("and"))
                    {
                        foreach (string atributes in SubDeclaration.Split(new string[] { "and" }, StringSplitOptions.None))
                        {
                            string[] atFinal = atributes.Split(new string[] { ":" }, StringSplitOptions.None);
                            subDec.Add(new List<string> { atFinal[0], atFinal[1] });
                        }
                        
                    }
                    else
                    {
                        
                        string[] atFinal = SubDeclaration.Split(new string[] { ":" }, StringSplitOptions.None);
                        subDec.Add(new List<string> { atFinal[0], atFinal[1] });

                    }
                    
                    Total.Add(subDec);
                }
                
            }
            else
            {
                List<List<string>> subDec = new List<List<string>>();
                if (Declaration.Contains("and"))
                {
                    foreach (string atributes0 in Declaration.Split(new string[] { "and" }, StringSplitOptions.None))
                    {
                        string atributes = atributes0.Replace(" ", "");

                        string[] atFinal = atributes.Split(new string[] { ":" }, StringSplitOptions.None);
                        subDec.Add(new List<string> { atFinal[0], atFinal[1] });

                    }
                }
                else
                {
                    string[] atFinal = Declaration.Split(new string[] { ":" }, StringSplitOptions.None);
                    subDec.Add(new List<string> { atFinal[0], atFinal[1] });
                }
                Total.Add(subDec);
            }

            return Total;
        }
            
    }

       
}

