using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing; 

namespace _3DSceneEditorCS.Classes
{
    public class CanvasVS : Canvas
    {
        public Bitmap canv;
        public Color bgcolor;
        public MyColor[,] ccanv; 
        public SceneObject[,] mas; 

        public CanvasVS(Bitmap image, Color nColor, SceneObject[,] mas)
        {
            this.canv = image;
            this.bgcolor = nColor;
            this.mas = mas;
            this.ccanv = new MyColor[image.Width, image.Height]; 
        }

        public override void drawPixel(int x, int y, MyColor clr)
        {
            /*if (clr != null)
            {
                MyColorVS clr1 = (MyColorVS)clr;
                canv.SetPixel(x, y, clr1.color);
            }*/
            ccanv[x, y] = clr; 
        }

        public override void setObject(int x, int y, SceneObject obj)
        {
            if (obj != null)
                mas[x, y] = obj; 
        }

        public override void allEmpty()
        {
            canv = new Bitmap(canv.Width, canv.Height);
            mas = new SceneObject[canv.Width, canv.Height]; 
        }

        public override int getX()
        {
            return canv.Width - 1;
        }

        public override int getY()
        {
            return canv.Height - 1;
        }

        /*public override MyColor getBackColor()
        {
            return new MyColorVS(bgcolor); 
        }*/

        public override void endDraw()
        {
            for (int i = 0; i < canv.Width; i++)
                for (int j = 0 ; j < canv.Height; j++)
                    if (ccanv[i,j] != null)
                    {
                        MyColorVS clr1 = (MyColorVS)ccanv[i,j];
                        canv.SetPixel(i, j, clr1.color);
                    }

        }
    }
}
