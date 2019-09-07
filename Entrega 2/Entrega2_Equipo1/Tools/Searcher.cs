using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1.Tools
{
    public class Searcher
    {
        private Scissors scissors;

        public Searcher() { }

        public List<Image> Search(List<Image> images, string searchDeclaration)
        {
            return null;
        }

        public List<System.Drawing.Bitmap> FaceSearcher(string PersonName, List<Image> images)
        {
            return null;
        }

        public List<List<List<string>>> Declaration(string Declaration)
        {
            List<List<List<string>>> Total = new List<List<List<string>>>();
            List<List<string>> subDec = new List<List<string>>();
            char [] splits = new char [] {'o','r' };
            string[] declarationsArray = Declaration.Split(splits);
            foreach (string SubDeclaration in declarationsArray)
            {

            }
            return null;
        }

       
    }
}
