using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO; 
using _3DSceneEditorCS.Classes; 

namespace _3DSceneEditorCS
{
    public partial class Form1 : Form
    {
        private CanvasVS workCanvas;
        private Scene workScene;
        private RayTracer workRT; 
        delegate void loadOperation(ReaderManager manager); 

        public Form1()
        {
            InitializeComponent();
            int w = pictureBox1.Width;
            int h = pictureBox1.Height;
            SceneObject[,] workMassive = new SceneObject[w, h];
            Bitmap workImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            workCanvas = new CanvasVS(workImage, pictureBox1.BackColor, workMassive);
            workScene = new Scene();
            workRT = new RayTracer(workScene, workCanvas);
            Vector.radius = 5;
            //Vector.color = new MyColorVS(Color.AntiqueWhite); 
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Курсовая работа. Редактор трёхмерной сцены v0.01a");
            MyColorVS clrp = new MyColorVS(Color.AntiqueWhite); 
            Vector p1 = new Vector(-180, -90, -70, clrp);
            Vector p2 = new Vector(0, 180, -70, clrp);
            Vector p3 = new Vector(180, -90, -70, clrp); 
            Edge[] em = new Edge[] { new Edge(p1, p2), new Edge(p2, p3), new Edge(p3, p1) };
            Vector[] vm = new Vector[] { p1, p2, p3 }; 
            MyColorVS clr = new MyColorVS(Color.Violet); 
            //Polygon pp = new Polygon(em, vm, clr);
            Triangle pp = new Triangle(vm, clr); 
            workScene.addFigure(pp);
            workRT.drawScene(checkBox1.Checked);
            pictureBox1.Image = workCanvas.canv;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            workRT.drawScene(checkBox1.Checked);     
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Point p = pictureBox1.PointToClient(System.Windows.Forms.Cursor.Position);
            if (workCanvas.mas[p.X, p.Y] != null)
            {
                SceneObject scsc = (SceneObject)workCanvas.mas[p.X, p.Y];
                MyColorVS clrsc = (MyColorVS)scsc.color;
                MessageBox.Show(clrsc.color.ToString()); 
            }
            else
                MessageBox.Show("null"); 
        }

        private void loadFromStream(loadOperation operation)
        {
            StreamReader stream = null;
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = "c:\\Projects\\sem5\\crsprj\\objects";
            openDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openDialog.FilterIndex = 2;
            openDialog.RestoreDirectory = true;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((stream = new StreamReader(openDialog.OpenFile())) != null)
                    {
                        ReaderVS reader = new ReaderVS(stream);
                        char[] section = { ' ' };
                        ReaderManager manager = new ReaderManager(reader, section);
                        label1.Text = "loadfrom"; 
                        operation(manager);
                        stream.Close(); 
                        label1.Text = "draw"; 
                        workRT.drawScene(checkBox1.Checked);
                        pictureBox1.Image = workCanvas.canv;
                        textBox1.Text = "";
                        label1.Text = "complete"; 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Невозможно загрузить из файла из-за ошибки!\nТип ошибки: " + ex.Message);
                    //textBox1.Text = "";
                    stream.Close(); 
                }

            }
        }

        void loadScene(ReaderManager manager)
        {
            workScene.clearScene(); 
            workScene.addScene(manager.getScene()); 
        }

        void addScene(ReaderManager manager)
        {
            workScene.addScene(manager.getScene()); 
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadOperation operation = new loadOperation(loadScene);
            loadFromStream(operation); 
        }

        private void добавитьКТекущейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadOperation operation = new loadOperation(addScene);
            loadFromStream(operation); 
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            workScene.clearScene();
            workRT.drawScene(checkBox1.Checked);
            pictureBox1.Image = workCanvas.canv; 
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close(); 
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            workRT.drawScene(checkBox1.Checked);
            pictureBox1.Image = workCanvas.canv; 
        }
    }
}
