using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace _3DSceneEditorCS.Classes
{
    public class Scene : SceneObject
    {
        protected List<SceneObject> figures;
        protected List<SceneObject> cameras;
        protected List<SceneObject> sources;
        protected List<List<SceneObject>> allObjs; 
        protected List<SceneObject>.Enumerator activeCamera; 

        public Scene()
        {
            figures = new List<SceneObject>();
            cameras = new List<SceneObject>();
            sources = new List<SceneObject>();
            allObjs = new List<List<SceneObject>>();
            allObjs.Add(figures);
            allObjs.Add(cameras);
            allObjs.Add(sources); 

            activeCamera = cameras.GetEnumerator(); 
        }

        public Scene(Figure fig, Camera cam, Source source)
        {
            figures = new List<SceneObject>();
            cameras = new List<SceneObject>();
            sources = new List<SceneObject>();
            allObjs = new List<List<SceneObject>>();
            allObjs.Add(figures);
            allObjs.Add(cameras);
            allObjs.Add(sources);

            figures.Add(fig);
            cameras.Add(cam);
            sources.Add(source);
            activeCamera = cameras.GetEnumerator(); 
            activeCamera.MoveNext();
        }

        public Scene(Scene oldScene)
        {
            figures = new List<SceneObject>(oldScene.getFigures());
            cameras = new List<SceneObject>(oldScene.getCameras());
            sources = new List<SceneObject>(oldScene.getSources());
            allObjs = new List<List<SceneObject>>(oldScene.getAllObjs()); 

            activeCamera = cameras.GetEnumerator();
            activeCamera.MoveNext(); 
        }

        public void addScene(Scene nScene)
        {
            figures.AddRange(nScene.getFigures());
            cameras.AddRange(nScene.getCameras());
            sources.AddRange(nScene.getSources());

            activeCamera = cameras.GetEnumerator();
            activeCamera.MoveNext(); 
        }

        public void addFigure(Figure figure)
        {
            figures.Add(figure); 
        }

        public void addCamera(Camera camera)
        {
            cameras.Add(camera);
            activeCamera = cameras.GetEnumerator();
            activeCamera.MoveNext(); 
        }

        public void addSource(Source source)
        {
            sources.Add(source); 
        }

        public Camera getActiveCamera()
        {
            return (Camera)activeCamera.Current; 
        }

        public List<SceneObject> getFigures()
        {
            return figures; 
        }

        public List<SceneObject> getCameras()
        {
            return cameras; 
        }

        public List<SceneObject> getSources()
        {
            return sources; 
        }

        public List<List<SceneObject>> getAllObjs()
        {
            return allObjs; 
        }

        public void clearScene()
        {
            figures = new List<SceneObject>();
            cameras = new List<SceneObject>();
            sources = new List<SceneObject>();
            allObjs = new List<List<SceneObject>>();
            allObjs.Add(figures);
            allObjs.Add(cameras);
            allObjs.Add(sources);

            activeCamera = cameras.GetEnumerator(); 
        }

        public override void applyMatrix(Matrix matrixP, Matrix matrixV)
        {
            foreach (var obj in allObjs)
            {
                foreach (var sceneObj in obj)
                    sceneObj.applyMatrix(matrixP, matrixV);
            }
        }

        public override void makePrepares()
        {
            foreach (var obj in allObjs)
            {
                foreach (var sceneObj in obj)
                    sceneObj.makePrepares();
            }
        }

        public override void undoPrepares()
        {
            foreach (var obj in allObjs)
            {
                foreach (var sceneObj in obj)
                    sceneObj.undoPrepares();
            }
        }
    }
}
