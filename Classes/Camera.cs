using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _3DSceneEditorCS.Classes
{
    public class Camera : SceneObject
    {
        public Vector position;
        public Vector direction;
        public double angleX;
        public double angleY;
        public Matrix matrixToZ;
        public Matrix matrixFromZ;

        public Camera(Vector nPosition, Vector nDirection, double nAngleX, double nAngleY)
        {
            this.position = nPosition;
            this.direction = nDirection;
            this.angleX = nAngleX;
            this.angleY = nAngleY;
            this.color = new MyColorVS(Color.Green);

            Matrix moveTo = Matrix.getMove(-position.x, -position.y, -position.z);
            Vector aaa = direction.normalize(); 
            Matrix rotateTo = Matrix.getRotateToZ(aaa);
            Vector check = aaa * rotateTo; 
            double divideZ = 1 / check.z;
            Matrix scaleTo = Matrix.getScale(divideZ, divideZ, divideZ, 0, 0, 0); 

            Matrix moveFrom = Matrix.getMove(position.x, position.y, position.z);
            Matrix rotateFrom = Matrix.getRotateFromZ(direction);
            Matrix scaleFrom = Matrix.getScale(check.z, check.z, check.z, 0, 0, 0); 
            
            this.matrixToZ = moveTo * rotateTo * scaleTo;
            this.matrixFromZ = scaleFrom * rotateFrom * moveFrom; 
        }

        public override void applyMatrix(Matrix matrix)
        {
            position *= matrix;
            direction *= matrix; 
        }
    }
}
