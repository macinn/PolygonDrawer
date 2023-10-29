namespace GrafikaKomputerowaDrawer
{
    public partial class GKDrawer : Form
    {
        private void GKDrawer_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    ClearTemp();
                    List<PointGK> selectedPoints = this.points.ToList().FindAll(p => p.IsSelected);

                    foreach (PointGK? p in selectedPoints)
                    {
                        foreach (IObjectGK obj in p.partOf)
                        {
                            switch (obj.Type)
                            {
                                case ObjectTypeGK.Vect:
                                    objects.Remove(obj);
                                    points.Remove(((VectGK)obj).p1);
                                    points.Remove(((VectGK)obj).p2);
                                    break;
                                case ObjectTypeGK.Poly:
                                    PolygonGK? poly = obj as PolygonGK;
                                    var pointsPoly = poly.GetPoints();
                                    int index = pointsPoly.ToList().IndexOf(p);
                                    var pL = pointsPoly[(index + poly.N - 1) % poly.N];
                                    var pR = pointsPoly[(index + poly.N + 1) % poly.N];
                                    if(p.constraintType != null)
                                    {
                                        if (pL.constraintType == p.constraintType)
                                            pL.RemoveConstraint();
                                        if(pR.constraintType == p.constraintType)
                                            pR.RemoveConstraint();
                                    }
                                    poly.Remove(p);
                                    if (poly.N < 3)
                                    {
                                        objects.Remove(poly);
                                        foreach (PointGK obj2 in poly.GetPoints())
                                            points.Remove(obj2);
                                    }
                                    break;
                            }
                        }
                        objects.Remove(p);
                        points.Remove(p);
                        p.RemoveConstraint();
                        verticalConstPoints.Remove(p);
                        horizontalConstPoints.Remove(p);
                    }

                    if (selectedPoints.Count > 0)
                        Canvas.Invalidate();
                    break;
                case Keys.ShiftKey:
                    ShiftPressed = true;
                    break;
                case Keys.ControlKey:
                    CtrlPressed = true;
                    break;
                //case Keys.Q:
                //    if (windowState == DrawerState.SelectState)
                //    {
                //        List<IObjectGK> splitable = objects.Where(
                //            o => o.Type==ObjectTypeGK.Vect || o.Type == ObjectTypeGK.Poly).ToList();
                //        foreach (IObjectGK? o in splitable)
                //        {
                //            if (o.Type == ObjectTypeGK.Vect && o.IsSelected)
                //            {
                //                PointGK[] points = o.GetPoints();
                //                if (points.Length == 2)
                //                {
                //                    objects.Remove(o);
                //                    PointGK newPoint = new PointGK((points[0].X + points[1].X) / 2, (points[0].Y + points[1].Y) / 2);
                //                    VectGK v = GetNewVect(points[0], newPoint);
                //                    v.IsSelected = true;
                //                    v = GetNewVect(newPoint, points[1]);
                //                    v.IsSelected = true;
                //                    this.points.Add(newPoint);
                //                }
                //            }
                //            else if(o.Type == ObjectTypeGK.Poly)
                //            {
                //                var poly = o as PolygonGK;
                //                var vectors2split = poly.vectors.Where(v=> v.IsSelected).ToList();
                //                foreach(VectGK v in  vectors2split)
                //                {
                //                    PointGK newPoint = new PointGK((v.p1.X + v.p2.X) / 2, (v.p1.Y + v.p2.Y) / 2);
                //                    int ind1 = poly.GetPoints().ToList().FindIndex(0,p => p == v.p1);
                //                    int ind2 = poly.GetPoints().ToList().FindIndex(0,p => p == v.p2);
                //                    poly.Add(newPoint,Math.Min(ind1, ind2)+1);
                //                    this.points.Add(newPoint);
                //                }

                //            }
                //        }
                //        Canvas.Invalidate();
                //    }
                //    break;

            }
            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        foreach (IObjectGK obj in objects) obj.IsSelected = true;
                        Canvas.Invalidate();
                        break;

                }
            }
        }
        private void GKDrawer_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    ShiftPressed = false;
                    break;
                case Keys.ControlKey:
                    CtrlPressed = false;
                    break;

            }
        }
    }
}
