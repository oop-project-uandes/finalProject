using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using IBM.WatsonDeveloperCloud.VisualRecognition.v3;
using IBM.WatsonDeveloperCloud;
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
        private Dictionary<string, string> LastRequest;
        private VisualRecognitionService _visualRecognition;



        // WatsonFilter's builder
        public WatsonFilter()
        {
            this.scissors = new Scissors();
            this.LastRequest = new Dictionary<string, string>();
            IBM.WatsonDeveloperCloud.Util.TokenOptions options = new IBM.WatsonDeveloperCloud.Util.TokenOptions();
            options.IamApiKey = APIKEY;
            this._visualRecognition = new VisualRecognitionService(options, VERSION_DATE);
            this._visualRecognition.SetEndpoint(ENDPOINT);
        }

        // Prueba
        public void Classify(string pathtofile)
        {
            FileStream stream = new FileStream(pathtofile, FileMode.Open);
            var resultFaces = _visualRecognition.DetectFaces(stream);
            Console.WriteLine(JsonConvert.SerializeObject(resultFaces, Newtonsoft.Json.Formatting.Indented));
            stream.Close();

            stream = new FileStream(pathtofile, FileMode.Open);
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
