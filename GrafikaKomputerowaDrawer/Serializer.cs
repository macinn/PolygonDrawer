using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GrafikaKomputerowaDrawer
{
    public partial class GKDrawer : Form
    {

        private void Serialize(string filename)
        {
            //filename = "./serializedData.xml";
            DataBank db = new DataBank();
            db.CreateFromDrawer(points.ToList(), objects);
            XmlSerializer ser = new XmlSerializer(typeof(DataBank));

            TextWriter writer = new StreamWriter(filename);
            ser.Serialize(writer, db);
            writer.Close();

        }

        private void Deserialize(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(DataBank));

            TextReader reader = new StreamReader(filename);
            DataBank? db = ser.Deserialize(reader) as DataBank;
            reader.Close();
            if (db != null)
            {
                this.points.Clear();
                this.objects.Clear();
                this.verticalConstPoints.Clear();
                this.horizontalConstPoints.Clear();
                ClearTemp();

                foreach(var p in db.points)
                {
                    this.points.Add(p);
                }
                foreach(var vect in db.vectors)
                {
                    PointGK p1 = db.points.Find((p) => (p.X, p.Y) == (vect.p1.X, vect.p1.Y));

                    PointGK p2 = db.points.Find((p) => (p.X, p.Y) == (vect.p2.X, vect.p2.Y));
                    vect.p1 = p1;
                    vect.p2 = p2;
                    p1.partOf.Add(vect);
                    p2.partOf.Add(vect);
                    this.objects.Add(vect);
                }
                foreach(var polyDB in db.polygons)
                {
                    PolygonGK polygon = new PolygonGK();

                    foreach(PointGK polyPoint in polyDB.GetPoints())
                    {
                        PointGK p = db.points.Find((p) => (p.X, p.Y) == (polyPoint.X, polyPoint.Y));
                        polygon.Add(p);
                    }
                    polygon.completed = true;
                    this.objects.Add(polygon);
                }
                foreach(var constraint in db.constraints)
                {
                    for(int i=0; i<2; i++)
                    {
                        PointGK pC = constraint.points[i];
                        PointGK p = db.points.Find((p) => (p.X, p.Y) == (pC.X, pC.Y));
                        constraint.points[i] = p;
                        p.AddConstraint(constraint);
                        if(constraint.type == ConstraintType.Horizontal)
                            horizontalConstPoints.Add(p);
                        else if (constraint.type == ConstraintType.Vertical)
                            verticalConstPoints.Add(p);
                    }
                }
                Canvas.Invalidate();
            }
            
        }
        
    }

    [XmlRoot("databank")]
    [XmlInclude(typeof(PointGK))]
    [XmlInclude(typeof(VectGK))]
    [XmlInclude(typeof(PolygonGK))]
    public class DataBank
    {
        public List<ConstraintGK>? constraints;

        public List<PointGK>? points;
        public List<VectGK>? vectors;
        public List<PolygonGK>? polygons;

        public void CreateFromDrawer(List<PointGK> points, List<IObjectGK> objects)
        {
            constraints = new();

            this.points = points;
            vectors = new();
            polygons = new();
            foreach (var obj in objects)
            {
                if (obj.Type == ObjectTypeGK.Vect)
                    vectors.Add((VectGK)obj);
                else if (obj.Type == ObjectTypeGK.Poly)
                    polygons.Add((PolygonGK)obj);
            }
            HashSet<ConstraintGK> constraintsSet = new();
            foreach (var p in points)
                if (p.constraint != null)
                    constraintsSet.Add(p.constraint);
            constraints = constraintsSet.ToList();
        }

    }
}
