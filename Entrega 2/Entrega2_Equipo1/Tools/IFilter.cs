﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Entrega2_Equipo1
{
    public interface IFilter
    {
        Bitmap ApplyFilter(Bitmap image);
    }
}
