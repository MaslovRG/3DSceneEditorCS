using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    public class SceneObject
    {
        public MyColor color { get; set; }

        public virtual Intersection isIntersect(Ray r)
        {
            return null;
        } 

        public virtual void applyMatrix(Matrix matrix)
        {

        }

        public virtual bool haveVertexes()
        {
            return false; 
        }

        public virtual Vector[] getVertexes()
        {
            return null; 
        }
    }
}
