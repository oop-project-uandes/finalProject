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

			List<List<Image>> Total = new List<List<Image>>();
			List<Image> Final = new List<Image>();
			foreach (List<List<string>> subDec in Declarations)
            {
				List<Image> temp = new List<Image>();
				foreach (List<string> atributes in subDec)
                {
                    foreach (Image image in images)
                    {
						switch (atributes[0])
						{
							case "HairColor":
								if (image.SomePersonLabelContains(atributes[0], null, ENationality.None, (EColor)Enum.Parse(typeof(EColor), atributes[1])))
								{
									temp.Add(image);
								}
								break;
							case "EyeColor":
								if (image.SomePersonLabelContains(atributes[0], null, ENationality.None, (EColor)Enum.Parse(typeof(EColor), atributes[1])))
								{
									temp.Add(image);
								}
								break;
							case "Sex":
								if (image.SomePersonLabelContains(atributes[0], null, ENationality.None, EColor.None, (ESex)Enum.Parse(typeof(ESex), atributes[1])))
								{
									temp.Add(image);
								}
								break;
							case "Name":
								if (image.SomePersonLabelContains(atributes[0], atributes[1]))
								{
									temp.Add(image);
								}
								break;
							case "SurName":
								if (image.SomePersonLabelContains(atributes[0], atributes[1]))
								{
									temp.Add(image);
								}
								break;
							case "Birthdate":
								if (image.SomePersonLabelContains(atributes[0], atributes[1]))
								{
									temp.Add(image);
								}
								break;
							
							case "FaceLocation:
								if (image.SomePersonLabelContains(atributes[0], null, ENationality.None, EColor.None, ESex.None, Convert.ToDouble(atributes[1])))
								{
									temp.Add(image);
								}
							
							case "Nationality":
								if (image.SomePersonLabelContains(atributes[0], null, (ENationality)Enum.Parse(typeof(ENationality), atributes[1])))
								{
									temp.Add(image);
								}
								break;

                            case "GeographicLocation":
                                
                                string[] substring = atributes[1].Split(new string[] { "," }, StringSplitOptions.None);
                                double[] coords = [Convert.ToDouble(substring[0]),Convert.ToDouble(substring[1])];
                                if (image.SomeSpecialLabelContains(atributes[0], coords))
                                {
                                    temp.Add(image);
                                }
                                break;

                            case "Address":
                                if (image.SomeSpecialLabelContains(atributes[0], null, atributes[1]))
                                {
                                    temp.Add(image);
                                }
                                break;
                            case "Photographer":
                                if (image.SomeSpecialLabelContains(atributes[0], null, atributes[1]))
                                {
                                    temp.Add(image);
                                }
                                break;
                            case "Photomotive":
                                if (image.SomeSpecialLabelContains(atributes[0], null, atributes[1]))
                                {
                                    temp.Add(image);
                                }
                                break;
                            case "Selfie":
                                if (image.SomeSpecialLabelContains(atributes[0], null, null, Convert.ToBoolean(atributes[1])))
                                {
                                    temp.Add(image);
                                }
                                break;
                            case "Sentence":
                                if (image.SomeSimpleLabelContains(atributes[0], atributes[1]))
                                {
                                    temp.Add(image);
                                }
                                break;

                        }               
                    }
                }
            }
			return Final;
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

