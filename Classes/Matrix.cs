using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace _3DSceneEditorCS.Classes
{
    public class Matrix
    {
        public double[] data; 

        public Matrix()
        {
            data = new double[16];
            for (int i = 0; i < 16; i++ )
                data[i] = 0; 
        }

        public Matrix(Matrix m)
        {
            this.data = new double[16];
            for (int i = 0; i < 16; i++)
                this.data[i] = m.data[i]; 
        }

        public Matrix(double a)
        {
            this.data = new double[16];
            for (int i = 0; i < 16; i++)
                data[i] = 0;
            data[0] = a;
            data[5] = a;
            data[10] = a;
            data[15] = 1;  
        }

        // получает матрицу поворота вектора к оси Z
        public static Matrix getRotateToZ(Vector c)
        {         
            double d = Math.Sqrt(c.y * c.y + c.z * c.z);
            Matrix rotY = new Matrix();
            rotY.data[0] = d;
            rotY.data[2] = c.x;
            rotY.data[5] = 1;
            rotY.data[8] = -c.x;
            rotY.data[10] = d;
            rotY.data[15] = 1;
            if (d != 0)
            {
                Matrix rotX = new Matrix();
                rotX.data[0] = 1;
                rotX.data[5] = c.z / d;
                rotX.data[6] = c.y / d;
                rotX.data[9] = -c.y / d;
                rotX.data[10] = c.z / d;
                rotX.data[15] = 1;

                rotY = rotX * rotY; 
            }          
            return rotY;
        }

        // возвращает обратную к предыдущей матрицу
        public static Matrix getRotateFromZ(Vector c)
        {
            double d = Math.Sqrt(c.y * c.y + c.z * c.z);
            Matrix rotY = new Matrix();
            rotY.data[0] = d;
            rotY.data[2] = -c.x;
            rotY.data[5] = 1;
            rotY.data[8] = c.x;
            rotY.data[10] = d;
            rotY.data[15] = 1;
            if (d != 0)
            {
                Matrix rotX = new Matrix();
                rotX.data[0] = 1;
                rotX.data[5] = c.z / d;
                rotX.data[6] = -c.y / d;
                rotX.data[9] = c.y / d;
                rotX.data[10] = c.z / d;
                rotX.data[15] = 1;

                rotY = rotX * rotY;
            }
            return rotY;
        }

        public static Matrix getMove(double dx, double dy, double dz)
        {
            Matrix matrix = new Matrix();
            matrix.data[0] = 1; 
            matrix.data[5] = 1;
            matrix.data[10] = 1;
            matrix.data[12] = dx;
            matrix.data[13] = dy;
            matrix.data[14] = dz;
            matrix.data[15] = 1; 
            return matrix; 
        }

        public static Matrix getScale(double kx, double ky, double kz/*, double xc, double yc, double zc*/)
        {
            Matrix matrix = new Matrix();
            matrix.data[0] = kx;
            matrix.data[5] = ky;
            matrix.data[10] = kz;
            /*matrix.data[12] = (1 - kx) * xc;
            matrix.data[13] = (1 - ky) * yc;
            matrix.data[14] = (1 - kz) * zc;*/
            matrix.data[15] = 1; 
            return matrix; 
        }

        public static Matrix matrixRotateXGet(double angleX)
        {
            Matrix matrix = new Matrix();
            matrix.data[0] = 1;
            matrix.data[5] = Math.Cos(angleX);
            matrix.data[6] = Math.Sin(angleX);
            matrix.data[9] = -Math.Sin(angleX);
            matrix.data[10] = Math.Cos(angleX);
            matrix.data[15] = 1; 
            return matrix; 
        }

        public static Matrix matrixRotateYGet(double angleY)
        {
            Matrix matrix = new Matrix();            
            matrix.data[0] = Math.Cos(angleY);
            matrix.data[2] = -Math.Sin(angleY);
            matrix.data[5] = 1;
            matrix.data[8] = Math.Sin(angleY);
            matrix.data[10] = Math.Cos(angleY);
            matrix.data[15] = 1;
            return matrix; 
        }

        public static Matrix matrixRotateZGet(double angleZ)
        {
            Matrix matrix = new Matrix();
            matrix.data[0] = Math.Cos(angleZ);
            matrix.data[1] = Math.Sin(angleZ);            
            matrix.data[4] = -Math.Sin(angleZ);
            matrix.data[5] = Math.Cos(angleZ);
            matrix.data[10] = 1;
            matrix.data[15] = 1;
            return matrix; 
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            Matrix nm = new Matrix();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    for (int k = 0; k < 4; k++ )
                    {
                        nm.data[i * 4 + j] += m1.data[i * 4 + k] * m2.data[k * 4 + j];
                    }

            return nm; 
        }

        public static Matrix[] getToZMatrixes(Vector vertex1, Vector vertex2)
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

            Matrix[] ret = new Matrix[4];
            ret[0] = moveTo * rotateTo * scaleTo;
            ret[1] = rotateTo * scaleTo;
            ret[2] = scaleFrom * rotateFrom * moveFrom;
            ret[3] = scaleFrom * rotateFrom;

            return ret; 
        }
    }
}
