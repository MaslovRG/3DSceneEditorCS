using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    public class Figure : SceneObject
    {               
        /*public virtual Vector getNormal(Vector point)
        {
            return new Vector(); 
        }*/

        /*public virtual void triangulate()
        {
            // empty
        }*/
    }

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
                tt = Math.Min(t1, t2); 
                Vector point2 = r.from + r.direction * tt;
                double dist = (point2 - r.from).getLength2();
                Vector norm = (point2 - center).normalize(); 
                return new Intersection(point2, norm, this, dist, this.color);
            }
            return null;
        }

        public override void applyMatrix(Matrix matrix)
        {
            center.applyMatrix(matrix);  
        }
    }

    public class Edge : Figure
    {
        public Vector vertex1;
        public Vector vertex2; 

        public Edge(Vector nVertex1, Vector nVertex2)
        {
            vertex1 = nVertex1;
            vertex2 = nVertex2; 
        }

        public override void applyMatrix(Matrix matrix)
        {
            vertex1.applyMatrix(matrix);
            vertex2.applyMatrix(matrix); 
        }

        public override bool haveVertexes()
        {
            return true; 
        }
    }

    public class Polygon : Figure
    {
        public Edge[] edges;
        private List<Vector> vertexes; 
        private List<Triangle> triangles; 
        protected const double eps = 1E-6; 

        public Polygon(Edge[] nEdges, MyColor nColor)
        {
            edges = nEdges;
            color = nColor; 
            triangles = triangulate(edges, color);
            vertexes = new List<Vector>(); 
            foreach (var edge in edges)
                vertexes.Add(edge.vertex1); 
        }

        public override Intersection isIntersect(Ray r)
        {
            Intersection nI = null;
            foreach (var vert in vertexes)
            {
                nI = vert.isIntersect(r);
                if (nI != null)
                    return nI; 
            }
            foreach(var trng in triangles)
            {
                nI = trng.isIntersect(r);
                if (nI != null)
                {
                    nI.figure = this;
                    return nI; 
                }
            }
            return nI; 
        }

        // выпуклых и в  одной плоскости
        private static List<Triangle> triangulate(Edge[] edges, MyColor color)
        {           
            List<Triangle> triangles = new List<Triangle>();
            int cnt = edges.Count();
            if (cnt > 2)
            {
                Vector v1 = edges[0].vertex1;
                for (int i = 1; i < cnt-1; i++)
                {
                    Vector v2 = edges[i].vertex1;
                    Vector v3 = edges[i].vertex2;
                    triangles.Add(new Triangle(new Vector[] { v1, v2, v3 }, color)); 
                }
            }
            return triangles; 
        }

        public override void applyMatrix(Matrix matrix)
        {
            foreach (var vertex in vertexes)
                vertex.applyMatrix(matrix); 
        }
    }

    public class Triangle : Figure
    {
        Vector[] vertexes; 

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
            if (z != 0)
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
                return new Intersection(point, normal, this, dist, this.color);
            }

            return null;
        }

        public override void applyMatrix(Matrix matrix)
        {
            foreach (var vert in vertexes)
                vert.applyMatrix(matrix); 
        }

        public override bool haveVertexes()
        {
            return true;
        }

        public override Vector[] getVertexes()
        {
            return vertexes;
        }
    }

    /*public class Polyhedron 
    {
        public Polygon[] polygonMas;
        public Edge[,] connectionMas; 
    }*/
}
