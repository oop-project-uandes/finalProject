using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    public class Gender
    {
        string gender;
        string genderLabel;
        double genderScore;
        public Gender(string gender, string genderLabel, double genderScore)
        {
            this.gender = gender;
            this.genderLabel = genderLabel;
            this.genderScore = genderScore;
        }
        public string MinAge { get => gender; set => gender = value; }
        public string MaxAge { get => genderLabel; set => genderLabel = value; }
        public double GenderScore { get => genderScore; set => genderScore = value; }
    }
}
