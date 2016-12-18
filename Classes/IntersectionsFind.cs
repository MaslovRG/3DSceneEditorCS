using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    public class IntersectionsFind
    {
        public static Intersection withSphere(Ray ray, Vector center, double radius, SceneObject obj)
        {
            double a = ray.direction.getLength2();
            Vector fmc = ray.from - center;
            double b = (fmc * ray.direction);
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
                Vector point2 = ray.from + ray.direction * tt;
                double dist = (point2 - ray.from).getLength2();
                Vector norm = (point2 - center).normalize();
                return new Intersection(point2, norm, obj, dist, obj.color);
            }
            return null;
        }

        public static Intersection withCylinder(Ray ray, Vector vertex1, Vector vertex2, double radius, SceneObject obj, Matrix[] m)
        {
            Vector from = ray.from * m[0];
            Vector direction = ray.direction * m[1];
            Vector mV1 = vertex1 * m[0];
            Vector mV2 = vertex2 * m[0]; 

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
                point2 = point2 * m[2];
                norm = norm * m[3];
                return new Intersection(point2, norm, obj, dist, obj.color);
            }
            return null;
        }

        public static Intersection withCone(Ray r, Vector center, Vector vertex, double radius, SceneObject obj, Matrix[] m)
        {
            Vector from = r.from * m[0];
            Vector direction = r.direction * m[1];
            Vector mC = center * m[0]; 
            Vector mV = vertex * m[0];
            Vector height = mC - mV; 

            double x0 = from.x;
            double y0 = from.y;
            double z0 = from.z; 
            double x1 = direction.x;
            double y1 = direction.y;
            double z1 = direction.z; 

            double k = radius * radius / (height.getLength2()); 
            double a = x1 * x1 + y1 * y1 - k * z1  * z1;
            double b = x0 * x1 + y0 * y1 - k * z0 * z1;
            double c = x0 * x0 + y0 * y0 - k * z0 * z0;

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
                if (point2.z < mV.z)
                    return null; 
                if (point2.z > mC.z && direction.z != 0)
                {
                    tt = (mC.z - from.z) / direction.z;
                    point2 = from + direction * tt;
                    if (point2.x * point2.x + point2.y * point2.y > radius * radius)
                        return null;
                    norm = height.normalize();
                }
                dist = (point2 - from).getLength2();
                point2 = point2 * m[2];
                norm = norm * m[3];  
                return new Intersection(point2, norm, obj, dist, obj.color);
            }
            return null;
        }
    }
}
