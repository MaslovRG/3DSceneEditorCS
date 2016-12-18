using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _3DSceneEditorCS.Classes
{
    public class Camera : SceneObject
    {
        public Vector position { get; set; }
        public Vector direction { get; set; }
        public double angleX { get; set; }
        public double angleY { get; set; }

        public Matrix matrixToZ { get; private set; }
        public Matrix mToZfD { get; private set; }
        public Matrix matrixFromZ { get; private set; }
        public Matrix mFromZfD { get; private set; }

        public static MyColor ccolor { get; set; }
        public static double cradius { get; set; }

        public Camera(Vector nPosition, Vector nDirection, double nAngleX, double nAngleY)
        {
            this.position = nPosition;
            this.direction = nDirection;
            this.angleX = nAngleX;
            this.angleY = nAngleY;           
        }

        public void setMatrixes()
        {
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
            matrixToZ = moveTo * rotateTo * scaleTo;
            mFromZfD = scaleFrom * rotateFrom;
            matrixFromZ = scaleFrom * rotateFrom * moveFrom; 
        }

        public override void applyMatrix(Matrix matrixP, Matrix matrixV)
        {
            position.applyMatrix(matrixP, null);
            direction.applyMatrix(matrixV, null); 
        }

        public override string ToString()
        {
            return "Камера п" + position.ToString() + " н" + direction.ToString() + " о(" + angleX.ToString() + ";" + angleY.ToString() + ")";
        }

        public override SceneObject[] getSubs()
        {
            SceneObject[] subs = new SceneObject[] { position, direction }; 
            return subs;
        }

        public override Intersection isIntersect(Ray r)
        {
            if (ccolor == null && color == null)
                return null;

            double a = r.direction.getLength2();
            Vector fmc = r.from - position;
            double b = (fmc * r.direction);
            double c = fmc.getLength2() - cradius * cradius;

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
                Vector norm = (point2 - position).normalize();
                MyColor clr = ccolor;
                if (color != null)
                    clr = color;
                return new Intersection(point2, norm, this, dist, clr);
            }
            return null;
        }
    }
}
