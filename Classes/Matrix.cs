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

        // создаёт матрицу поворота из v2 в v1
        // создаёт матрицу поворота из 
        /*public Matrix(Vector v1, Vector v2)
        {
            Vector l = v1 % v2;
            double cf = v1 * v2; // cos(fi)
            double df = 1.0 / (1.0 + cf);

            Vector m = l * df;
            double a = l.x * m.y;
            double b = l.y * m.z;
            double c = l.z * m.x;
            data = new double[16];
 
            data[0] = l.x * m.x + cf;
            data[1] = a - l.z;
            data[2] = c + l.y;
            data[3] = 0;

            data[4] = a + l.z;
            data[5] = l.y * m.y + cf;
            data[6] = c + l.y;
            data[7] = 0;

            data[8] = c - l.y;
            data[9] = b + l.x;
            data[10] = l.z * m.z + cf;
            data[11] = 0;

            data[12] = 0;
            data[13] = 0;
            data[14] = 0;
            data[15] = 1; 
        }*/

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

        public static Matrix getScale(double kx, double ky, double kz, double xc, double yc, double zc)
        {
            Matrix matrix = new Matrix();
            matrix.data[0] = kx;
            matrix.data[5] = ky;
            matrix.data[10] = kz;
            matrix.data[12] = (1 - kx) * xc;
            matrix.data[13] = (1 - ky) * yc;
            matrix.data[14] = (1 - kz) * zc;
            matrix.data[15] = 1; 
            return matrix; 
        }

        public static Matrix matrixRotateXGet(double angleX, double xc, double yc, double zc)
        {
            Matrix matrix = new Matrix();
            matrix.data[0] = 1;
            matrix.data[5] = Math.Cos(angleX);
            matrix.data[6] = Math.Sin(angleX);
            matrix.data[9] = -Math.Sin(angleX);
            matrix.data[10] = Math.Cos(angleX);
            matrix.data[13] = yc - yc * Math.Cos(angleX) + zc * Math.Sin(angleX);
            matrix.data[14] = zc - yc * Math.Sin(angleX) - yc * Math.Cos(angleX);
            matrix.data[15] = 1; 
            return matrix; 
        }

        public static Matrix matrixRotateYGet(double angleY, double xc, double yc, double zc)
        {
            Matrix matrix = new Matrix();            
            matrix.data[0] = Math.Cos(angleY);
            matrix.data[2] = -Math.Sin(angleY);
            matrix.data[5] = 1;
            matrix.data[8] = Math.Sin(angleY);
            matrix.data[10] = Math.Cos(angleY);
            matrix.data[12] = xc - xc * Math.Cos(angleY) - zc * Math.Sin(angleY);
            matrix.data[14] = zc + xc * Math.Sin(angleY) - zc * Math.Cos(angleY);
            matrix.data[15] = 1;
            return matrix; 
        }

        public static Matrix matrixRotateZGet(double angleZ, double xc, double yc, double zc)
        {
            Matrix matrix = new Matrix();
            matrix.data[0] = Math.Cos(angleZ);
            matrix.data[1] = Math.Sin(angleZ);            
            matrix.data[4] = -Math.Sin(angleZ);
            matrix.data[5] = Math.Cos(angleZ);
            matrix.data[10] = 1;
            matrix.data[12] = xc - xc * Math.Cos(angleZ) + yc * Math.Sin(angleZ);
            matrix.data[13] = yc - xc * Math.Sin(angleZ) - yc * Math.Cos(angleZ);
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
    }
}
