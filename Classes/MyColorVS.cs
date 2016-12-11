using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing; 

namespace _3DSceneEditorCS.Classes
{
    public class MyColorVS : MyColor
    {
        public Color color; 

        public MyColorVS(Color nColor)
        {
            this.color = nColor;
        }

        public override object Clone()
        {
            return new MyColorVS(this.color); 
        }

        public override void SetSaturation(double proc)
        {
            color = Color.FromArgb((int)(proc * 255), color); 
        }
    }
}
