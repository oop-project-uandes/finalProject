using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    public class Gender
    {
        string genderType;
        string genderLabel;
        double genderScore;
        public Gender(string gender, string genderLabel, double genderScore)
        {
            this.genderType = gender;
            this.genderLabel = genderLabel;
            this.genderScore = genderScore;
        }
        public string GenderType { get => genderType; set => genderType = value; }
        public string GenderLabel { get => genderLabel; set => genderLabel = value; }
        public double GenderScore { get => genderScore; set => genderScore = value; }
    }
}
