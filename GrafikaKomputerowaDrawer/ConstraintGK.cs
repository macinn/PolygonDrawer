using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaKomputerowaDrawer
{
    public enum ConstraintType
    {
        Vertical = 'V',
        Horizontal = 'H'
    }

    public class ConstraintGK
    {
        public ConstraintType type;
        public List<PointGK> points;
        public ConstraintGK(ConstraintType type, List<PointGK> points, params (int X, int Y)[] iconLocations)
        {
            this.type = type;
            this.points = points;
        }

        public ConstraintGK()
        {
        }

        // https://learn.microsoft.com/en-us/dotnet/desktop/winforms/advanced/how-to-draw-text-on-a-windows-form?view=netframeworkdesktop-4.8
        public static void DrawString(PaintEventArgs e, int x, int y, string text)
        {
            System.Drawing.Graphics formGraphics = e.Graphics;
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.DarkBlue);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            formGraphics.DrawString(text, drawFont, drawBrush, x, y, drawFormat);
            drawFont.Dispose();
            drawBrush.Dispose();
        }

        public void Draw(PaintEventArgs e)
        {
            int X = (points[0].X + points[1].X )/2;
            int Y = (points[0].Y + points[1].Y )/2;
            if(type == ConstraintType.Vertical)
                DrawString(e, X, Y, "V");
            else
                DrawString(e, X, Y, "H");
        }
    }
}
