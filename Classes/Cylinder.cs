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

        private Matrix mToZ;
        private Matrix mToZfD;
        private Matrix mFromZ;
        private Matrix mFromZfD;
        private Vector mV1;
        private Vector mV2;

        public Сylinder(Vector nVertex1, Vector nVertex2, double nRadius, MyColor nColor)
        {
            vertex1 = nVertex1;
            vertex2 = nVertex2;
            radius = nRadius;
            color = nColor;
        }

        private void setMatrixes()
        {
            Vector direction = vertex2 - vertex1;
            Vector position = vertex1;
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
            mV1 = vertex1 * mToZ;
            mV2 = vertex2 * mToZ;
            mV2.x = 0;
            mV2.y = 0; 
        }

        public override void applyMatrix(Matrix matrixP, Matrix matrixV)
        {
            vertex1.applyMatrix(matrixP, null);
            vertex2.applyMatrix(matrixP, null);
        }

        public override Intersection isIntersect(Ray r)
        {
            Vector from = r.from * mToZ;
            Vector direction = r.direction * mToZfD;

            double x0 = from.x;
            double y0 = from.y;
            double x1 = direction.x;
            double y1 = direction.y;

            double r2 = radius * radius; 
            double a = x1 * x1 + y1 * y1;
            double b = x0 * x1 + y0 * y1;
            double c = x0 * x0 + y0 * y0 - r2;

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
                if (point2.z < mV1.z || point2.z > mV2.z)
                {
                    if (direction.z == 0)
                        return null; 
                    if (point2.z < mV1.z)
                    {
                        tt = (mV1.z - from.z) / direction.z;
                        point2 = from + direction * tt;
                        if (point2.x * point2.x + point2.y * point2.y > r2)
                            return null;
                        norm = (mV1 - mV2).normalize();
                    }
                    else
                    {
                        tt = (mV2.z - from.z) / direction.z;
                        point2 = from + direction * tt;
                        if (point2.x * point2.x + point2.y * point2.y > r2)
                            return null;
                        norm = (mV2 - mV1).normalize();
                    }
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
            return "Цилиндр в1" + vertex1.ToString() + " в2" + vertex2.ToString() + " р" + radius.ToString();
        }

        public override SceneObject[] getSubs()
        {
            SceneObject[] subs = new SceneObject[] { vertex1, vertex2 }; 
            return subs;
        }
    }
}
