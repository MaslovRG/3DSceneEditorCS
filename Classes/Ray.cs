using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    public class Ray
    {
        public Vector from;
        public Vector direction; 

        public Ray(Vector fr, Vector dir)
        {
            this.from = fr;
            this.direction = dir; 
        }
    }
}
