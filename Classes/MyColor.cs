using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    public class MyColor : ICloneable
    {
        public virtual object Clone()
        {
            return new MyColor(); 
        }

        public virtual void SetSaturation(double proc)
        {
            // empty
        }
    }
}
