﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; 

namespace _3DSceneEditorCS.Classes
{
    public class RayTracer
    {
        public Scene scene;
        public Canvas canv; 
        private const double eps = 1E-5; 

        public RayTracer(Scene sc, Canvas cv)
        {
            this.scene = sc;
            this.canv = cv; 
           
        }

        public void drawScene(bool isEdit)
        {
            DateTime time1 = DateTime.Now; 
            int width = canv.getX();
            int height = canv.getY();
            Camera camera = null;
            canv.allEmpty(); 
            if (scene == null || (camera  = scene.getActiveCamera()) == null || scene.getFigures().Count == 0)
                return;
            camera.setMatrixes(); 
            scene.applyMatrix(camera.matrixToZ, camera.mToZfD);
            scene.makePrepares(); 
            double aX = (camera.angleX / 2) * Math.PI / 180;
            double aY = (camera.angleY / 2) * Math.PI / 180; 

            double dopX = aX / (double)width * 2;
            double dopY = aY / (double)height * 2;

            double nnz = 100; 
            /*Vector vray = new Vector(0, 0, nnz); 
            Ray ray = new Ray(camera.position, vray);*/

            Stopwatch stp = new Stopwatch();

            stp.Start(); 
            ParallelLoopResult result = Parallel.For(0, height, y =>
                {
                    double nY = y * dopY - aY;
                    Vector vray = new Vector(0, nnz * Math.Tan(nY), nnz); 
                    for (int x = 0; x < width; x++)
                    {
                        double nX = x * dopX - aX;
                        vray.x = nnz * Math.Tan(nX);

                        SceneObject pixelObject;
                        Ray ray = new Ray(camera.position, vray);
                        MyColor clr = Sampler(ray, out pixelObject, camera);

                        canv.drawPixel(x, height - y, clr);
                        canv.setObject(x, height - y, pixelObject);
                    }
                });
            canv.endDraw();
            /*for (int y = 0; y < height; y++)
            {
                double nY = y * dopY - aY;
                vray.y = nnz * Math.Tan(nY);
                for (int x = 0; x < width; x++)
                {
                    if (x == 250 && y == 250)
                    {
                        nnz++;
                        nnz--; 
                    }
                    double nX = x * dopX - aX;                    
                    vray.x = nnz * Math.Tan(nX);

                    SceneObject pixelObject;
                    MyColor clr = Sampler(ray, out pixelObject, isEdit);

                    canv.drawPixel(x, height - y, clr);
                    canv.setObject(x, height - y, pixelObject);
                }
            }*/
            stp.Stop();
            long ololo = stp.ElapsedMilliseconds;
            scene.undoPrepares(); 
            scene.applyMatrix(camera.matrixFromZ, camera.mFromZfD);
            DateTime time2 = DateTime.Now;
            int one = (time2 - time1).Milliseconds;
            one++; 
        }

        private MyColor Sampler(Ray r, out SceneObject obj, Camera camera)
        {
            Intersection hit = Tracer(r, camera);

            if (hit == null)
            {
                obj = null;
                return null;
            }
            obj = hit.figure;
            if (obj is Source || !(ViewSettings.allLight) || (!(ViewSettings.subLight) && (obj is Vector || obj is Edge || obj is Camera)))
            {
                return hit.color; 
            }

            Vector p = hit.point;
            Vector norm = hit.normal;
            double depth = 0;

            foreach (Source source in scene.getSources())
            {
                if (inShadow(p, source))
                    continue;
                Vector ddd = (source.position - p).normalize();
                double cos = norm * ddd;
                depth = source.depth * cos;
            }
            if (depth > 1.0)
                depth = 1.0;
            if (depth < 0.01)
                depth = 0.0;
            MyColor clr = (MyColor)hit.color.Clone();
            clr.SetSaturation(depth);

            return clr;
        }

        private Intersection Tracer(Ray r, SceneObject src)
        {
            Intersection nI = null;

            foreach (var objList in scene.getAllObjs())
                foreach (var sceneObj in objList)
                {
                    Intersection nI1 = sceneObj.isIntersect(r);
                    if (nI1 != null && (nI == null || nI1.distance < nI.distance) && nI1.figure != src)
                        nI = nI1;
                }
            return nI; 
        }        

        private bool inShadow(Vector p, Source light)
        {
            Intersection q = Tracer(new Ray(light.position, p - light.position), light);
            return (q == null) || ((light.position - q.point).getLength2() + eps < (light.position-p).getLength2()); 
        }
    }
}
