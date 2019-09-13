using Entrega2_Equipo1.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    public class Producer
    {
        private WorkingArea WorkingArea;
        private List<Tool> tools;

        public Producer(WorkingArea workingArea,List<Tool> tools)
        {
            this.WorkingArea = workingArea;
            this.tools = tools;
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

        public System.Drawing.Bitmap Collage(List<string> nombresImágenes, double[2] tamanoFondo, double[] tamanosImagenes, System.Drawing.Bitmap imagenFondo)
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
