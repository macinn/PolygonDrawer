using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

// TODO: 30.10

// - wielokat odsuniety
//// ++++
/// popup udana serializacja

namespace GrafikaKomputerowaDrawer
{
    enum DrawerState
    {
        SelectState,
        PointState,
        LineState,
        PolygonState,
        CircleState
    }

    public partial class GKDrawer : Form
    {

        // data containers
        private SortedSet<PointGK> points = new SortedSet<PointGK>(Comparer<PointGK>.Create((p1, p2) =>
            {
                if (p1.X < p2.X || (p1.X == p2.X && p1.Y < p2.Y)) return -1;
                if (p1.X == p2.X && p1.Y == p2.Y) return 0;
                return 1;
            }));
        private List<IObjectGK> objects = new List<IObjectGK>();
        private HashSet<PointGK> verticalConstPoints = new HashSet<PointGK>();
        private HashSet<PointGK> horizontalConstPoints = new HashSet<PointGK>();

        // current state
        private PointGK? mouseDownPoint;
        private VectGK? temporaryVect;
        private DrawerState windowState = DrawerState.SelectState;
        private PointGK polyStart;
        private bool IsDrawing;
        private bool ShiftPressed;
        private bool CtrlPressed;
        private PolygonGK currentPoly;
        private DrawingGK drawingAlg = DrawingGK.Default;
        bool drawOffsetPoly;
        int offSet = 10;

        private void ClearTemp()
        {
            mouseDownPoint = null;
            temporaryVect = null;
            IsDrawing = false;
            polyStart = null;
            currentPoly = null;
        }

        public GKDrawer()
        {
            InitializeComponent();
            SelectButton.Checked = true;
            if (File.Exists(".\\startData.xml"))
            {
                this.Deserialize(".\\startData.xml");
            }
            else
            {
                MessageBox.Show("Przenieœ plik \"startData.xml\" do folderu z plikami wykonywamlnymi, aby otworzyæ scenê startow¹!");
            }
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            Brush backgroundBrush = new SolidBrush(Color.White);
            e.Graphics.FillRectangle(backgroundBrush, new Rectangle(0, 0, Canvas.Width, Canvas.Height));

            foreach (var obj in objects)
            {
                obj.Draw(e, drawingAlg);

                if (drawOffsetPoly && obj.Type == ObjectTypeGK.Poly)
                {
                    PolygonGK poly = obj as PolygonGK;
                    if (poly.completed) poly.DrawOffsetPoly(e, drawingAlg, offSet);
                }
            }
            HashSet<ConstraintGK> consts = new HashSet<ConstraintGK>(points.Select(p => p.constraint).OfType<ConstraintGK>());
            foreach (var constraint in consts)
                constraint.Draw(e);
        }
        private PointGK GetNewPoint(MouseEventArgs me)
        {
            PointGK? newPoint = null;
            if (SnapBox.Checked)
            {
                newPoint = SnapPoint(me);
            }
            if (newPoint == null)
            {
                newPoint = new PointGK(me.X, me.Y);
                points.Add(newPoint);
            }
            return newPoint;
        }
        private VectGK GetNewVect(PointGK p1, PointGK p2, bool addToRender = true)
        {
            VectGK newVect = null;
            newVect = new VectGK(p1, p2);
            if (addToRender)
            {
                objects.Add(newVect);
            }

            //points.Add(p1);
            //points.Add(p2);
            objects.Remove(temporaryVect);
            temporaryVect = null;
            mouseDownPoint = null;
            return newVect;
        }
        private PointGK SnapPoint(MouseEventArgs e)
        {
            PointGK LD = new PointGK(e.X - Parameters.ClickTolerance, e.Y - Parameters.ClickTolerance);
            PointGK RU = new PointGK(e.X + Parameters.ClickTolerance, e.Y + Parameters.ClickTolerance);
            PointGK closest = null; double minDistance = double.MaxValue; double curDistance;
            foreach (var point in points.GetViewBetween(LD, RU))
            {
                curDistance = point.Distance(e.X, e.Y);
                if (curDistance < minDistance && curDistance < Parameters.ClickTolerance)
                {
                    minDistance = curDistance;
                    closest = point;
                }

            }
            return closest;
        }

        private void DefButton_CheckedChanged(object sender, EventArgs e)
        {
            if (DefButton.Checked) drawingAlg = DrawingGK.Default;
            Canvas.Invalidate();
        }

        private void BresButton_CheckedChanged(object sender, EventArgs e)
        {
            if (BresButton.Checked) drawingAlg = DrawingGK.Default;
            Canvas.Invalidate();
        }

        private void constrintsOnSegmentToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            ConstraintBox.Visible = constrintsOnSegmentToolStripMenuItem.Checked;
        }

