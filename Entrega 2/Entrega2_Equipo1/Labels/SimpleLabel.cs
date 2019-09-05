using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    public class SimpleLabel : Label
    {
        private string sentence;
        private const string DEFAULT_SENTENCE = null;

        
        public SimpleLabel() : base(DEFAULT_SERIAL_NUMBER)
        {
            this.sentence = DEFAULT_SENTENCE;
        }

        public SimpleLabel(string sentence) : this(sentence, DEFAULT_SERIAL_NUMBER) {}

        public SimpleLabel(string sentence, int serialNumber) : base(serialNumber)
        {
            this.sentence = sentence;
        }

        public SimpleLabel(int serialNumber) : base(serialNumber)
        {
            this.sentence = DEFAULT_SENTENCE;
        }

        public string Sentence
        {
            get => this.sentence;
            set => this.sentence = value;
        }

    }
}
