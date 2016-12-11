using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    public class Intersection
    {
        public Vector point;
        public Vector normal; 
        public SceneObject figure;
        public double distance; 

        public Intersection(Vector nPoint, Vector nNormal, SceneObject nFigure, double nDist)
        {
            this.point = nPoint;
            this.normal = nNormal; 
            this.figure = nFigure;
            this.distance = nDist; 
        }
    }
}
