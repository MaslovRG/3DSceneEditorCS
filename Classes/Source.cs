﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing; 

namespace _3DSceneEditorCS.Classes
{
    public class Source : SceneObject
    {
        public Vector position { get; set; }
        public double depth { get; set; }
        public static MyColor scolor { get; set; }
        public static double sradius { get; set; }

        public Source(Vector nPos, double nDepth)
        {
            this.position = nPos; 
            this.depth = nDepth;
        }

        public override void applyMatrix(Matrix matrixP, Matrix matrixV)
        {
            position.applyMatrix(matrixP, null); 
        }

        public override Intersection isIntersect(Ray r)
        {
            if (scolor == null && color == null)
                return null;

            double a = r.direction.getLength2();
            Vector fmc = r.from - position;
            double b = (fmc * r.direction);
            double c = fmc.getLength2() - sradius * sradius;

            double dd = b * b - a * c;
            double tt;

            if (dd >= 0)
            {
                double bda = -b / a;
                dd = Math.Sqrt(dd) / a;
                double t1 = bda + dd;
                double t2 = bda - dd;
                if (t1 < 0 || t2 < 0)
                {
                    tt = Math.Max(t1, t2);
                    if (tt < 0)
                        return null;
                }
                else
                    tt = Math.Min(t1, t2);
                Vector point2 = r.from + r.direction * tt;
                double dist = (point2 - r.from).getLength2();
                Vector norm = (point2 - position).normalize();
                MyColor clr = scolor;
                if (color != null)
                    clr = color;
                return new Intersection(point2, norm, this, dist, clr);
            }
            return null;
        }

        public override string ToString()
        {
            return "Источник света п" + position.ToString() + " я" + depth.ToString();
        }
    }
}
