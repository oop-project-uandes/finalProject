using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1.Labels
{
    public class SpecialLabel : Label
    {
        private double[] geographicLocation;
        private readonly double[] DEFAULT_GEOGRAPHIC_LOCATION = null;
        private string address;
        private const string DEFAULT_ADDRESS = null;
        private string photographer;
        private const string DEFAULT_PHOTOGRAPHER = null;
        private string photoMotive;
        private const string DEFAULT_PHOTOMOTIVE = null;
        private bool selfie;
        private const bool DEFAULT_SELFIE = false;
        private int serialNumber;

        // Falta trabajar en los getters y setters. En algunos como la ubicacion geografica se debe revisar que
        // sea valido el valor que se intenta ingresar.
    }
}
