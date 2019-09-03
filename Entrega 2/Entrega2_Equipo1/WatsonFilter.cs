using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using IBM.WatsonDeveloperCloud.VisualRecognition.v3;
using System.IO;
using Newtonsoft.Json;

namespace Entrega2_Equipo1
{
    class WatsonFilter
    {
        // WatsonFilter's attributes
        private const string APIKEY = "";
        private const string ENDPOINT = "https://gateway.watsonplatform.net/visual-recognition/api";
        private const string VERSION_DATE = "2018-03-19";
        private Scissors scissors;
        private IBM.WatsonDeveloperCloud.VisualRecognition.v3.VisualRecognitionService _visualRecognition;


        // WatsonFilter's builder
        public WatsonFilter()
        {
            this.scissors = new Scissors();
            this.LastFacesRequest = new Dictionary<int, Dictionary<string, object>>();
            this.LastClassifyRequest = new Dictionary<int, Dictionary<string, double>>();
            IBM.WatsonDeveloperCloud.Util.TokenOptions options = new IBM.WatsonDeveloperCloud.Util.TokenOptions();
            options.IamApiKey = APIKEY;
            this._visualRecognition = new IBM.WatsonDeveloperCloud.VisualRecognition.v3.VisualRecognitionService(options, VERSION_DATE);
            this._visualRecognition.SetEndpoint(ENDPOINT);
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
         * is a Gender object, the value of "position" is a Position object,
         * and the "bitmap" value is a System.Drawing.Bitmap of the face of the person*/
        public Dictionary<int,Dictionary<string, object>> FindFaces(Bitmap image)
        {
            Dictionary<int, Dictionary<string, object>> returningDic = new Dictionary<int, Dictionary<string, object>>();
            // Primero hay que salvar el bitmap en una direccion. Dicha direccion se le pasa a ClassifyFaces,
            // se obtiene la posicion de la cara, se recorta, y se agregan todos los valores al returningDic.
            // Luego se elimina la imagen guardada, y se retorna. 
            return returningDic;
        }

        

        public Dictionary<int, Dictionary<string, double>> FindClassifiers(Bitmap image)
        {
            Dictionary<int, Dictionary<string, double>> returningDic = new Dictionary<int, Dictionary<string, double>>();
            //Primero hay que salvar el bitmap en una direccion, Dicha direccion se le pasa a ClassifyFaces,
            // se obtiene el resultado, se elimina la imagen guardada, y se retorna el resultado.
            return returningDic;
        }

    }
}
