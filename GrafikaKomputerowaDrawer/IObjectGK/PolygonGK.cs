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
            //new PolygonGK(getPointsWithMinimumDist(20)).vectors.ForEach(v => v.Draw(e, drawing));
            //foreach(PointGK p in points)
            //    p.Draw(e, drawing);


            if(this.completed)
            {
                if(!this.isSelected)
                    e.Graphics.FillPolygon(Parameters.PolygonBrush, this.points.Select(p => (Point)p).ToArray());
                else
                    e.Graphics.FillPolygon(Parameters.SelectedPolygonBrush, this.points.Select(p => (Point)p).ToArray());
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
        public List<PolygonGK> DrawOffsetPoly(PaintEventArgs e, DrawingGK drawing, int offset)
        {
            List<List<PointGK>> lists = hullOffset(this.points, offset);
            List<PolygonGK> offsetPolygons = new List<PolygonGK>();

            
            PolygonGK offsetPoly = GetOffsetPolyFromPoints(lists[0], offset);
            offsetPoly.vectors.ForEach(v => v.Draw(e, drawing));
            offsetPolygons.Add(offsetPoly);

            for (int k = 1; k < lists.Count; k++)
            {
                //lists[k].ForEach(p => { p.IsSelected = true; p.Draw(e, drawing); });
                PolygonGK innerOffsetPoly = GetInnerOffsetPoly(lists[k], offset);
                innerOffsetPoly.points.ForEach(v => v.Draw(e, drawing));
                innerOffsetPoly.vectors.ForEach(v => v.Draw(e, drawing));
                //offsetPolygons.Add(innerOffsetPoly);
            }
            
            return offsetPolygons;
        }

        private PolygonGK GetOffsetPolyFromPoints(List<PointGK> points, int offset)
        {
            List<VectGK> offsetEdges = new List<VectGK>();
            List<PointGK> offsetPoints = new List<PointGK>();
            PolygonGK hullPoly = new PolygonGK(points)
            { completed = true };

            for (int i = 0; i < points.Count; i++)
            {
                PointGK p1 = points[i];
                PointGK p2 = points[(i + 1) % points.Count];
                (double outNX, double outNY) = getOutNormal(p1, p2);
                int offsetX = (int)(outNX * offset);
                int offsetY = (int)(outNY * offset);
                if (hullPoly.IsInPolygon(new PointGK((p1.X + p2.X) / 2 + Math.Sign(offsetX), (p1.Y + p2.Y) / 2 + Math.Sign(offsetY))))
                {
                    offsetX *= -1;
                    offsetY *= -1;
                }
                p1 = new PointGK(p1.X + offsetX, p1.Y + offsetY);
                p2 = new PointGK(p2.X + offsetX, p2.Y + offsetY);

                VectGK offsetEdge = new VectGK(p1, p2);
                offsetEdges.Add(offsetEdge);
            }
            for (int i = 0; i < offsetEdges.Count; i++)
            {
                VectGK e1 = offsetEdges[i];
                VectGK e2 = offsetEdges[(i + 1) % offsetEdges.Count];

                PointGK p = GetLinesIntersect(e1, e2);
                if (p != null && this.DistanceTo(p) >= offset - Math.Sqrt(2) - 1)
                {
                    offsetPoints.Add(p);
                }
            }
            PolygonGK offsetPoly = new PolygonGK(offsetPoints);
            offsetPoly.completed = true;
            return offsetPoly;
        }
        private PolygonGK GetInnerOffsetPoly(List<PointGK> points, int offset)
        {
            //int minX = this.points.Min(p => p.X);
            //int minY = this.points.Min(p => p.Y);
            //int maxX = this.points.Max(p => p.X);
            //int maxY = this.points.Max(p => p.Y);

            PolygonGK innerPoly = new PolygonGK(points);
            innerPoly.completed = true;
            List<VectGK> offsetEdges = new List<VectGK>();
            List<PointGK> offsetPoints = new List<PointGK>();

            for (int i = 0; i < points.Count - 1; i++)
            {
                PointGK p1 = points[i];
                PointGK p2 = points[(i + 1) % points.Count];
                (double outNX, double outNY) = getOutNormal(p1, p2);
                int offsetX = (int)(outNX * offset);
                int offsetY = (int)(outNY * offset);
                if (this.IsInPolygon(new PointGK((p1.X + p2.X) / 2 + Math.Sign(offsetX), (p1.Y + p2.Y) / 2 + Math.Sign(offsetY))))
                {
                    offsetX *= -1;
                    offsetY *= -1;
                }
                p1 = new PointGK(p1.X + offsetX, p1.Y + offsetY);
                p2 = new PointGK(p2.X + offsetX, p2.Y + offsetY);

                VectGK offsetEdge = new VectGK(p1, p2);
                offsetEdges.Add(offsetEdge);
            }
            for (int i = 0; i < offsetEdges.Count; i++)
            {
                VectGK e1 = offsetEdges[i];
                VectGK e2 = offsetEdges[(i + 1) % offsetEdges.Count];

                PointGK p = GetLinesIntersect(e1, e2);
                if (p != null && this.DistanceTo(p) >= offset - Math.Sqrt(2) - 1)
                {
                    //if(p.X > minX && p.Y > minY && p.X < maxX && p.Y < maxY)

                    if (innerPoly.IsInPolygon(p))
                        offsetPoints.Add(p);
                }
            }
            PolygonGK offsetPoly = new PolygonGK(offsetPoints);
            offsetPoly.completed = true;
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
        
        private List<List<PointGK>> hullOffset(List<PointGK> points, int offset)
        {
            var hull = PolygonGK.ConvexHull(points.ToArray()).ToList();
            HashSet<PointGK> okP = new HashSet<PointGK>(hull);

            List<List<PointGK>> outputLists = new List<List<PointGK>>();
            List<List<PointGK>> innerLists = new List<List<PointGK>>();

            for(int i=0; i<hull.Count; i++)
            {
                int ind0 = i;
                int ind1 = (i + 1) % hull.Count;
                ind0 = points.FindIndex(p => p == hull[ind0]);
                ind1 = points.FindIndex(p => p == hull[ind1]);
                bool goOtherDirection = false;
                if(Math.Abs(ind0 - ind1) != 1)
                    if(points[ind0].Distance(points[ind1]) >= 2 * offset)
                    {
                        int maxInd = Math.Max(ind0, ind1);
                        int minInd = Math.Min(ind0, ind1);
                        for(int k=minInd + 1; k<maxInd; k++)
                        {
                            if (hull.Contains(points[k]))
                            {
                                goOtherDirection = true;
                                break;
                            }
                            okP.Add(points[k]);
                        }
                        if(goOtherDirection)
                        {
                            for (int k = maxInd + 1; k != minInd; k++)
                            {
                                k %= points.Count;
                                if (k == minInd) break;
                                okP.Add(points[k]);
                            }
                        }
                    }
                    else
                    {
                        List<PointGK> innerPart = new List<PointGK>();
                        int maxInd = Math.Max(ind0, ind1);
                        int minInd = Math.Min(ind0, ind1);
                        for (int k = minInd + 1; k < maxInd; k++)
                        {
                            if (hull.Contains(points[k]))
                            {
                                goOtherDirection = true;
                                break;
                            }
                            innerPart.Add(points[k]);
                        }
                        if (goOtherDirection)
                        {
                            for (int k = maxInd + 1; k != minInd; k++)
                            {
                                k %= points.Count;
                                if (k == minInd) break;
                                innerPart.Add(points[k]);
                            }
                        }
                        if(innerPart.Count >= 3)
                        {
                            innerLists.Add(innerPart);
                        }
                    }
                
            }

            //foreach (var innerPoly in innerLists)
            //{
            //    var current = innerPoly;
            //    var trueInner = hullOffset(current, offset);
            //    while (trueInner[0].Count != current.Count)
            //    {
            //        current = trueInner[0];
            //        trueInner = hullOffset(current, offset);
            //    }
            //    outputLists.Add(trueInner[0]);
            //}
            outputLists.AddRange(innerLists);
            outputLists.Insert(0, this.points.FindAll(p => okP.Contains(p)));
            
            return outputLists;
        }

       
        private static PointGK[] ConvexHull(PointGK[] points)
        {
            if (points.Length == 1) return points;

            (double minX, double minY) = (points[0].X, points[0].Y);

            for (int i = 1; i < points.Length; i++)
            {
                if (points[i].Y > minY || points[i].Y == minY && points[i].X < minX)
                {
                    (minX, minY) = (points[i].X, points[i].Y);
                    (points[0], points[i]) = (points[i], points[0]);
                }
            }

            Array.Sort(points, (a, b) =>
            {
                int cros = -Cross(points[0], a, b);
                if (cros != 0) return cros;
                else if (a.X.CompareTo(b.X) != 0) return a.X.CompareTo(b.X);
                else return -a.Y.CompareTo(b.Y);
            });

            List<PointGK> lista = points.ToList();
            for (int i = 1; i < lista.Count - 1; i++)
            {
                if (Cross(lista[i - 1], lista[i], lista[i + 1]) == 0)
                {
                    lista.RemoveAt(i);
                    i--;
                }
            }

            List<PointGK> S = new List<PointGK>();
            S.Add(lista[0]);
            S.Add(lista[1]);
            for (int k = 2; k < lista.Count; k++)
            {
                while (S.Count >= 2 && Cross(S[S.Count - 2], S[S.Count - 1], lista[k]) != 1)
                {
                    S.RemoveAt(S.Count - 1);
                }
                S.Add(lista[k]);
            }

            return S.ToArray();
        }

    }
}
