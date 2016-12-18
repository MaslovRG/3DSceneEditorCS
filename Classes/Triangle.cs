using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    public class Triangle : Figure
    {
        Vector[] vertexes;
        private const double eps = 1E-4; 

        public Triangle(Vector[] nVertexes, MyColor nColor)
        {
            vertexes = nVertexes;
            color = nColor;
        }

        public override Intersection isIntersect(Ray r)
        {
            Vector v1 = vertexes[0];
            Vector v2 = vertexes[1];
            Vector v3 = vertexes[2];
            Vector normal = (v2 - v1) % (v3 - v1);
            if (normal * r.direction > 0)
                normal *= (-1);
            double d = -(normal * v1);
            double z = -(normal * r.direction);
            if (Math.Abs(z) > eps)
            {
                double t = (normal * r.from + d) / z;
                if (t < 0)
                    return null;
                Vector point = r.from + r.direction * t;

                Vector one = (v2 - v1) % (v2 - point);
                Vector two = (v3 - v2) % (v3 - point);
                Vector thr = (v1 - v3) % (v1 - point);

                double k1 = one * two;
                double k2 = one * thr;
                double k3 = two * thr;

                if (k1 * k2 < 0 || k2 * k3 < 0 || k1 * k3 < 0)
                    return null;

                double dist = (point - r.from).getLength2();
                normal = normal.normalize(); 
                return new Intersection(point, normal, this, dist, this.color);
            }

            return null;
        }

        public override void applyMatrix(Matrix matrixP, Matrix matrixV)
        {
            foreach (var vert in vertexes)
                vert.applyMatrix(matrixP, null);
        }
    }
}
