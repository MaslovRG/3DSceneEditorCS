using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    public class Polyhedron : Figure
    {
        public Polygon[] polygons;
        private List<Vector> vertexes; 
        private int id;
        private static int num = 0;  

        public Polyhedron(Polygon[] nPolygons)
        {
            polygons = nPolygons;
            vertexes = new List<Vector>(); 
            foreach(var polygon in polygons)
            {
                foreach (var edge in polygon.edges)
                {
                    if (!vertexes.Contains(edge.vertex1))
                        vertexes.Add(edge.vertex1);
                    if (!vertexes.Contains(edge.vertex2))
                        vertexes.Add(edge.vertex2);
                }
            }
            num++;
            id = num; 
        }

        public override void applyMatrix(Matrix matrixP, Matrix matrixV)
        {
            foreach (var vertex in vertexes)
                vertex.applyMatrix(matrixP, null); 
        }

        public override Intersection isIntersect(Ray r)
        {
            Intersection nI = null;
            foreach (var polygon in polygons)
            {
                Intersection nI1 = polygon.isIntersect(r);
                if (nI1 != null && (nI == null || nI1.distance < nI.distance))
                    nI = nI1;
            }
            return nI;
        }

        public override string ToString()
        {
            return "Многогранник " + id.ToString();
        }

        public override SceneObject[] getSubs()
        {
            return polygons;
        }

        public override void makePrepares()
        {
            foreach (var polygon in polygons)
                polygon.makePrepares(); 
        }
    }
}
