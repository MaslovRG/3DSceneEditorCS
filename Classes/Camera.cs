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
        public override MyColor color
        {
            get
            {
                return ccolor; 
            }
            set
            {
                ccolor = value; 
            }
        }

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
            return IntersectionsFind.withSphere(r, position, cradius, this); 
        }
    }
}
