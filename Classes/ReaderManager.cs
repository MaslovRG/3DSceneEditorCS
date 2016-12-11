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
                    Vector position = readVector();
                    double radius = readDouble();                    
                    figure = new Sphere(radius, position, color); 
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
    }
}
