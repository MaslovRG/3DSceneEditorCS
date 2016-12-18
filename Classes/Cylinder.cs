using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    public class Сylinder : Figure
    {
        public Vector vertex1;
        public Vector vertex2;
        public double radius;

        private Matrix[] mToAndFrom; 

        public Сylinder(Vector nVertex1, Vector nVertex2, double nRadius, MyColor nColor)
        {
            vertex1 = nVertex1;
            vertex2 = nVertex2;
            radius = nRadius;
            color = nColor;
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
            return IntersectionsFind.withCylinder(r, vertex1, vertex2, radius, this, mToAndFrom); 
        }

        public override void makePrepares()
        {
            setMatrixes(); 
        }

        public override string ToString()
        {
            return "Цилиндр в1" + vertex1.ToString() + " в2" + vertex2.ToString() + " р" + radius.ToString();
        }

        public override SceneObject[] getSubs()
        {
            SceneObject[] subs = new SceneObject[] { vertex1, vertex2 }; 
            return subs;
        }
    }
}
