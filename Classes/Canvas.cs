// базовый класс канвы
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DSceneEditorCS.Classes
{
    public class Canvas
    {
        public Canvas()
        {
            // empty 
        }

        public virtual void drawPixel(int x, int y, MyColor clr)
        {
            // empty
        }

        public virtual void setObject(int x, int y, SceneObject obj)
        {
            // empty
        }

        public virtual void allEmpty()
        {
            // empty
        }

        public virtual int getX()
        {
            return 0;
        }

        public virtual int getY()
        {
            return 0; 
        }

        public virtual MyColor getBackColor()
        {
            return new MyColor(); 
        }

        public virtual void endDraw()
        {
            // empty
        }
    }
}
