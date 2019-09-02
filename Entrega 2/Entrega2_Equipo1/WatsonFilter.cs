using System;
using System.Collections.Generic;
using System.Text;

namespace Entrega2_Equipo1
{
    class WatsonFilter
    {

        // WatsonFilter's attributes
        private Scissors scissors;
        private Dictionary<string, string> LastRequest;


        // WatsonFilter's Builder
        public WatsonFilter() { }


        // Methods
        // Method to apply the WatsonFilter to a System.Drawing.Bitmap. It returns a dictionary
        // with sex, age, and their probabilities of people in the picture
        public Dictionary<string, string> applyFilter(System.Drawing.Bitmap image)
        {
            throw new NotImplementedException("Not implemented yet");
        }

        // Method to cut the faces of the people on the picture, based on the LastRequest made
        // to the filter. s
        public List<System.Drawing.Bitmap> CutFaces(System.Drawing.Bitmap image)
        {
            throw new NotImplementedException("Not implemented yet");
        }

    }
}
