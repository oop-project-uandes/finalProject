using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    public class SimpleLabel : Label
    {
        private const int DEFAULT_MAX_LENGTH_SENTENCE = 20;
        private string sentence;
        private int serialNumber;

        public SimpleLabel() : base()
        { }

        public SimpleLabel(string sentence) : base()
        {
            this.sentence = sentence;
        }

        public SimpleLabel(string sentence, int serialNumber) : base()
        {
            this.sentence = sentence;
            this.serialNumber = serialNumber;
        }

        public string Sentence
        {
            get => this.sentence;
            set
            {
                if (value.Length > DEFAULT_MAX_LENGTH_SENTENCE)
                {
                    throw new Exception("Sentence too long. Didn't assign the Label");
                }
                else
                {
                    this.sentence = value;
                }
            }
        }
        
        public int SerialNumber
        {
            get => this.serialNumber;
            set => this.serialNumber = value;
        }

    }
}
