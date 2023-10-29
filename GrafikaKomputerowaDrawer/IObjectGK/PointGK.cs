using System.Xml.Serialization;

namespace GrafikaKomputerowaDrawer
{
    public class PointGK : IObjectGK
    {
        public int X, Y;
        [XmlIgnore]
        public HashSet<IObjectGK> partOf;
        [XmlIgnore]
        public ConstraintGK constraint;
        [XmlIgnore]
        public ConstraintType? constraintType {
        get{
                if (this.constraint == null) return null;
                else return constraint.type;
                }
            }
        private bool isSelected;
        [XmlIgnore]
        public bool IsSelected
        {
            get => isSelected;
            set => isSelected = value;
        }

        public ObjectTypeGK Type => ObjectTypeGK.Point;

        public double Distance(PointGK p)
        {
            return Distance(p.X, p.Y);
        }

        public double Distance(int X, int Y)
        {
            return Math.Sqrt((X - this.X) * (X - this.X) + (Y - this.Y) * (Y - this.Y));
        }

        public PointGK(int x, int y)
        {
            X = x;
            Y = y;
            partOf = new HashSet<IObjectGK>();
        }

        public PointGK()
        {
            partOf = new HashSet<IObjectGK>();
        }

        public bool CheckClick(MouseEventArgs e)
        {
            isSelected = this.Distance(e.X, e.Y) < Parameters.ClickTolerance;
            return isSelected;
        }

        public void Draw(PaintEventArgs e, DrawingGK drawing = DrawingGK.Default)
        {
            try
            {

                if (!isSelected) e.Graphics.FillRectangle(Parameters.Brush, X - (Parameters.PointSize / 2), Y - (Parameters.PointSize / 2), Parameters.PointSize, Parameters.PointSize);
                else
                    e.Graphics.FillRectangle(Parameters.SelectedBrush, X - (Parameters.SelectedPointSize / 2), Y - (Parameters.SelectedPointSize / 2), Parameters.SelectedPointSize, Parameters.SelectedPointSize);
            }
            catch
            {
                // przecięcie poza limitem inta
            }
        }

        public void Offset(int x, int y)
        {
            bool allSelected = false;
            if (constraint != null)
            {
                allSelected = constraint.points.TrueForAll(p => p.isSelected);

                if (allSelected || constraint.type != ConstraintType.Horizontal)
                    this.Y += y;
                if (allSelected || constraint.type != ConstraintType.Vertical)
                    this.X += x;
            }
            else
            {
                    this.Y += y;
                    this.X += x;
            }
        }

        public PointGK[] GetPoints()
        {
            return new PointGK[1] { this };
        }

        public void AddConstraint(ConstraintGK constraint)
        {
            this.constraint = constraint;
        }

        public void RemoveConstraint()
        {
            this.constraint = null;
        }

        public static implicit operator Point(PointGK p)
        {
            return new Point(p.X, p.Y);
        }

    }
}
