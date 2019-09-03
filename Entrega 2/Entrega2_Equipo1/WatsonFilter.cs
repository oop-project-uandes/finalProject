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
        private const string APIKEY = "jL9phS26mYRAEf8iJ3yZEdnHKr1NNaZ5H97oWpJ_EtyJ";
        private const string ENDPOINT = "https://gateway.watsonplatform.net/visual-recognition/api";
        private const string VERSION_DATE = "2018-03-19";
        private Scissors scissors;
        private Dictionary<int, Dictionary<string, object>> LastFacesRequest;
        private IBM.WatsonDeveloperCloud.VisualRecognition.v3.VisualRecognitionService _visualRecognition;

        // WatsonFilter's builder
        public WatsonFilter()
        {
            this.scissors = new Scissors();
            this.LastFacesRequest = new Dictionary<int, Dictionary<string, object>>();
            IBM.WatsonDeveloperCloud.Util.TokenOptions options = new IBM.WatsonDeveloperCloud.Util.TokenOptions();
            options.IamApiKey = APIKEY;
            this._visualRecognition = new IBM.WatsonDeveloperCloud.VisualRecognition.v3.VisualRecognitionService(options, VERSION_DATE);
            this._visualRecognition.SetEndpoint(ENDPOINT);
        }


        // Hay que arreglar el desastre
        

        private Dictionary<int, Dictionary<string,object>> ClassifyFaces(string pathtofile)
        {
            FileStream stream = new FileStream(pathtofile, FileMode.Open);
            var resultFaces = _visualRecognition.DetectFaces(stream);

            Dictionary<int, Dictionary<string, object>> result = new Dictionary<int, Dictionary<string, object>>();
            foreach (IBM.WatsonDeveloperCloud.VisualRecognition.v3.Model.ImageWithFaces image in resultFaces.Images)
            {
                int i = 1;
                foreach (IBM.WatsonDeveloperCloud.VisualRecognition.v3.Model.Face face in image.Faces)
                {
                    
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
                    Dictionary<string, object> auxDic = new Dictionary<string, object>();
                    auxDic.Add("age", new Age(agemin, agemax, agescore));
                    auxDic.Add("gender", new Gender(gender, genderLabel, genderScore));
                    auxDic.Add("position", new FacePosition(widthLocation, heightLocation, left, top));
                    result.Add(i, auxDic);
                    i++;
                    
                }
            }
            stream.Close();
            return result;
        }

        // Falta hacer la misma organizacion que se realizo con detect faces, en este caso
        private Dictionary<int, Dictionary<string, object>> Classify(string pathtofile)
        {
            FileStream stream = new FileStream(pathtofile, FileMode.Open);
            var resultIdentifiers = _visualRecognition.Classify(stream);
            Console.WriteLine(JsonConvert.SerializeObject(resultIdentifiers, Newtonsoft.Json.Formatting.Indented));
            stream.Close();
            Console.ReadKey();
        }
















        // Methods

        /*Method to apply the WatsonFilter to a System.Drawing.Bitmap. It returns a dictionary
        with sex, age, and their probabilities of people in the picture */
        public Dictionary<string, string> applyFilter(Bitmap image)
        {
            throw new NotImplementedException("Not implemented yet");
        }

        /*Method to cut the faces of the people on the picture, based on the LastRequest made
        to the filter*/
        public List<Bitmap> CutFaces(Bitmap image)
        {
            throw new NotImplementedException("Not implemented yet");
        }

    }
}
