using Entrega2_Equipo1.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    public class Producer
    {
        private WorkingArea WorkingArea;
        private List<Tool> tools;

        public Producer()
        {
            this.WorkingArea = new WorkingArea();
            this.tools = new List<Tool>() { new Brush(), new Merger(), new Resizer(),
                new Scissors(), new Zoom(), new AddCensorship(), new AddImage(),
                new AddShape(), new AddText(), new WatsonAnalizer(), new AutomaticAdjustmentFilter(),
                new BlackNWhiteFilter(), new BrightnessFilter(), new ColorFilter(), new InvertFilter(),
                new MirrorFilter(), new OldFilmFilter(), new RotateFlipFilter(), new SepiaFilter(), new WindowsFilter()};
        }


        public Dictionary<int, Dictionary<string, double>> ClassifyImage(string path)
        {
            Bitmap bitmapImage = new Bitmap(path);
            WatsonAnalizer myFilter = (WatsonAnalizer)this.tools[9];
            Dictionary<int, Dictionary<string, double>> resultadoClasificacion = myFilter.FindClassifiers(bitmapImage);
            return resultadoClasificacion;
        }


        public bool Presentation(List<string> nombresImagenes)
        {
            throw new NotImplementedException();
        }

        public bool Slideshow(List<string> nombresImagenes)
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Bitmap Merge(List<string> nombresImagenes)
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Bitmap Mosaic (string imagenBase, List<string> nombresImagenes)
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Bitmap Collage(List<string> nombresImágenes, double[] tamanoFondo, double[] tamanosImagenes, System.Drawing.Bitmap imagenFondo)
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Bitmap Album(List<string> nombresImagenes, int cantFotosXPagina)
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Bitmap Calendar(string[] nombresImágenes, int anio)
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Bitmap ApplyFilter(string nombreImagen, EFilter filtro, string Texto = null)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> SexAndAgeRecognition(string nombreImagen)
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Bitmap PixelCensorship(string nombreImagen, double[] posicion)
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Bitmap BlackCensorship(string nombreImagen, double[] posicion)
        {
            throw new NotImplementedException();
        }

    }
}
