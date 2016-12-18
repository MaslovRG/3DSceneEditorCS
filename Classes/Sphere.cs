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
            return IntersectionsFind.withSphere(r, center, radius, this); 
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
