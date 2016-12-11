using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    public class Reader
    {
        public virtual bool EndOf()
        {
            return true;
        }

        public virtual string ReadLine()
        {
            return ""; 
        }
    }
}
