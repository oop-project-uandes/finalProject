using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    public class Age
    {
        int minAge;
        int maxAge;
        double ageScore;
        public Age(int minAge, int maxAge, double ageScore)
        {
            this.minAge = minAge;
            this.maxAge = maxAge;
            this.ageScore = ageScore;
        }
        public int MinAge { get => minAge; set => minAge = value; }
        public int MaxAge { get => maxAge; set => maxAge = value; }
        public double ScoreAge { get => ageScore; set => ageScore = value; }
    }
}
