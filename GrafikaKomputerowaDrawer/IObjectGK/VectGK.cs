using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace GrafikaKomputerowaDrawer
{
    public enum ObjectTypeGK
    {
        Vect,
        Point,
        Poly
    }
    public enum DrawingGK
    { 
        Default,
        Bres
    }
    public interface IObjectGK
    {
        ObjectTypeGK Type { get; }
        void Offset(int x, int y);
        bool IsSelected { get; set; }
        PointGK[] GetPoints();
        bool CheckClick(MouseEventArgs e);
        void Draw(PaintEventArgs e, DrawingGK drawing = DrawingGK.Default);
        void AddConstraint(ConstraintGK constraint);
    }
    public class VectGK : IObjectGK
    {
        public PointGK p1, p2; // p1.x <= p2.x
        [XmlIgnore]
        public double A { get => this.p2.Y - this.p1.Y; }
        [XmlIgnore]
        public double B { get => this.p1.X - this.p2.X; }
        [XmlIgnore]
        public double C { get => (-this.p1.Y * this.B) - (this.A * this.p1.X); }
        [XmlIgnore]
        public double Length { get => this.p1.Distance(this.p2); }


        private bool isSelected;
        public VectGK(PointGK p1, PointGK p2, bool isEdge = false)
        {
            if (p1.X < p2.X || (p1.X == p2.X && p1.Y <= p2.X))
            {
                this.p1 = p1;
                this.p2 = p2;
            }
            else
            {
                this.p1 = p2;
                this.p2 = p1;
            }
            if (!isEdge)
            {
                this.p1.partOf.Add(this);
                this.p2.partOf.Add(this);
            }
        }
        public VectGK(int X, int Y)
        {
            this.p1 = new PointGK(0, 0);
            this.p2 = new PointGK(X, Y);
        }

        public VectGK()
        {

        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                //p1.IsSelected = this.isSelected || p1.IsSelected;
                //p2.IsSelected = this.isSelected || p2.IsSelected;
            }
        }

        public ObjectTypeGK Type => ObjectTypeGK.Vect;

        public bool CheckClick(MouseEventArgs e)
        {
            double d1 = this.p1.Distance(e.X, e.Y);
            double d2 = this.p2.Distance(e.X, e.Y);
            if (Math.Max(d1, d2) > this.Length && Math.Min(d1, d2) > Parameters.ClickTolerance)
            {
                IsSelected = false;
                p1.IsSelected = false; p2.IsSelected = false;
                return false;
            }
            double d = Math.Abs((this.A * e.X) + (this.B * e.Y) + this.C) / Math.Sqrt((this.A * this.A) + (this.B * this.B));
            IsSelected = d < Parameters.ClickTolerance;
            p1.IsSelected = IsSelected; p2.IsSelected = IsSelected;
            return IsSelected;

        }

        public void Draw(PaintEventArgs e, DrawingGK drawing = DrawingGK.Default)
        {
            p1.Draw(e);
            p2.Draw(e);
            switch (drawing)
            {
                case DrawingGK.Default:
                    if (!this.isSelected)
                        e.Graphics.DrawLine(Parameters.Pen, p1, p2);                    
                    else
                        e.Graphics.DrawLine(Parameters.SelectedPen, p1, p2);                                       
                    break;
                case DrawingGK.Bres:
                    //line(p1.X, p1.Y, p2.X, p2.Y);
                    //https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
                    int dy = p2.Y - p1.Y;
                    int dx = p2.X - p1.X;
                    if (Math.Abs(dy) < Math.Abs(dx))
                    {
                        if (p1.X > p2.X)
                            plotLineLow(p2, p1);
                        else
                            plotLineLow(p1, p2);
                    }
                    else
                    {
                        if (p1.Y > p2.Y)
                            plotLineHigh(p2, p1);
                        else
                            plotLineHigh(p1, p2);
                    }
                    break;
            }

            void plotLineHigh(PointGK p1, PointGK p2)
            {
                int dx = p2.X - p1.X;
                int dy = p2.Y - p1.Y;
                int xi = 1;
                if(dx<0)
                {
                    xi = -1;
                    dx = -dx;
                }
                int D = 2 * dx - dy;
                int x = p1.X;

                int PointSize = 2;
                int SelectedPointSize = 4;
                //if(p1.Y <= p2.Y)
                    for (int y = p1.Y; y <=p2.Y; y++)
                    {
                        if (!isSelected) e.Graphics.FillRectangle(Parameters.Brush, x - (PointSize / 2), y - (PointSize / 2), PointSize, PointSize);
                        else
                            e.Graphics.FillRectangle(Parameters.SelectedBrush, x - (SelectedPointSize / 2), y - (SelectedPointSize / 2), SelectedPointSize, SelectedPointSize);

                        if (D>0)
                        {
                            x += xi;
                            D -= 2 * dy;
                        }
                        D += 2 * dx;
                    }
                //else
                //{
                //    for (int y = p1.Y; y >= p2.Y; y--)
                //    {
                //        if (!isSelected) e.Graphics.FillRectangle(Parameters.Brush, x - (PointSize / 2), y - (PointSize / 2), PointSize, PointSize);
                //        else
                //            e.Graphics.FillRectangle(Parameters.SelectedBrush, x - (SelectedPointSize / 2), y - (SelectedPointSize / 2), SelectedPointSize, SelectedPointSize);

                //        if (D > 0)
                //        {
                //            x += xi;
                //            D -= 2 * dy;
                //        }
                //        D += 2 * dx;
                //    }
                //}
            }
            void plotLineLow(PointGK p0, PointGK p1)
            {
                int dx = p1.X - p0.X;
                int dy = p1.Y - p0.Y;
                int yi = 1;
                if(dy<0)
                {
                    yi = -1;
                    dy = -dy;

                }
                int D = 2 * dy - dx;
                int y = p0.Y;

                int PointSize = 2;
                int SelectedPointSize = 4;
                //if(p0.X<=p1.X)
                    for (int x = p0.X; x <=p1.X; x++)
                    {
                        if (!isSelected) e.Graphics.FillRectangle(Parameters.Brush, x - (PointSize / 2), y - (PointSize / 2), PointSize, PointSize);
                        else
                            e.Graphics.FillRectangle(Parameters.SelectedBrush, x - (SelectedPointSize / 2), y - (SelectedPointSize / 2), SelectedPointSize, SelectedPointSize);

                        if (D >= 0)
                        {
                            y += yi;
                            D -=  2 * dx;
                        }
                        D += 2 * dy;
                    }
                //else
                //    for (int x = p0.X; x >= p1.X; x--)
                //    {
                //        if (!isSelected) e.Graphics.FillRectangle(Parameters.Brush, x - (PointSize / 2), y - (PointSize / 2), PointSize, PointSize);
                //        else
                //            e.Graphics.FillRectangle(Parameters.SelectedBrush, x - (SelectedPointSize / 2), y - (SelectedPointSize / 2), SelectedPointSize, SelectedPointSize);

                //        if (D > 0)
                //        {
                //            y += yi;
                //            D -= 2 * dx;
                //        }
                //        D += 2 * dy;
                //    }

            
            }
        }
           
        public PointGK[] GetPoints()
        {
            return new PointGK[2] { p1, p2 };
        }

        public void Offset(int x, int y)
        {
            p1.Offset(x, y);
            p2.Offset(x, y);
        }

        public void AddConstraint(ConstraintGK constraint)
        {
            if(constraint.points.Contains(p1))
                p1.AddConstraint(constraint);
        }

        public double DistanceTo(PointGK p)
        {        
            double l12 = Length;
            double l13 = p1.Distance(p);
            double l23 = p2.Distance(p);
            double cos1 = (l13 * l13 - l23 * l23 - l12 * l12) / (-2 * l12 * l23);
            double cos2 = (l23 * l23 - l12 * l12 - l13 * l13) / (-2 * l12 * l13);
            if (cos1 > 0 && cos2 > 0)
            {
                // dist is between point nad vect
                return Math.Abs(A*p.X+B*p.Y+C)/Math.Sqrt(A*A+B*B);
            }
            else
            {
                // dist i s between point and vect's end
                return Math.Min(l13, l23);
            }
        }
    }
    static class Parameters
    {
        public static int ClickTolerance = 10;
        public static Pen Pen = new Pen(Color.Black, 2);
        public static Pen SelectedPen = new Pen(Color.Red, 2);
        public static Brush Brush = new SolidBrush(Color.Black);
        public static Brush SelectedBrush = new SolidBrush(Color.Red);
        public static int PointSize = 5;
        public static int SelectedPointSize = 7;

        public static Brush PolygonBrush = new SolidBrush(Color.FromArgb(20, 255, 0, 0));
        public static Brush SelectedPolygonBrush = new SolidBrush(Color.FromArgb(100, 255, 0, 0));
    }
}
