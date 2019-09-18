using Entrega2_Equipo1.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    [Serializable]
    public class Producer
    {
        private WorkingArea WorkingArea;
        private List<Tool> tools;

        public Producer()
        {
            this.WorkingArea = new WorkingArea();
            this.tools = new List<Tool>() { new Brush(), new Merger(), new Resizer(),
                new Scissors(), new Zoom(), new AddCensorship(), new AddImage(),
                new AddShape(), new AddText(), new AutomaticAdjustmentFilter(),
                new BlackNWhiteFilter(), new BrightnessFilter(), new ColorFilter(), new InvertFilter(),
                new MirrorFilter(), new OldFilmFilter(), new RotateFlipFilter(), new SepiaFilter(), new WindowsFilter()};
        }


        public void DeleteImageInTheWorkingArea(int position)
        {
            WorkingArea.WorkingAreaImages.RemoveAt(position);
            return;
        }

        public List<Image> imagesInTheWorkingArea()
        {
            return this.WorkingArea.WorkingAreaImages;
        }

        public void LoadWatsonAnalyzer()
        {
            this.tools.Add(new WatsonAnalizer());
        }

        public void DeleteWatsonAnalyzer()
        {
            this.tools.RemoveAt(19);
        }

        public void LoadImagesToWorkingArea(List<Image> images)
        {
            List<Image> workingAreaImages = new List<Image>();
            foreach (Image image in images)
            {
                List<Label> listLabelCopy = new List<Label>();
                foreach (Label label in image.Labels)
                {
                    if (label.labelType == "SimpleLabel")
                    {
                        SimpleLabel label2 = (SimpleLabel)label;
                        listLabelCopy.Add(new SimpleLabel(label2.Sentence));
                    }
                    else if (label.labelType == "PersonLabel")
                    {
                        PersonLabel label2 = (PersonLabel)label;
                        listLabelCopy.Add(new PersonLabel(label2.Name, label2.FaceLocation, label2.Surname, label2.Nationality, label2.EyesColor, label2.HairColor, label2.Sex, label2.BirthDate, label2.SerialNumber));
                    }
                    else if (label.labelType == "SpecialLabel")
                    {
                        SpecialLabel label2 = (SpecialLabel)label;
                        listLabelCopy.Add(new SpecialLabel(label2.GeographicLocation, label2.Address, label2.Photographer, label2.PhotoMotive, label2.Selfie, label2.SerialNumber));
                    }
                }

                Image newImage = new Image(image.Name, listLabelCopy, image.Calification, image.BitmapImage, image.Resolution, image.AspectRatio, image.DarkClear, image.Exif);
                workingAreaImages.Add(newImage);
            }
            foreach (Image image in workingAreaImages)
            {
                this.WorkingArea.WorkingAreaImages.Add(image);
            }

            return;
        }


        public Dictionary<int, Dictionary<string, double>> ClassifyImage(string path)
        {
            Bitmap bitmapImage = new Bitmap(path);
            WatsonAnalizer myFilter = (WatsonAnalizer)this.tools[9];
            Dictionary<int, Dictionary<string, double>> resultadoClasificacion = myFilter.FindClassifiers(bitmapImage);
            return resultadoClasificacion;
        }


        public bool Presentation(List<Image> images)
        {
            throw new NotImplementedException();
        }

        public bool Slideshow(List<Image> images)
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Bitmap Merge(List<Image> images)
        {
            List<Image> Final = new List<Image>();
            while (images.Count != 0)
            {
                int x = 0;
                int y = 0;
                Image final = images[0];
                for (int i = 0; i < images.Count; i++)
                {
                    if ((x >= images[i].Resolution[0] && y >= images[i].Resolution[1]) || (x == 0 && y == 0))
                    {
                        x = images[i].Resolution[0];
                        y = images[i].Resolution[1];
                        final = images[i];
                    }
                }
                Final.Add(final);
                images.Remove(final);
            }
            return MergeRecursive(Final, Final[0].BitmapImage, 2);
        }

        public System.Drawing.Bitmap MergeRecursive(List<Image> images, Bitmap merged = null, int cont = 2)
        {
            Merger merger = new Merger();
            if (images.Count == 2 && cont == 2)
            {
                cont++;
                return MergeRecursive(images, merger.Merge(images[0].BitmapImage, images[1].BitmapImage), cont);
            }
            else if (cont < images.Count)
            {
                cont++;
                return MergeRecursive(images, merger.Merge(merged, images[cont-1].BitmapImage), cont);

            }
            return merged;

        }

        public System.Drawing.Bitmap Mosaic (string imagenBase, List<Image> Imagenes)
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Bitmap Collage(List<string> nombresImágenes, double[] tamanoFondo, double[] tamanosImagenes, System.Drawing.Bitmap imagenFondo)
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Bitmap Album(List<string> nombresImagenes, int cantFotosXPagina)
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Bitmap Calendar(string[] nombresImágenes, int anio)
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Bitmap ApplyFilter(Image image ,EFilter filtro, Color color = default(Color), int brightness = 0, int noise = 60, 
            RotateFlipType RFT = RotateFlipType.RotateNoneFlipNone)
        {   
            switch (filtro)
            {
                case EFilter.AutomaticAdjustmentFilter:
                    AutomaticAdjustmentFilter AAF = new AutomaticAdjustmentFilter();
                    return AAF.ApplyFilter(image.BitmapImage);

                case EFilter.BlackNWhiteFilter:
                    BlackNWhiteFilter BNWF = new BlackNWhiteFilter();
                    return BNWF.ApplyFilter(image.BitmapImage);

                case EFilter.BrightnessFilter:
                    BrightnessFilter BF = new BrightnessFilter();
                    return BF.ApplyFilter(image.BitmapImage, brightness);
                
                case EFilter.ColorFilter:
                    ColorFilter CF = new ColorFilter();
                    return CF.ApplyFilter(image.BitmapImage, color);
                
                case EFilter.InvertFilter:
                    InvertFilter IF = new InvertFilter();
                    return IF.ApplyFilter(image.BitmapImage);

                case EFilter.MirrorFilter:
                    MirrorFilter MF = new MirrorFilter();
                    return MF.ApplyFilter(image.BitmapImage);

                case EFilter.OldFilmFilter:
                    OldFilmFilter OFF = new OldFilmFilter();
                    return OFF.ApplyFilter(image.BitmapImage, noise);

                case EFilter.RotateFlipFilter:
                    RotateFlipFilter RFF = new RotateFlipFilter();
                    return RFF.RotateFlip(image.BitmapImage, RFT);

                case EFilter.SepiaFilter:
                    SepiaFilter SF = new SepiaFilter();
                    return SF.ApplyFilter(image.BitmapImage);

                case EFilter.WindowsFilter:
                    WindowsFilter WF = new WindowsFilter();
                    return WF.ApplyFilter(image.BitmapImage);
            }
            return image.BitmapImage;
        }

        public Dictionary<string, int> SexAndAgeRecognition(Image image)
        {
            Dictionary<string, int> result = new Dictionary<string, int>() { };
            WatsonAnalizer WA = new WatsonAnalizer();
            Dictionary<int, Dictionary<string, object>> facesDict = WA.FindFaces(image.BitmapImage);
            double ageScore = 0;
            int maleScore = 0;
            int femaleScore = 0;
            int cont = 0;

            foreach (KeyValuePair<int, Dictionary<string,object>> pair in facesDict)
            {
                Dictionary<string, object> value = pair.Value;
                foreach(KeyValuePair<string,object> Scores in value)
                {
                    string String = Scores.Key;
                    object Object = Scores.Key;

                    switch (String)
                    {
                        case "age":
                            Age ageObj = (Age)Object;
                            ageScore += (ageObj.MaxAge-ageObj.MinAge)/ 2;
                            break;

                        case "gender":
                            Gender genderObj = (Gender)Object;
                            if (genderObj.GenderLabel == "male")
                            {
                                maleScore++;
                            }
                            else
                            {
                                femaleScore++;
                            }
                            break;
                    }
                }
                cont++;
            }
            if (maleScore > femaleScore)
            {
                result.Add("Male", Convert.ToInt32(Math.Floor(ageScore/cont)));
            }
            else if (maleScore < femaleScore)
            {
                result.Add("Female", Convert.ToInt32(Math.Floor(ageScore / cont)));
            }
            else
            {
                Random rnd = new Random();
                string[] genders = new string[2];
                genders[0] = "Male";
                genders[1] = "Female";
                result.Add(genders[rnd.Next(0,1)], Convert.ToInt32(Math.Floor(ageScore / cont)));
            }

            return result;
        }

        public System.Drawing.Bitmap PixelCensorship(string nombreImagen, double[] posicion)
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Bitmap BlackCensorship(Image image, int[] coordinates)
        {
            AddCensorship AC = new AddCensorship();
            return AC.blackCensorship(image.BitmapImage,coordinates);
        }

    }
}
