using System.Xml.Serialization;

namespace GrafikaKomputerowaDrawer
{
    public class PolygonGK : IObjectGK
    {
        [XmlIgnore]
        public bool completed = false;
        [XmlIgnore]
        public List<VectGK> vectors
        {
            get
            {
                List<VectGK> list = new List<VectGK>();
                for (int i = 0; i < points.Count; i++)
                {
                    if (!(i + 1 < points.Count || completed)) break;
                    PointGK p1 = points[i];
                    PointGK p2 = points[(i + 1) % points.Count];
                    VectGK vect = new VectGK(p1, p2, true);
                    vect.IsSelected = p1.IsSelected && p2.IsSelected;
                    list.Add(vect);
                }
                return list;
            }
        }

        public List<PointGK> points;
        public int N => points.Count;
        public void Add(PointGK p, int index = -1)
        {
            if (points.Count > 0 && p == points[0])
            {
                completed = true;
                return;
            }
            if (index != -1)
            {
                points.Insert(index, p);
            }
            else
            {
                points.Add(p);
            }
            p.partOf.Add(this);
            

        }
        public void Remove(PointGK p)
        {
            points.Remove(p);
            p.partOf.Remove(this);
        }
        public PolygonGK(List<PointGK> points)
        {
            this.points = points;
        }
        public PolygonGK()
        {
            this.points = new();
        }

        private bool isSelected;
        public bool IsSelected { get => isSelected; set => isSelected = value; }

        public ObjectTypeGK Type => ObjectTypeGK.Poly;

        public bool CheckClick(MouseEventArgs e)
        {
            bool clicked = IsInPolygon(new PointGK(e.X, e.Y));
            foreach (PointGK p in points)
                p.IsSelected = clicked;
            this.isSelected = clicked;
            return clicked;
        }

