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

        private Matrix mToZ;
        private Matrix mToZfD;
        private Matrix mFromZ;
        private Matrix mFromZfD;
        private Vector mV;
        private Vector mC;
        private Vector height; 

        public Cone(Vector nVertex, Vector nCenter, double nRadius, MyColor nColor)
        {
            vertex = nVertex;
            center = nCenter; 
            radius = nRadius;
            color = nColor;
        }

        private void setMatrixes()
        {
            Vector direction = center - vertex;
            Vector position = vertex;
            Matrix moveTo = Matrix.getMove(-position.x, -position.y, -position.z);
            Vector aaa = direction.normalize();
            Matrix rotateTo = Matrix.getRotateToZ(aaa);
            Vector check = aaa * rotateTo;
            double divideZ = 1 / check.z;
            Matrix scaleTo = Matrix.getScale(divideZ, divideZ, divideZ);

            Matrix moveFrom = Matrix.getMove(position.x, position.y, position.z);
            Matrix rotateFrom = Matrix.getRotateFromZ(aaa);
            Matrix scaleFrom = Matrix.getScale(check.z, check.z, check.z);

            mToZfD = rotateTo * scaleTo;
            mToZ = moveTo * rotateTo * scaleTo;
            mFromZfD = scaleFrom * rotateFrom;
            mFromZ = scaleFrom * rotateFrom * moveFrom;
            mV = vertex * mToZ;
            mC = center * mToZ;
            height = mC - mV; 
        }

        public override void applyMatrix(Matrix matrixP, Matrix matrixV)
        {
            vertex.applyMatrix(matrixP, null);
            center.applyMatrix(matrixP, null);
        }

        public override Intersection isIntersect(Ray r)
        {
            Vector from = r.from * mToZ;
            Vector direction = r.direction * mToZfD;

            double x0 = from.x;
            double y0 = from.y;
            double z0 = from.z; 
            double x1 = direction.x;
            double y1 = direction.y;
            double z1 = direction.z; 

            double k = radius * radius / (height.getLength2()); 
            double a = x1 * x1 + y1 * y1 - k * z1  * z1;
            double b = x0 * x1 + y0 * y1 - k * z0 * z1;
            double c = x0 * x0 + y0 * y0 - k * z0 * z0;

            double dd = b * b - a * c;
            double t;
            double tt; 
            if (dd > 0)
            {
                double bda = -b / a;
                dd = Math.Sqrt(dd) / a;
                double t1 = bda + dd;
                double t2 = bda - dd;
                if (t1 < 0 || t2 < 0)
                {
                    t = Math.Max(t1, t2);
                    if (t < 0)
                        return null;
                }
                else
                    t = Math.Min(t1, t2);
                Vector point2 = from + direction * t;
                Vector norm = (point2 - (new Vector(0, 0, point2.z))).normalize(); 
                double dist;
                if (point2.z < mV.z)
                    return null; 
                if (point2.z > mC.z && direction.z != 0)
                {
                    tt = (mC.z - from.z) / direction.z;
                    point2 = from + direction * tt;
                    if (point2.x * point2.x + point2.y * point2.y > radius * radius)
                        return null;
                    norm = height.normalize();
                }
                dist = (point2 - from).getLength2();
                point2 = point2 * mFromZ;
                norm = norm * mFromZfD;  
                return new Intersection(point2, norm, this, dist, color);
            }
            return null;
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
