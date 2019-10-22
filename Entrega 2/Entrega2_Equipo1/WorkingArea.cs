using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    [Serializable]
    public class WorkingArea
    {
        List<Image> workingAreaImages;

        public List<Image> WorkingAreaImages { get => this.workingAreaImages; set => this.workingAreaImages = value; }

        public WorkingArea()
        {
            this.workingAreaImages = new List<Image>();
        }
    }
}
