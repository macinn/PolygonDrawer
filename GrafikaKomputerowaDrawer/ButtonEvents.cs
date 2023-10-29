using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaKomputerowaDrawer
{
    public partial class GKDrawer : Form
    {
        private void ConstraintButton_Click(object sender, EventArgs e)
        {
            ConstraintErrorProvider.SetError(ConstraintButton, null);
            var selectedPoints = this.points.Where(p => p.IsSelected).ToList();
            bool pointsCorrect = false;
            if (selectedPoints.Count == 2)
            {
                var p0 = selectedPoints[0];
                var p1 = selectedPoints[1];
                if (p0.constraintType != null && p0.constraintType == p1.constraintType)
                {
                    ConstraintButton.Enabled = false;
                    RemoveConstraintButton.Enabled = true;
                    VerticalConst1.Enabled = false;
                    HorizontalConst1.Enabled = false;
                    if (p0.constraintType == ConstraintType.Vertical)
                        VerticalConst1.Checked = true;
                    else
                        HorizontalConst1.Checked = true;
                    return;
                }
                foreach (var obj in p0.partOf)
                {
                    if (p1.partOf.Contains(obj))
                    {
                        // sa czescia tego samego obiektu
                        if (obj.Type == ObjectTypeGK.Vect)
                        {
                            pointsCorrect = true;
                            break;
                        }
                        if (obj.Type == ObjectTypeGK.Poly)
                        {
                            var poly = obj as PolygonGK;
                            int ind0 = -1, ind1 = -1;

                            ind0 = poly.points.FindIndex(p => p==p0);
                            ind1 = poly.points.FindIndex(p => p == p1);
                            if (Math.Abs(ind0 - ind1) == 1 || Math.Abs(ind0 - ind1) ==poly.N-1)
                            {
                                pointsCorrect = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (pointsCorrect)
            {
                ConstraintErrorProvider.SetError(ConstraintButton, null);
                if (VerticalConst1.Checked)
                {
                    if (verticalConstPoints.Contains(selectedPoints[0]) || verticalConstPoints.Contains(selectedPoints[1]))
                    {
                        ConstraintErrorProvider.SetError(ConstraintButton, "No two connected edges can have same constraint!");
                        return;
                    }
                    if(selectedPoints[0].Y == selectedPoints[1].Y)
                    {
                        ConstraintErrorProvider.SetError(ConstraintButton, "Points will degenerate to one!");
                        return;
                    }
                    selectedPoints[1].X = selectedPoints[0].X;
                    var constraint = new ConstraintGK(ConstraintType.Vertical, selectedPoints);
                    foreach (var point in selectedPoints)
                    {
                        point.AddConstraint(constraint);
                        verticalConstPoints.Add(point);
                    }
                }
                if (HorizontalConst1.Checked)
                {
                    if (horizontalConstPoints.Contains(selectedPoints[0]) || horizontalConstPoints.Contains(selectedPoints[1]))
                    { 
                        ConstraintErrorProvider.SetError(ConstraintButton, "No two connected edges can have same constraint!");
                        return;
                    }
                    if (selectedPoints[0].X == selectedPoints[1].X)
                    {
                        ConstraintErrorProvider.SetError(ConstraintButton, "Points will degenerate to one!");
                        return;
                    }
                selectedPoints[1].Y = selectedPoints[0].Y;
                    var constraint = new ConstraintGK(ConstraintType.Horizontal, selectedPoints);
                    foreach (var point in selectedPoints)
                    {
                        point.AddConstraint(constraint);
                        horizontalConstPoints.Add(point);
                    }
                }
            }
            else
            {
                ConstraintErrorProvider.SetError(ConstraintButton, "Points are not valid for constraint!");
            }
            Canvas.Invalidate();
        }

    }
}
