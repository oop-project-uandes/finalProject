using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1.Labels
{
    public class PersonLabel : Label
    {
        private string name;
        private const string DEFAULT_SURNAME = null;
        private string surname;
        private const ENationality DEFAULT_NATIONALITY = ENationality.None;
        private ENationality nationality;
        private const EColor DEFAULT_COLOR = EColor.None;
        private EColor eyesColor;
        private EColor hairColor;
        private const ESex DEFAULT_SEX = ESex.None;
        private ESex sex;
        private static readonly DateTime DEFAULT_BIRTHDATE = new DateTime(1900, 1, 1);
        private DateTime birthDate;
        private double[] faceLocation;
        private string type;

        public PersonLabel(string name, double[] faceLocation) : this(name, faceLocation, DEFAULT_SURNAME) { }

        public PersonLabel(string name, double[] faceLocation, string surname) : this(name, faceLocation, surname, DEFAULT_NATIONALITY) { }

        public PersonLabel(string name, double[] faceLocation, string surname, ENationality nationality) : this(name, faceLocation, surname, nationality, DEFAULT_COLOR) { }

        public PersonLabel(string name, double[] faceLocation, string surname, ENationality nationality, EColor eyesColor) : this(name, faceLocation, surname, nationality, eyesColor, DEFAULT_COLOR) { }

        public PersonLabel(string name, double[] faceLocation, string surname, ENationality nationality, EColor eyesColor, EColor hairColor) : this(name, faceLocation, surname, nationality, eyesColor, hairColor, DEFAULT_SEX) { }

        public PersonLabel(string name, double[] faceLocation, string surname, ENationality nationality, EColor eyesColor, EColor hairColor, ESex sex) : this(name, faceLocation, surname, nationality, eyesColor, hairColor, sex, DEFAULT_BIRTHDATE) { }

        public PersonLabel(string name, double[] faceLocation, string surname, ENationality nationality, EColor eyesColor, EColor hairColor, ESex sex, DateTime birthDate) : base(DEFAULT_SERIAL_NUMBER)
        {
            this.Name = name;
            this.FaceLocation = faceLocation;
            this.Surname = DEFAULT_SURNAME;
            this.Nationality = DEFAULT_NATIONALITY;
            this.EyesColor = DEFAULT_COLOR;
            this.HairColor = DEFAULT_COLOR;
            this.Sex = DEFAULT_SEX;
            this.BirthDate = DEFAULT_BIRTHDATE;
            this.labelType = "PersonLabel";
        }

        
        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public ENationality Nationality { get => nationality; set => nationality = value; }
        public EColor EyesColor { get => eyesColor; set => eyesColor = value; }
        public EColor HairColor { get => hairColor; set => hairColor = value; }
        public ESex Sex { get => sex; set => sex = value; }
        public DateTime BirthDate { get => birthDate; set => birthDate = value; }
        public double[] FaceLocation { get => faceLocation; set => faceLocation = value; }
        protected override string labelType { get => this.labelType; set => this.labelType = value; }
    }
}

