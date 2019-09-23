using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Entrega2_Equipo1
{
    
    public class WatsonAnalizer : Tool
    {
        // WatsonFilter's attributes
        private const string APIKEY = "J";
        private const string ENDPOINT = "https://gateway.watsonplatform.net/visual-recognition/api";
        private const string VERSION_DATE = "2018-03-19";
        private IBM.WatsonDeveloperCloud.VisualRecognition.v3.VisualRecognitionService _visualRecognition;
        private Scissors scissors;


        // WatsonFilter's builder
        public WatsonAnalizer()
        {
            IBM.WatsonDeveloperCloud.Util.TokenOptions options = new IBM.WatsonDeveloperCloud.Util.TokenOptions();
            options.IamApiKey = APIKEY;
            _visualRecognition = new IBM.WatsonDeveloperCloud.VisualRecognition.v3.VisualRecognitionService(options, VERSION_DATE);
            _visualRecognition.SetEndpoint(ENDPOINT);
            this.scissors = new Scissors();
        }



        // Method to classify faces from an image
        private Dictionary<int, Dictionary<string, object>> ClassifyFaces(string pathtofile)
        {
            FileStream stream = new FileStream(pathtofile, FileMode.Open);
            var resultFaces = _visualRecognition.DetectFaces(stream);
            Dictionary<int, Dictionary<string, object>> dicFaces = new Dictionary<int, Dictionary<string, object>>();
            foreach (IBM.WatsonDeveloperCloud.VisualRecognition.v3.Model.ImageWithFaces image in resultFaces.Images)
            {
                int i = 1;
                foreach (IBM.WatsonDeveloperCloud.VisualRecognition.v3.Model.Face face in image.Faces)
                {
                    Dictionary<string, object> auxDic = new Dictionary<string, object>();
                    int agemin = Convert.ToInt32(face.Age.Min);
                    int agemax = Convert.ToInt32(face.Age.Max);
                    double agescore = Convert.ToDouble(face.Age.Score);
                    string gender = face.Gender.Gender;
                    string genderLabel = face.Gender.GenderLabel;
                    double genderScore = Convert.ToDouble(face.Gender.Score);
                    double widthLocation = Convert.ToDouble(face.FaceLocation.Width);
                    double heightLocation = Convert.ToDouble(face.FaceLocation.Height);
                    double left = Convert.ToDouble(face.FaceLocation.Left);
                    double top = Convert.ToDouble(face.FaceLocation.Top);
                    auxDic.Add("age", new Age(agemin, agemax, agescore));
                    auxDic.Add("gender", new Gender(gender, genderLabel, genderScore));
                    auxDic.Add("position", new FacePosition(widthLocation, heightLocation, left, top));
                    dicFaces.Add(i, auxDic);
                    i++;
                }
            }
            stream.Close();
            return dicFaces;
        }



        // Method to classify an image
        private Dictionary<int, Dictionary<string, double>> Classify(string pathtofile)
        {
            FileStream stream = new FileStream(pathtofile, FileMode.Open);
            var resultIdentifiers = _visualRecognition.Classify(stream);
            Dictionary<int, Dictionary<string, double>> dicClassify = new Dictionary<int, Dictionary<string, double>>();
            foreach (IBM.WatsonDeveloperCloud.VisualRecognition.v3.Model.ClassifiedImage image in resultIdentifiers.Images)
            {
                int i = 0;
                foreach (IBM.WatsonDeveloperCloud.VisualRecognition.v3.Model.ClassifierResult classResult in image.Classifiers)
                {
                    Dictionary<string, double> auxDic = new Dictionary<string, double>();
                    foreach (IBM.WatsonDeveloperCloud.VisualRecognition.v3.Model.ClassResult result in classResult.Classes)
                    {
                        string name = result.ClassName;
                        double score = Convert.ToDouble(result.Score);
                        auxDic.Add(name, score);
                    }
                    dicClassify.Add(i, auxDic);
                }
                i++;
            }
            stream.Close();
            return dicClassify;
        }



        /* Method to find the faces on a bitmap image.
         * The ints are the number of the images processed,
         * the string can be "age", "gender", "position" or "bitmap"
         * The value of "age" is an Age object, the value of "gender"
         * is a Gender object, the value of "position" is a Position object*/
        public Dictionary<int, Dictionary<string, object>> FindFaces(Bitmap image)
        {
            Dictionary<int, Dictionary<string, object>> returningDic = new Dictionary<int, Dictionary<string, object>>();
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\WatsonTempFiles\faces.jpg";
            /*
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            */
            image.Save(path);
            returningDic = ClassifyFaces(path);
            File.Delete(path);
            return returningDic;
        }




        /* Method to find classifiers of the image. The ints are the number
         of images processed, and every dictionary contains what classifiers Watson found
         in them. The string is the class, and the double the actual score*/
        public Dictionary<int, Dictionary<string, double>> FindClassifiers(Bitmap image)
        {
            Dictionary<int, Dictionary<string, double>> returningDic = new Dictionary<int, Dictionary<string, double>>();
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\WatsonTempFiles\classify.jpg";
            image.Save(path);
            returningDic = Classify(path);
            File.Delete(path);
            return returningDic;
        }




        // TODO: NEW FUNCTION, DONT KNOW IF IT WORKS
        public Dictionary<int, Dictionary<string, object>> FindFacesWithBmp(Bitmap image)
        {
            Dictionary<int, Dictionary<string, object>> returningDic = FindFaces(image);
            foreach (KeyValuePair<int,Dictionary<string,object>> pair in returningDic)
            {
                FacePosition facePosition = (FacePosition)pair.Value["position"];
                double[] croppingCoordinates = new double[] { facePosition.Left, facePosition.Top, facePosition.Width, facePosition.Height };
                returningDic[pair.Key].Add("bitmap", scissors.Crop(image,croppingCoordinates));
            }
            return returningDic;
        }
    }
}
