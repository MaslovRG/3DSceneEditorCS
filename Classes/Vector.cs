using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing; 

namespace _3DSceneEditorCS.Classes
{
	public class Vector : SceneObject
	{
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public static double vradius { get; set; }
        public static MyColor vcolor { get; set; }

        public Vector()
        {
            x = 0.0;
            y = 0.0;
            z = 0.0;
            color = null; 
        }
        
        public Vector(Vector v)
        {
            x = v.x;
			y = v.y;
			z = v.z;
            color = v.color; 
        }
        
        public Vector(double x1, double y1, double z1)
        {
            x = x1;
            y = y1;
            z = z1;
            color = null; 
        }
        
        public Vector normalize()
        {
            double lng = getLength(); 
            double nX = x / lng;
            double nY = y / lng;
            double nZ = z / lng;
            return new Vector(nX, nY, nZ); 
        }

        public double getLength()
        {
            return Math.Sqrt(x * x + y * y + z * z); 
        }

        public double getLength2()
        {
            return x * x + y * y + z * z; 
        }
        
        static public Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }
        
        public static Vector  operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }    
        
        public static  Vector operator *(Vector v, double d)
		{
			return new Vector(v.x*d, v.y*d, v.z*d);
		}
        
        public static Vector operator *(Vector v, Matrix m)
        {
            Vector nv = new Vector();

            nv.x = v.x * m.data[0] + v.y * m.data[4] + v.z * m.data[8] + m.data[12];
            nv.y = v.x * m.data[1] + v.y * m.data[5] + v.z * m.data[9] + m.data[13];
            nv.z = v.x * m.data[2] + v.y * m.data[6] + v.z * m.data[10] + m.data[14];

            return nv; 
        }

        public override void applyMatrix(Matrix matrixP, Matrix matrixV)
        {
            if (matrixP == null)
                return; 
            Matrix m = matrixP; 
            double vx = x;
            double vy = y;
            double vz = z;
            x = vx * m.data[0] + vy * m.data[4] + vz * m.data[8] + m.data[12];
            y = vx * m.data[1] + vy * m.data[5] + vz * m.data[9] + m.data[13];
            z = vx * m.data[2] + vy * m.data[6] + vz * m.data[10] + m.data[14];
        }

        // ��������� ������������
		public static  double operator *(Vector v1, Vector v2)
		{
			return v1.x*v2.x + v1.y*v2.y +  v1.z*v2.z;
		}

        // ��������� ������������
        public static Vector operator %(Vector v1, Vector v2)
        {
            return new Vector(v1.y * v2.z - v1.z * v2.y, v1.z * v2.x - v1.x * v2.z, v1.x * v2.y - v1.y * v2.x);
        }

        public static bool operator ==(Vector v1, Vector v2)
        {
            return (v1.x == v2.x) && (v1.y == v2.y) && (v1.z == v2.z); 
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            return !(v1 == v2); 
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override Intersection isIntersect(Ray r)
        {
            if ((vcolor == null && color == null) || vradius < 0)
                return null; 

            double a = r.direction.getLength2();
            Vector fmc = r.from - this;
            double b = (fmc * r.direction);
            double c = fmc.getLength2() - vradius * vradius;

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
                Vector norm = (point2 - this).normalize();
                double dist = (point2 - r.from).getLength2();
                MyColor clr = vcolor;
                if (color != null)
                    clr = color;
                return new Intersection(point2, norm, this, dist, clr);
            }
            return null;
        }

        public override string ToString()
        {
            return "(" + x.ToString() + ";" + y.ToString() + ";" + z.ToString() + ")";
        }
    }
}
