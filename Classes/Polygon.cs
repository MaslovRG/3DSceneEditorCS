using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    public class Polygon : Figure
    {
        public Vector[] vertexes; 
        public Edge[] edges;
        //private List<Vector> vertexes;
        private List<Triangle> triangles;
        private int id;
        private static int num = 0;

        public Polygon(Vector[] nVertexes, Edge[] nEdges, MyColor nColor)
        {
            vertexes = nVertexes; 
            edges = nEdges;
            color = nColor;
            triangles = triangulate(vertexes, color);            
            /*vertexes = new List<Vector>();
            foreach (var edge in edges)
            {
                if (!vertexes.Contains(edge.vertex1))
                    vertexes.Add(edge.vertex1);
                if (!vertexes.Contains(edge.vertex2))
                    vertexes.Add(edge.vertex2);
            }*/ 
            num++;
            id = num; 
        }

        public override Intersection isIntersect(Ray r)
        {
            Intersection nI = null;
            if (ViewSettings.interPoint)
            {
                foreach (var vert in vertexes)
                {
                    nI = vert.isIntersect(r);
                    if (nI != null)
                        return nI;
                }
            }

            if (ViewSettings.interEdge)
            {
                foreach (var edge in edges)
                {
                    nI = edge.isIntersect(r);
                    if (nI != null)
                        return nI;
                }
            }
            
            foreach (var trng in triangles)
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
        private static List<Triangle> triangulate(Vector[] vertexes, MyColor color)
        {
            List<Triangle> triangles = new List<Triangle>();
            int cnt = vertexes.Count();
            if (cnt > 2)
            {
                Vector v1 =vertexes[0];
                for (int i = 1; i < cnt - 1; i++)
                {
                    Vector v2 = vertexes[i];
                    Vector v3 = vertexes[i+1];
                    triangles.Add(new Triangle(new Vector[] { v1, v2, v3 }, color));
                }
            }
            return triangles;
        }

        public override void applyMatrix(Matrix matrixP, Matrix matrixV)
        {
            foreach (var vertex in vertexes)
                vertex.applyMatrix(matrixP, null);
        }

        public override void makePrepares()
        {
            foreach (var edge in edges)
                edge.makePrepares();
        }

        public override string ToString()
        {
            return "Многоугольник " + id.ToString();
        }

        public override SceneObject[] getSubs()
        {
            return edges;
        }
    }
}