        private void ConstraintBox_MouseMove(object sender, EventArgs e)
        {
            bool updateData = false;
            var selectedPoints = points.ToList().FindAll(p => p.IsSelected == true);
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
                            updateData = true;
                            break;
                        }
                        if (obj.Type == ObjectTypeGK.Poly)
                        {
                            var poly = obj as PolygonGK;
                            int ind0 = -1, ind1 = -1;

                            for (int i = 0; i < poly.N; i++)
                            {
                                if (poly.GetPoints()[i] == p0) ind0 = i;
                                if (poly.GetPoints()[i] == p0) ind1 = i;
                            }
                            if (Math.Abs(ind0 - ind1) == 1)
                            {
                                updateData = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (updateData)
            {
                ConstraintButton.Enabled = true;
                VerticalConst1.Enabled = true;
                HorizontalConst1.Enabled = true;

            }
            else
            {
                ConstraintButton.Enabled = false;
                VerticalConst1.Enabled = false;
                HorizontalConst1.Enabled = false;
            }

        }

        private void PolygonButton_CheckedChanged(object sender, EventArgs e)
        {
            if (PolygonButton.Checked)
            {
                windowState = DrawerState.PolygonState;
                ClearTemp();
                Canvas.Invalidate();
            }
        }

        private void LineButton_CheckedChanged(object sender, EventArgs e)
        {
            if (LineButton.Checked)
            {
                windowState = DrawerState.LineState;
                ClearTemp();
                Canvas.Invalidate();
            }
        }

        private void PointButton_CheckedChanged(object sender, EventArgs e)
        {
            if (PointButton.Checked)
            {
                windowState = DrawerState.PointState;
                ClearTemp();
                Canvas.Invalidate();
            }
        }

        private void SelectButton_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectButton.Checked)
            {
                windowState = DrawerState.SelectState;
                ClearTemp();
                Canvas.Invalidate();
            }
            else
            {
                foreach (var obj in objects)
                    obj.IsSelected = false;
                foreach (var p in points)
                    p.IsSelected = false;
                Canvas.Invalidate();
            }
        }

        private void RemoveConstraintButton_Click(object sender, EventArgs e)
        {
            var selectedPoints = points.ToList().FindAll(p => p.IsSelected == true);

            if (!(selectedPoints.Count == 2 && selectedPoints[0].constraint != null && selectedPoints[0].constraint == selectedPoints[1].constraint))
            {
                ConstraintErrorProvider.SetError(RemoveConstraintButton, "Select valid points for contraint deletion");
                return;
            }
            else
            {
                ConstraintErrorProvider.SetError(RemoveConstraintButton, null);
            }

            foreach (var p in selectedPoints)
            {
                p.RemoveConstraint();
                verticalConstPoints.Remove(p);
                horizontalConstPoints.Remove(p);
            }
            Canvas.Invalidate();
        }

        private void serializeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //https://learn.microsoft.com/pl-pl/dotnet/api/system.windows.forms.openfiledialog?view=windowsdesktop-7.0

            using SaveFileDialog openFileDialog = new SaveFileDialog();
            openFileDialog.InitialDirectory = ".\\";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog.DefaultExt = "xml";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(openFileDialog.FileName))
                    File.Create(openFileDialog.FileName).Close();
                this.Serialize(openFileDialog.FileName);

            }
        }

        private void deserializeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = ".\\";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog.DefaultExt = "xml";
            //openFileDialog.FilterIndex = 2;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.Deserialize(openFileDialog.FileName);

            }
        }

        private void clearDrawerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.points.Clear();
            this.objects.Clear();
            this.verticalConstPoints.Clear();
            this.horizontalConstPoints.Clear();
            ClearTemp();
            Canvas.Invalidate();
        }

        private void drawOffsetPolygonToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            OffsetPolyBox.Visible = drawOffsetPolygonToolStripMenuItem.Checked;
            drawOffsetPoly = drawOffsetPolygonToolStripMenuItem.Checked;
            offSet = (int)OffsetInput.Value;
            Canvas.Invalidate();
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            offSet = (int)OffsetInput.Value;
            Canvas.Invalidate();
        }

        private void CircleButton_CheckedChanged(object sender, EventArgs e)
        {
            if (CircleButton.Checked)
            {
                windowState = DrawerState.CircleState;

                CircleBox.Visible = true;

            }
            else
            {
                CircleBox.Visible = false;
            }
        }

        private void RadiusBox_TextChanged(object sender, EventArgs e)
        {
            CircleGK circle = objects.Find(p=> p.IsSelected && p.Type == ObjectTypeGK.Circle) as CircleGK;
            if (circle != null)
            {
                int.TryParse(RadiusBox.Text, out int rad);
                circle.radius = rad;
                Canvas.Invalidate();
            }

        }
    }
    public class Display : Panel
    {

        public Display()
        {
            this.DoubleBuffered = true;
        }
    }

}