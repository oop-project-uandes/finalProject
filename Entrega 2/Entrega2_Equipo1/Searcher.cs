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
                foreach (Image image in images)
                {
                    int cont = 0;
                    foreach (List<string> atributes in subDec)
                    {
						switch (atributes[0])
						{
							case "HairColor":
								if (image.SomePersonLabelContains(atributes[0], null, ENationality.None, (EColor)Enum.Parse(typeof(EColor), atributes[1])))
								{
                                    cont++;
								}
								break;
							case "EyesColor":
								if (image.SomePersonLabelContains(atributes[0], null, ENationality.None, (EColor)Enum.Parse(typeof(EColor), atributes[1])))
								{
                                    cont++;
                                }
								break;
							case "Sex":
								if (image.SomePersonLabelContains(atributes[0], null, ENationality.None, EColor.None, (ESex)Enum.Parse(typeof(ESex), atributes[1])))
								{
                                    cont++;
                                }
								break;
							case "Name":
								if (image.SomePersonLabelContains(atributes[0], atributes[1]))
								{
                                    cont++;
                                }
								break;
							case "Surname":
                                //Console.WriteLine(atributes[1]);
								if (image.SomePersonLabelContains(atributes[0], atributes[1]))
								{
                                    cont++;
                                }
								break;
							case "Birthdate":
								if (image.SomePersonLabelContains(atributes[0], atributes[1]))
								{
                                    cont++;
                                }
								break;
							
							case "FaceLocation":
                                string[] subFACEstring = atributes[1].Split(new string[] { "," }, StringSplitOptions.None);
                                double[] faceCoords = { Convert.ToDouble(subFACEstring[0]), Convert.ToDouble(subFACEstring[1]), Convert.ToDouble(subFACEstring[2]), Convert.ToDouble(subFACEstring[3]) };
                                if (image.SomePersonLabelContains(atributes[0], null, ENationality.None, EColor.None, ESex.None, faceCoords))
								{
                                    cont++;
                                }
                                break;
							
							case "Nationality":
								if (image.SomePersonLabelContains(atributes[0], null, (ENationality)Enum.Parse(typeof(ENationality), atributes[1])))
								{
                                    cont++;
                                }
								break;

                            case "GeographicLocation":
                                
                                string[] substring = atributes[1].Split(new string[] { "," }, StringSplitOptions.None);
                                double[] coords = { Convert.ToDouble(substring[0]), Convert.ToDouble(substring[1]) };
                                if (image.SomeSpecialLabelContains(atributes[0], coords))
                                {
                                    cont++;
                                }
                                break;

                            case "Address":
                                if (image.SomeSpecialLabelContains(atributes[0], null, atributes[1]))
                                {
                                    cont++;
                                }
                                break;
                            case "Photographer":
                                if (image.SomeSpecialLabelContains(atributes[0], null, atributes[1]))
                                {
                                    cont++;
                                }
                                break;
                            case "Photomotive":
                                if (image.SomeSpecialLabelContains(atributes[0], null, atributes[1]))
                                {
                                    cont++;
                                }
                                break;
                            case "Selfie":
                                if (image.SomeSpecialLabelContains(atributes[0], null, null, Convert.ToBoolean(atributes[1])))
                                {
                                    cont++;
                                }
                                break;
                            case "Sentence":
                                if (image.SomeSimpleLabelContains(atributes[0], atributes[1]))
                                {
                                    cont++;
                                }
                                break;
                            case "ImageName": //string
                                if (image.Name == atributes[1])
                                {
                                    cont++;
                                }
                                break;
                            case "Calification": // int 
                                try
                                {
                                    if (image.Calification == Convert.ToInt32(atributes[1]))
                                    {
                                        cont++;
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Calification => Parameter: {0} must be an integer", atributes[1]);
                                }
                                break;
                            case "Resolution": // int[]
                                try
                                {
                                    string[] resString = atributes[1].Split(new string[] { "," }, StringSplitOptions.None);
                                    int[] resolution = { Convert.ToInt32(resString[0]), Convert.ToInt32(resString[1]) };
                                    if (image.Resolution[0] == resolution[0] && image.Resolution[1] == resolution[1])
                                    {
                                        cont++;
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Resolution => Parameter: {0} must be an integer array", atributes[1]);
                                }
                                break;
                            case "AspectRatio": // int[]
                                try
                                {
                                    string[] aspectString = atributes[1].Split(new string[] { "," }, StringSplitOptions.None);
                                    int[] aspectRatio = { Convert.ToInt32(aspectString[0]), Convert.ToInt32(aspectString[1]) };
                                    if (image.Resolution[0] == aspectRatio[0] && image.Resolution[1] == aspectRatio[1])
                                    {
                                        cont++;
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("AspectRatio => Parameter: {0} must be an integer array", atributes[1]);
                                }
                                break;
                            case "DarkClear": // bool
                                try
                                {
                                    if ( image.DarkClear == Convert.ToBoolean(atributes[1]))
                                    {
                                        cont++;
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("DarkClear => Parameter: {0} must be a boolean", atributes[1]);
                                }
                                break;
                        }               
                    }
                if (cont == subDec.Count) //Contador de parametros que calzan, es igual a la cantidad de parametros en subDec
                    {
                        temp.Add(image);
                    }
                }
                Total.Add(temp);
            }
            //Image Filter
            foreach(List<Image> subList  in Total)
            {
                foreach(Image tempImage in subList)
                {
                    if (!Final.Contains(tempImage))
                    {
                        Final.Add(tempImage);
                    }
                    
                }
            }

			return Final;
		}

        public List<System.Drawing.Bitmap> FaceSearcher(string PersonName, List<Image> images)
        {
			List<Image> resultImages = Search(images, "Name: "+PersonName); //Must search by the following format: "Matias", list of images where "Matias" appears in
			Dictionary<System.Drawing.Bitmap, double[]> imageDict = new Dictionary<System.Drawing.Bitmap, double[]>(); //Dictionary made out of Bitmap and coordinates
			foreach (Image image in resultImages)
			{
				List<PersonLabel> labels = image.SelectPersonLabels(); //List of PersonLabel of one image
				foreach (PersonLabel label in labels) //PersonLabel of labels
				{
					if (label.Name == PersonName && label.FaceLocation != null)
					{
						double[] faceCoord = label.FaceLocation;
						System.Drawing.Bitmap bitImage = image.BitmapImage;
						imageDict.Add(bitImage, faceCoord); 
					}
				}
			}
			return scissors.Crop(imageDict); //returns a List<System.Drawing.Bitmap>
        }
    
        public List<List<List<string>>> Declaration(string Declaration)
        {
            List<List<List<string>>> Total = new List<List<List<string>>>();
            
            if (Declaration.Contains(" or ")) //Note: before it was .Contains("or") and now is .Contains(" or ")
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

