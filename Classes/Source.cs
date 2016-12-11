using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing; 

namespace _3DSceneEditorCS.Classes
{
    public class Source : SceneObject
    {
        public Vector position;
        public double depth; 

        public Source(Vector nPos, double nDepth)
        {
            this.position = nPos; 
            this.depth = nDepth;
            this.color = new MyColorVS(Color.Yellow); 
        }

        public override void applyMatrix(Matrix m)
        {
            position *= m; 
        }
    }
}