        public void Draw(PaintEventArgs e, DrawingGK drawing = DrawingGK.Default)
        {
            //if(vectors.Count > 0)
            //    e.Graphics.FillPolygon(Parameters.Brush, points.Select(p => new PointF(p.X, p.Y)).ToArray());
            foreach (VectGK v in vectors)
            {
                v.Draw(e, drawing);
            }
            //foreach(PointGK p in points)
            //    p.Draw(e, drawing);


            if(this.completed)
            {
                if(!this.isSelected)
                    e.Graphics.FillPolygon(Parameters.PolygonBrush, this.points.Select(p => (Point)p).ToArray());
                else
                    e.Graphics.FillPolygon(Parameters.SelectedPolygonBrush, this.points.Select(p => (Point)p).ToArray());
            }
            if (false)
            {
                List<VectGK> offsetEdges = new List<VectGK>();
                int offset = 10;
                if (points.Count > 2)
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        PointGK p1 = points[i];
                        PointGK p2 = points[(i + 1) % points.Count];
                        (double outNX, double outNY) = getOutNormal(p1, p2);
                        int offsetX = (int)(outNX * offset);
                        int offsetY = (int)(outNY * offset);
                        //int max = Math.Max(Math.Abs(offsetX), Math.Abs(offsetY));
                        PointGK mp = new PointGK(((p1.X + p2.X) / 2) + (offsetX  * offset / 2), ((p1.Y + p2.Y) / 2) + (offsetY * offset / 2));
                        mp.Draw(e);
                        if (IsInPolygon(new PointGK(((p1.X + p2.X) / 2) + (offsetX / 10), ((p1.Y + p2.Y) / 2) + (offsetY / 10))))
                        {
                            offsetX *= -1;
                            offsetY *= -1;
                        }
                        p1 = new PointGK(p1.X + offsetX, p1.Y + offsetY);
                        p2 = new PointGK(p2.X + offsetX, p2.Y + offsetY);

                        VectGK offsetEdge = new VectGK(p1, p2);
                        offsetEdge.Draw(e);
                        offsetEdges.Add(offsetEdge);
                    }
                    for (int i = 0; i < offsetEdges.Count; i++)
                    {
                        VectGK e1 = offsetEdges[i];
                        VectGK e2 = offsetEdges[(i + 1) % offsetEdges.Count];

                        PointGK p = GetLinesIntersect(e1, e2);
                        if (p != null && this.DistanceTo(p) >= offset)
                        {
                            // && !IsInPolygon(p)
                            p.IsSelected = true;
                            p.Draw(e);
                        }
                    }
                }
            }
        }
        public PointGK[] GetPoints()
        {
            return points.ToArray();
        }
        public void Offset(int x, int y)
        {
            foreach (PointGK p in points)
                p.Offset(x, y);
        }
        public PolygonGK DrawOffsetPoly(PaintEventArgs e, int offset)
        {
            List<VectGK> offsetEdges = new List<VectGK>();
            List<PointGK> offsetPoints = new List<PointGK>();

            for (int i = 0; i < points.Count; i++)
            {
                PointGK p1 = points[i];
                PointGK p2 = points[(i + 1) % points.Count];
                (double outNX, double outNY) = getOutNormal(p1, p2);
                int offsetX = (int)(outNX * offset);
                int offsetY = (int)(outNY * offset);
                // mp.Draw(e);
                if (IsInPolygon(new PointGK((p1.X + p2.X) / 2 + Math.Sign(offsetX), (p1.Y + p2.Y) / 2 + Math.Sign(offsetY) )))
                {
                    offsetX *= -1;
                    offsetY *= -1;
                }
                p1 = new PointGK(p1.X + offsetX, p1.Y + offsetY);
                p2 = new PointGK(p2.X + offsetX, p2.Y + offsetY);

                VectGK offsetEdge = new VectGK(p1, p2);
                // offsetEdge.Draw(e);
                offsetEdges.Add(offsetEdge);
            }
            for (int i = 0; i < offsetEdges.Count; i++)
            {
                VectGK e1 = offsetEdges[i];
                VectGK e2 = offsetEdges[(i + 1) % offsetEdges.Count];

                PointGK p = GetLinesIntersect(e1, e2);
                if (p != null && this.DistanceTo(p) >= offset - Math.Sqrt(2) - 1)
                {
                    // this.DistanceTo(p) >= offset - Math.Sqrt(2) - 1
                    offsetPoints.Add(p);
                    //p.IsSelected = true;
                    //p.Draw(e);
                }
            }
            //points.ForEach(p => p.IsSelected = false);
            //var parts = PolygonGK.GetOutterBound(points, offset);
            //foreach(var p in parts.Item2)
            //{
            //    p.IsSelected = true;
            //}
            PolygonGK offsetPoly = new PolygonGK(offsetPoints)  ;
            offsetPoly.completed = true;
            offsetPoly.Draw(e);
            return offsetPoly;
        }

        //https://stackoverflow.com/questions/4243042/c-sharp-point-in-polygon
        public bool IsInPolygon(PointGK p)
        {
            Point p1, p2;
            bool inside = false;

            if (points.Count < 3)
            {
                return inside;
            }

            Point oldPoint = new Point(
                points[^1].X, points[points.Count - 1].Y);

            for (int i = 0; i < points.Count; i++)
            {
                Point newPoint = new Point(points[i].X, points[i].Y);

                if (newPoint.X > oldPoint.X)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }

                if ((newPoint.X < p.X) == (p.X <= oldPoint.X)
                    && (p.Y - (long)p1.Y) * (p2.X - p1.X)
                    < (p2.Y - (long)p1.Y) * (p.X - p1.X))
                {
                    inside = !inside;
                }

                oldPoint = newPoint;
            }

            return inside;
        }
        private static int Cross(PointGK o, PointGK a, PointGK b)
        {
            double value = ((a.X - o.X) * (b.Y - o.Y)) - ((a.Y - o.Y) * (b.X - o.X));
            return Math.Abs(value) < 1e-10 ? 0 : value < 0 ? -1 : 1;
        }
        private static PointGK GetLinesIntersect(VectGK v1, VectGK v2)
        {
            int X = (int)(((v1.B * v2.C) - (v2.B * v1.C)) / ((v1.A * v2.B) - (v2.A * v1.B)));
            int Y = (int)(((v1.C * v2.A) - (v2.C * v1.A)) / ((v1.A * v2.B) - (v2.A * v1.B)));
            if (double.IsFinite(X) && double.IsFinite(Y))
                return new PointGK(X, Y);
            else
                return null;
        }
        private static (double, double) getOutNormal(PointGK p1, PointGK p2)
        {
            int dx = p2.X - p1.X;
            int dy = p2.Y - p1.Y;
            double edgeLength = Math.Sqrt((dx * dx) + (dy * dy));
            double X = -dy / edgeLength;
            double Y = dx / edgeLength;
            return (X, Y);
        }
        public void AddConstraint(ConstraintGK constraint)
        {
            foreach(var p in points)
            {
                if(constraint.points.Contains(p))
                    p.AddConstraint(constraint);
            }
        }
        public double DistanceTo(PointGK p)
        {
            if (IsInPolygon(p)) return 0;
            double dist = double.MaxValue;
            foreach(var edge in vectors)
                dist = Math.Min(edge.DistanceTo(p), dist);
            return dist;

        }
        private static (List<PointGK>, List<PointGK>) GetOutterBound(List<PointGK> points, int offset)
        {
            List<PointGK> part1 = new List<PointGK>();
            List<PointGK> part2 = new List<PointGK>();

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    if (Math.Abs(i - j) > 1 && points[i].Distance(points[j]) <= 2 * offset)
                    {
                        part1 = points.GetRange(i, Math.Abs(j - i));
                        part2 = points.FindAll(p => !part1.Contains(p));
                    }
                }
            }

            return (part1, part2);
        }
        private List<PointGK> GetCloseOffset(List<PointGK> points, int offset) 
        { 
            PolygonGK polygon = new PolygonGK();     
            var sortedPoints = points.OrderBy(p => DistanceTo(p)).Where(p => this.DistanceTo(p) >= offset - Math.Sqrt(2) - 1).ToList();
            if (sortedPoints.Count < 3) return null;

            for (int i= 0; i < sortedPoints.Count; i++)
            {
                if(i < 3) polygon.Add(sortedPoints[i]);
                if (this.points.TrueForAll(p => polygon.IsInPolygon(p)))
                    break;
                else
                    polygon.Add(sortedPoints[i]);
            }

            if (polygon.N < 3) 
                return null;

            return SortAround(polygon.points);
        }

        private static List<PointGK> SortAround(List<PointGK> points) 
        { 
            int meanX = points.Sum(p => p.X)/ points.Count;
            int meanY = points.Sum(p => p.Y)/ points.Count;
            return points.OrderBy(p => Math.Atan2(p.Y-meanY, p.X - meanX)).ToList();

        }
    }
}
