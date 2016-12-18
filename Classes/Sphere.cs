using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    public class Sphere : Figure
    {
        // свойства
        public double radius;
        public Vector center;

        public Sphere(double nwRadius, Vector nwCenter, MyColor nwColor)
        {
            this.radius = nwRadius;
            this.center = nwCenter;
            this.color = nwColor;
        }

        public override Intersection isIntersect(Ray r)
        {
            double a = r.direction.getLength2();
            Vector fmc = r.from - center;
            double b = (fmc * r.direction);
            double c = fmc.getLength2() - radius * radius;

            double dd = b * b - a * c;
            double tt;

            if (dd >= 0)
            {
                double bda = -b / a;
                dd = Math.Sqrt(dd) / a;
                double t1 = bda + dd;
                double t2 = bda - dd;
                if (t1 < 0 || t2 < 0)
                {
                    tt = Math.Max(t1, t2);
                    if (tt < 0)
                        return null;
                }
                else
                    tt = Math.Min(t1, t2);
                Vector point2 = r.from + r.direction * tt;
                double dist = (point2 - r.from).getLength2();
                Vector norm = (point2 - center).normalize();
                return new Intersection(point2, norm, this, dist, this.color);
            }
            return null;
        }

        public override void applyMatrix(Matrix matrixP, Matrix matrixV)
        {
            center.applyMatrix(matrixP, null);
        }

        public override string ToString()
        {
            return "Сфера ц" + center.ToString() + " р" + radius.ToString();
        }
    }
}
