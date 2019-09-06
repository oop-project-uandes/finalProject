using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Entrega2_Equipo1
{
    public class Image
    {
        private List<Label> labels;
        private string name;
        private Bitmap image;
        private int calification;
        private Dictionary<string, string> exif;
        private double saturation;
        private int[] resolution;
        private int[] aspectRatio;
        private bool hdr;
        private bool darkClear;

        public string Width { get; internal set; }
    }
}
