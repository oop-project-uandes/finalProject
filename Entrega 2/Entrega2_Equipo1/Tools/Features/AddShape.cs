using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    // TODO: IMPLEMENTAR
    [Serializable]
    public class AddShape : Tool
    {
        public AddShape() { }
        public Bitmap InsertShape(Bitmap bitmap,string shapeType, int xAxis, int yAxis, int width = 150, int height = 100, string colorName = null)
        {
            Bitmap temp = (Bitmap)bitmap.Clone();
            Graphics gr = Graphics.FromImage(temp);
            if (string.IsNullOrEmpty(colorName))
                colorName = "Black";
            Pen pen = new Pen(Color.FromName(colorName));
            switch (shapeType.ToLower())
            {
                case "filledellipse":
                    gr.FillEllipse(pen.Brush, xAxis,
              yAxis, width, height);
                    break;
                case "filledrectangle":
                    gr.FillRectangle(pen.Brush, xAxis,
              yAxis, width, height);
                    break;
                case "ellipse":
                    gr.DrawEllipse(pen, xAxis, yAxis, width, height);
                    break;
                case "rectangle":
                    gr.DrawRectangle(pen, xAxis, yAxis, width, height);
                    break;

            }
            return temp;
        }
    }
}
