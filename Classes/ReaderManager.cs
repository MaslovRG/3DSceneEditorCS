using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing; 

namespace _3DSceneEditorCS.Classes
{
    public class ReaderManager
    {
        protected Reader reader;
        protected char[] section; 

        public ReaderManager(Reader nReader, char[] nSection)
        {
            reader = nReader;
            section = nSection; 
        }

        public Scene getScene()
        {
            Scene nScene = new Scene();
            while (!reader.EndOf())
            {
                string type = reader.ReadLine();
                switch (type)
                {
                    case "figure":
                        Figure figure = readOneFigure();
                        nScene.addFigure(figure);
                        break;
                    case "camera":
                        Camera camera = readOneCamera();
                        nScene.addCamera(camera);
                        break;
                    case "source":
                        Source source = readOneSource();
                        nScene.addSource(source);
                        break;
                }
            } 
            return nScene; 
        } 

        public List<Figure> getFigures()
        {
            List<Figure> figures = new List<Figure>();
            while (!reader.EndOf())
                figures.Add(getOneFigure()); 
            return figures; 
        }

        public List<Camera> getCameras()
        {
            List<Camera> cameras = new List<Camera>(); 
            while (!reader.EndOf())
                cameras.Add(getOneCamera()); 
            return cameras; 
        }

        public List<Source> getSources()
        {
            List<Source> sources = new List<Source>(); 
            while (!reader.EndOf())
                sources.Add(getOneSource()); 
            return sources; 
        }

        public Figure getOneFigure()
        {
            string type = reader.ReadLine();
            Figure figure = readOneFigure();
            return figure; 
        }

        public Camera getOneCamera()
        {
            string type = reader.ReadLine();
            Camera camera = readOneCamera();
            return camera; 
        }

        public Source getOneSource()
        {
            string type = reader.ReadLine();
            Source source = readOneSource();
            return source; 
        }

        private Figure readOneFigure()
        {
            Figure figure;
            string type = reader.ReadLine();
            MyColor color = readColor();
            switch (type)
            {
                case "sphere":
                    Vector scenter = readVector();
                    double sradius = readDouble();                    
                    figure = new Sphere(sradius, scenter, color); 
                    break; 
                case "polygon":
                    int kvertexes;
                    kvertexes = readInt();
                    Vector[] vertexes;
                    vertexes = new Vector[kvertexes]; 
                    int i; 
                    for (i = 0; i < kvertexes; i++)
                        vertexes[i] = readVector();
                    Edge[] edges; 
                    int kedges;
                    kedges = readInt();
                    edges = new Edge[kedges]; 
                    for (i = 0; i < kedges; i++)
                    {
                        int[] con = readCons(); 
                        edges[i] = new Edge(vertexes[con[0]-1], vertexes[con[1]-1]);
                    }
                    figure = new Polygon(vertexes, edges, color); 
                    break;
                case "polyhedron":
                    kvertexes = readInt();
                    vertexes = new Vector[kvertexes]; 
                    for (i = 0; i < kvertexes; i++)
                        vertexes[i] = readVector();                    
                    kedges = readInt();
                    edges = new Edge[kedges]; 
                    for (i = 0; i < kedges; i++)
                    {
                        int[] con = readCons(); 
                        edges[i] = new Edge(vertexes[con[0]-1], vertexes[con[1]-1]);
                    }
                    int kpolygons = readInt(); 
                    Polygon[] polygons = new Polygon[kpolygons]; 
                    for (i = 0; i < kpolygons; i++)
                    {
                        int[] ue = readCons();
                        int[] uv = readCons(); 
                        int num = ue.Count(); 
                        Edge[] te = new Edge[num];
                        int j; 
                        for (j = 0; j < num; j++)
                        {
                            te[j] = edges[ue[j] - 1]; 
                        }
                        num = uv.Count();
                        Vector[] tv = new Vector[num];
                        for (j = 0; j < num; j++)
                        {
                            tv[j] = vertexes[uv[j] - 1]; 
                        }
                        polygons[i] = new Polygon(tv, te, color); 
                    }
                    figure = new Polyhedron(polygons); 
                    break;
                case "cylinder":
                    Vector cvertex1 = readVector();
                    Vector cvertex2 = readVector();
                    double cradius = readDouble();
                    figure = new Сylinder(cvertex1, cvertex2, cradius, color); 
                    break; 
                case "cone":
                    Vector overtex = readVector();
                    Vector ocenter = readVector();
                    double oradius = readDouble();
                    figure = new Cone(overtex, ocenter, oradius, color); 
                    break; 
                default:
                    figure = new Figure();
                    break; 
                    
            }
            return figure; 
        }

        private Camera readOneCamera()
        {
            Vector position = readVector();
            Vector direction = readVector();
            double angleX = readDouble();
            double angleY = readDouble();
            Camera camera = new Camera(position, direction, angleX, angleY);
            return camera;             
        }

        private Source readOneSource()
        {
            Vector position = readVector();
            double depth = readDouble();
            Source source = new Source(position, depth);
            return source; 
        }

        private MyColor readColor()
        {
            Color color = Color.Black;
            string colorType = reader.ReadLine();
            switch (colorType)
            {
                case "name":
                    string colorName = reader.ReadLine();
                    color = Color.FromName(colorName);
                    break;
            }
            MyColorVS rightColor = new MyColorVS(color);
            return rightColor;
        }

        private int readInt()
        {
            string numberstr = reader.ReadLine();
            int number = int.Parse(numberstr);
            return number; 
        }

        private double readDouble()
        {
            string numberstr = reader.ReadLine();
            double number = double.Parse(numberstr);
            return number;
        }

        private Vector readVector()
        {
            string vectorstr = reader.ReadLine();
            string[] coordinates = vectorstr.Split(section);
            double x = double.Parse(coordinates[0]);
            double y = double.Parse(coordinates[1]);
            double z = double.Parse(coordinates[2]);
            Vector vector = new Vector(x, y, z);
            return vector;
        }

        private int[] readCons()
        {
            string constr = reader.ReadLine();
            string[] conmas = constr.Split(section);
            int n = conmas.Count(); 
            int[] cons = new int[n];
            for (int i = 0; i < n; i++)
                cons[i] = int.Parse(conmas[i]);
            return cons; 
        }
    }
}
