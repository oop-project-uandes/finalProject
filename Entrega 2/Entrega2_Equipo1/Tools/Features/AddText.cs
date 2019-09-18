using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    // TODO: Implementar
    [Serializable]
    public class AddText : Tool
    {
        public AddText() { }

        public Bitmap InsertText(Bitmap bitmap, string text, int xAxis, int yAxis, float fontSize = 10.0F, 
            string colorName1 = null, string fontStyle = "bold", string fontName = "Times New Roman" 
            , string colorName2 = null)
        {
            Bitmap temp = (Bitmap)bitmap.Clone();
            Graphics gr = Graphics.FromImage(temp);
            FontStyle fStyle = FontStyle.Regular;
            Font font = new Font(fontName, fontSize);
            switch (fontStyle.ToLower())
            {
                case "bold":
                    fStyle = FontStyle.Bold;
                    break;
                case "italic":
                    fStyle = FontStyle.Italic;
                    break;
                case "underline":
                    fStyle = FontStyle.Underline;
                    break;
                case "strikeout":
                    fStyle = FontStyle.Strikeout;
                    break;
            }
            font = new Font(fontName, fontSize, fStyle);
            if (string.IsNullOrEmpty(colorName1))
            {
                colorName1 = "Black";
            }
            if (string.IsNullOrEmpty(colorName2))
            {
                colorName2 = colorName1;
            }
            Color color1 = Color.FromName(colorName1);
            Color color2 = Color.FromName(colorName2);
            int gW = (int)(text.Length * fontSize);
            gW = gW == 0 ? 10 : gW;
            LinearGradientBrush LGBrush = new LinearGradientBrush(new Rectangle(0,0,gW,(int)fontSize),color1,color2,LinearGradientMode.Vertical);
            gr.DrawString(text, font, LGBrush, xAxis, yAxis);
            return temp;
        }
    }
}
