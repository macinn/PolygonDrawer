using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaKomputerowaDrawer
{
    public partial class GKDrawer : Form
    {
        private void Canvas_Click(object sender, EventArgs e)
        {

            switch (windowState)
            {
                case DrawerState.LineState:
                    if (!IsDrawing)
                    {
                        IsDrawing = true;
                        mouseDownPoint = GetNewPoint((MouseEventArgs)e);
                    }
                    else if (IsDrawing)
                    {
                        IsDrawing = false;
                        PointGK endPoint = GetNewPoint((MouseEventArgs)e);
                        GetNewVect(mouseDownPoint, endPoint);
                        ClearTemp();
                    }
                    Canvas.Invalidate();
                    break;
                case DrawerState.PointState:
                    PointGK point = GetNewPoint((MouseEventArgs)e);
                    if(!AddPointToObject(point, (MouseEventArgs)e))
                    {
                        objects.Add(point);
                    }
                    Canvas.Invalidate();
                    break;
                case DrawerState.SelectState:
                    if (CtrlPressed)
                        foreach (var p in points)
                        {
                            if (!(ShiftPressed && p.IsSelected))
                                p.CheckClick((MouseEventArgs)e);
                        }
                    else
                    {
                        HashSet<IObjectGK> objectsToBeSelected = new HashSet<IObjectGK>();
                        foreach (var obj in objects)
                        {
                            if (!(ShiftPressed && obj.IsSelected))
                            {
                                bool saveClick = obj.CheckClick((MouseEventArgs)e);
                                if (saveClick)
                                {
                                    objectsToBeSelected.Add(obj);
                                }
                                if (obj.Type == ObjectTypeGK.Poly)
                                {
                                    PolygonGK poly = (PolygonGK)obj;
                                    bool AnyEdgeClicked = false;
                                    var vectors = poly.vectors;
                                    foreach (var vector in vectors)
                                    {
                                        AnyEdgeClicked |= vector.CheckClick((MouseEventArgs)e);
                                        if (AnyEdgeClicked) break;
                                    }


                                    if (!AnyEdgeClicked) poly.IsSelected = saveClick;
                                    else objectsToBeSelected.Remove(obj);
                                }
                            }
                        }
                        foreach (var obj in objectsToBeSelected) obj.CheckClick((MouseEventArgs)e);
                    }
                    UpdatePointData();
                    Canvas.Invalidate();
                    break;
                case DrawerState.PolygonState:
                    if(!IsDrawing)
                    {
                        polyStart = GetNewPoint((MouseEventArgs)e);
                        polyStart.IsSelected = true;
                        currentPoly = new PolygonGK();
                        currentPoly.Add(polyStart);
                        objects.Add(currentPoly);
                        mouseDownPoint = polyStart;
                        IsDrawing = true;
                    }
                    else
                    {
                        PointGK newPoint = GetNewPoint((MouseEventArgs)e);
                        currentPoly.Add(newPoint);
                        if(newPoint == polyStart)
                        {
                            polyStart.IsSelected = false;
                            IsDrawing = false;
                            polyStart = null;
                            currentPoly.completed = true;
                            objects.Remove(temporaryVect);
                            temporaryVect = null;
                        }
                        else 
                            mouseDownPoint = newPoint;
                    }
                    Canvas.Invalidate();
                    break;
            }
        }
        private void UpdatePointData()
        {
            var data = points.ToList().FindAll(p => p.IsSelected).Select(p => $"({p.X}, {p.Y})").ToList();
            PointTextBox.Text = String.Join(", ", data);

        }
        private bool AddPointToObject(PointGK point, MouseEventArgs e)
        {
            var copyObject = objects.FindAll(obj => obj.Type == ObjectTypeGK.Vect || obj.Type == ObjectTypeGK.Poly);
            foreach (var obj in objects)
            {
                if(obj.Type == ObjectTypeGK.Vect) 
                { 
                    VectGK v = (VectGK)obj;
                    bool czyTrafiony = v.CheckClick(e);
                    if (czyTrafiony)
                    {
                        objects.Remove(obj);
                        objects.Add(new VectGK(v.p1, point));
                        objects.Add(new VectGK(point, v.p2));
                        v.p1.RemoveConstraint();
                        v.p2.RemoveConstraint();
                        horizontalConstPoints.Remove(v.p1);
                        horizontalConstPoints.Remove(v.p2);
                        verticalConstPoints.Remove(v.p1);
                        verticalConstPoints.Remove(v.p2);
                        return true;
                    }
                }
                else if (obj.Type == ObjectTypeGK.Poly)
                {
                    PolygonGK poly = (PolygonGK)obj;
                    var points = poly.GetPoints();
                    for (int i=0; i<poly.N; i++)
                    {
                        var p0 = points[i];
                        var p1 = points[(i+1)% poly.N];
                        if(new VectGK(p0, p1).CheckClick(e))
                        {
                            poly.Add(point, i + 1);
                            p0.RemoveConstraint();
                            p1.RemoveConstraint();
                            horizontalConstPoints.Remove(p0);
                            horizontalConstPoints.Remove(p1);
                            verticalConstPoints.Remove(p0);
                            verticalConstPoints.Remove(p1);
                            return true;
                        }
                    }
                }

            }
            return false;
        }
        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            switch (windowState)
            {
                case DrawerState.SelectState:
                    mouseDownPoint = new PointGK(e.X, e.Y);
                    break;
            }
        }
        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            switch (windowState)
            {
                case DrawerState.SelectState:
                    mouseDownPoint = null;
                    break;
            }
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDownPoint == null) return;
            PointGK curPoint = new PointGK(e.X, e.Y);
            switch (windowState)
            {
                case DrawerState.LineState:
                case DrawerState.PolygonState:
                    if (IsDrawing)
                    {
                        if (temporaryVect == null)
                        {
                            temporaryVect = new VectGK(mouseDownPoint, curPoint);
                            objects.Add(temporaryVect);
                        }
                        else
                        {
                            PointGK p1 = mouseDownPoint;
                            PointGK p2 = curPoint;
                            temporaryVect.p1 = p1;
                            temporaryVect.p2 = p2;
                        }
                        Canvas.Invalidate();
                    }
                    break;
                case DrawerState.SelectState when !LockBox.Checked:
                    (int x, int y) = (curPoint.X - mouseDownPoint.X, curPoint.Y - mouseDownPoint.Y);
                    mouseDownPoint = curPoint;
                    bool anyMoved = false;
                    foreach (var p in points)
                    {
                        if (p.IsSelected)
                        {
                            p.Offset(x, y);
                            anyMoved = true;
                        }
                    }
                    if (anyMoved) Canvas.Invalidate();
                    break;
            }
        }
        private void Canvas_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}
