using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    public class Edge : Figure
    {
        public Vector vertex1 { get; set; }
        public Vector vertex2 { get; set; }
        public override MyColor color
        {
            get
            {
                return ecolor;
            }
            set
            {
                ecolor = value; 
            }
        }

        public static double eradius { get; set; }
        public static MyColor ecolor { get; set; }
        private Matrix[] mToAndFrom; 

        public Edge(Vector nVertex1, Vector nVertex2)
        {
            vertex1 = nVertex1;
            vertex2 = nVertex2;
        }

        private void setMatrixes()
        {
            mToAndFrom = Matrix.getToZMatrixes(vertex1, vertex2); 
        }

        public override void applyMatrix(Matrix matrixP, Matrix matrixV)
        {
            vertex1.applyMatrix(matrixP, null);
            vertex2.applyMatrix(matrixP, null);
        }

        public override Intersection isIntersect(Ray r)
        {
            return IntersectionsFind.withCylinder(r, vertex1, vertex2, eradius, this, mToAndFrom); 
        }

        public override void makePrepares()
        {
            setMatrixes();
        }

        public override string ToString()
        {
            return "Ребро в1" + vertex1.ToString() + " в2" + vertex2.ToString();
        }

        public override SceneObject[] getSubs()
        {
            SceneObject[] subs = new SceneObject[] { vertex1, vertex2 };
            return subs;
        }
    }
}
