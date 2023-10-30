namespace GrafikaKomputerowaDrawer
{
    public class CircleGK : IObjectGK
    {
        public PointGK center;
        public int radius;
        public ObjectTypeGK Type => ObjectTypeGK.Circle;
        private bool isSelected = false;

        public CircleGK(PointGK center, int radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public bool IsSelected { get => isSelected; set => isSelected = value; }

        public void AddConstraint(ConstraintGK constraint)
        {
            throw new NotImplementedException();
        }

        public bool CheckClick(MouseEventArgs e)
        {
            center.IsSelected = center.Distance(e.X, e.Y) <= radius;
            isSelected = center.Distance(e.X, e.Y) <= radius;
            return center.Distance(e.X, e.Y) <= radius;
        }

        public void Draw(PaintEventArgs e, DrawingGK drawing = DrawingGK.Default)
        {
            center.Draw(e, drawing);

            int x = radius;
            int y = 0;
            if (D(radius, y - 1) > D(radius, y))
                x--;

            double T = 0;
            double I = 255;
            putPixel(x, y, I);

            while (x > y)
            {
                y++;
                if (D(radius, y) < T)
                    x--;

                putPixel(x, y, I * (1 - D(radius, y)));
                putPixel(x - 1, y, I * D(radius, y));

                putPixel(y, x, I * (1 - D(radius, y)));
                putPixel(y, x-1, I * D(radius, y));

                putPixel(-x, -y, I * (1 - D(radius, y)));
                putPixel(-x + 1, -y, I * D(radius, y));

                putPixel(-y, -x, I * (1 - D(radius, y)));
                putPixel(-y, -x+1, I * D(radius, y));

                putPixel(x, -y, I * (1 - D(radius, y)));
                putPixel(x - 1, -y, I * D(radius, y));

                putPixel(-y, x, I * (1 - D(radius, y)));
                putPixel(-y, x-1, I * D(radius, y));

                putPixel(-x, y, I * (1 - D(radius, y)));
                putPixel(-x + 1, y, I * D(radius, y));

                putPixel(y, -x, I * (1 - D(radius, y)));
                putPixel(y,-x + 1, I * D(radius, y));

                T = D(radius, y);

            }

            putPixel(radius, 0, 255);
            putPixel(-radius, 0, 255);
            putPixel(0, radius, 255);
            putPixel(0, -radius, 255);


            double D(int a, int b)
            {
                return Math.Ceiling(Math.Sqrt(a * a - b * b)) - Math.Sqrt(a * a - b * b);
            }
            void putPixel(int x, int y, double i)
            {
                int I = 255 - (int)i;
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(I, I, I)), center.X + x, center.Y+ y, 1, 1);
            }
        }

        public PointGK[] GetPoints()
        {
            return new PointGK[1] { center };
        }

        public void Offset(int x, int y)
        {
            center.Offset(x, y);
        }
    }
}
