using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    public class Cone : Figure
    {
        public Vector vertex;
        public Vector center;
        public double radius;

        private Matrix[] mToAndFrom; 

        public Cone(Vector nVertex, Vector nCenter, double nRadius, MyColor nColor)
        {
            vertex = nVertex;
            center = nCenter; 
            radius = nRadius;
            color = nColor;
        }

        private void setMatrixes()
        {
            mToAndFrom = Matrix.getToZMatrixes(vertex, center);              
        }

        public override void applyMatrix(Matrix matrixP, Matrix matrixV)
        {
            vertex.applyMatrix(matrixP, null);
            center.applyMatrix(matrixP, null);
        }

        public override Intersection isIntersect(Ray r)
        {
            return IntersectionsFind.withCone(r, center, vertex, radius, this, mToAndFrom);
        }

        public override void makePrepares()
        {
            setMatrixes(); 
        }

        public override string ToString()
        {
            return "Конус ц" + center.ToString() + " в" + vertex.ToString() + " р" + radius.ToString();
        }

        public override SceneObject[] getSubs()
        {
            SceneObject[] subs = new SceneObject[] { center, vertex }; 
            return subs;
        }
    }
}
