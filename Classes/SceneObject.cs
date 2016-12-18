using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    public class SceneObject
    {
        public virtual MyColor color { get; set; }

        public virtual Intersection isIntersect(Ray r)
        {
            return null;
        } 

        public virtual void applyMatrix(Matrix matrixP, Matrix matrixV)
        {

        }

        public virtual void makePrepares()
        {

        }

        public virtual void undoPrepares()
        {

        }

        public virtual SceneObject[] getSubs()
        {
            return null; 
        }
    }
}
