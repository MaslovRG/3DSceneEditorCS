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
        private Point one, two;
        private SceneObject activeObj; 

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
            Vector.vradius = 1;
            Vector.vcolor = new MyColorVS(Color.AntiqueWhite);
            Edge.eradius = 1;
            Edge.ecolor = new MyColorVS(Color.Red);
            Source.sradius = 10;
            Source.scolor = new MyColorVS(Color.Yellow);
            Camera.cradius = 10;
            Camera.ccolor = new MyColorVS(Color.Green);
            dataGridView1.RowCount = 5;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {                    
                    if (i == j)
                        dataGridView1.Rows[i].Cells[j].Value = "1"; 
                    else
                        dataGridView1.Rows[i].Cells[j].Value = "0";
                }
            dataGridView1.Rows[3].Cells[3].ReadOnly = true;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.SelectedIndex = 0; 
            activeObj = null; 
            setViewSettings(); 
        }

        private void drawScene(SceneObject aObj)
        {
            workRT.drawScene(checkBox1.Checked);
            pictureBox1.Image = workCanvas.canv;
            comboBox2.Items.Clear();
            comboBox1.Items.Clear();
            foreach (var objectList in workScene.getAllObjs())
                foreach (var obj in objectList)
                    comboBox1.Items.Add(obj);
            changeActiveObject(aObj); 
        }

        private void changeActiveObject(SceneObject obj)
        {
            activeObj = obj;
            comboBox2.Items.Clear(); 
            if (obj == null)
            {
                label3.Text = "ничего";
                return;
            }
            string type = activeObj.GetType().ToString(); 
            switch (type)
            {
                case "_3DSceneEditorCS.Classes.Camera":
                    break;
                case "_3DSceneEditorCS.Classes.Cone":
                    break;
                case "_3DSceneEditorCS.Classes.Сylinder":
                    break;
                case "_3DSceneEditorCS.Classes.Edge":
                    break;
                case "_3DSceneEditorCS.Classes.Polygon":
                    break;
                case "_3DSceneEditorCS.Classes.Polyhedron":
                    break;
                case "_3DSceneEditorCS.Classes.Source":
                    break;
                case "_3DSceneEditorCS.Classes.Sphere":
                    break;
                case "_3DSceneEditorCS.Classes.Vector":
                    break;
                default:
                    break;
            }
            label3.Text = activeObj.ToString(); 
            SceneObject[] subs = activeObj.getSubs(); 
            if (subs != null)
            {
                foreach (var sub in subs)
                    comboBox2.Items.Add(sub); 
            }
            
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Редактор трёхмерной сцены v0.5 \n\nАвтор: Маслов А.Д., ИУ7-51\nРуководитель: Куров А.В.", "Курсовая работа");

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            drawScene(null);    
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Point p = pictureBox1.PointToClient(Cursor.Position);
            if (workCanvas.mas[p.X, p.Y] != null)
            {
                activeObj = (SceneObject)workCanvas.mas[p.X, p.Y];
                changeActiveObject(activeObj); 
            }
            else
                changeActiveObject(null); 
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
                        drawScene(null); 
                        label1.Text = "complete"; 
                    }
                }
                catch
                {
                    MessageBox.Show("Невозможно загрузить из файла из-за ошибки! Выберите корректный файл!");
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
            drawScene(null); 
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close(); 
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransferSettings.vradius = Vector.vradius;
            TransferSettings.vcolor = Vector.vcolor;
            TransferSettings.sradius = Source.sradius;
            TransferSettings.scolor = Source.scolor;
            TransferSettings.cradius = Camera.cradius;
            TransferSettings.ccolor = Camera.ccolor;
            TransferSettings.eradius = Edge.eradius;
            TransferSettings.ecolor = Edge.ecolor; 
            Settings setForm = new Settings();
            setForm.ShowDialog();
            Vector.vradius = TransferSettings.vradius;
            Vector.vcolor = TransferSettings.vcolor;
            Source.sradius = TransferSettings.sradius;
            Source.scolor = TransferSettings.scolor;
            Camera.cradius = TransferSettings.cradius;
            Camera.ccolor = TransferSettings.ccolor;
            Edge.eradius = TransferSettings.eradius;
            Edge.ecolor = TransferSettings.ecolor;
            drawScene(activeObj); 
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            one = pictureBox1.PointToClient(Cursor.Position); 
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            two = pictureBox1.PointToClient(Cursor.Position);
            //MessageBox.Show(one.X.ToString() + " " + two.X.ToString()); 
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            /*Point pp = pictureBox1.PointToClient(Cursor.Position);
            pp.X += 5;
            pp.Y += 5; 
            toolTip1.Show("ololo", this.pictureBox1, pp);*/
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Point p = pictureBox1.PointToClient(Cursor.Position);
            if (workCanvas.mas[p.X, p.Y] != null)
            {
                SceneObject scsc = (SceneObject)workCanvas.mas[p.X, p.Y];
                /*MyColorVS clrsc = (MyColorVS)scsc.color;
                MessageBox.Show(clrsc.color.ToString());*/
            }
            else
                MessageBox.Show("null"); 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (activeObj != null)
            {
                try
                {
                    Matrix matrix = new Matrix();
                    for (int i = 0; i < 4; i++)
                        for (int j = 1; j <= 4; j++)
                        {
                            matrix.data[i * 4 + j - 1] = double.Parse((string)dataGridView1.Rows[i].Cells[j-1].Value);
                        }
                    activeObj.applyMatrix(matrix, new Matrix(1)); 
                    drawScene(activeObj); 
                }
                catch
                {
                    MessageBox.Show("Введите корректные значения!"); 
                }
            }
            else
            {
                MessageBox.Show("Выберите объект!"); 
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeActiveObject((SceneObject)comboBox1.SelectedItem); 
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeActiveObject((SceneObject)comboBox2.SelectedItem); 
        }

        private void setViewSettings()
        {
            ViewSettings.interEdge = checkBox1.Checked;
            ViewSettings.interPoint = checkBox2.Checked;
            ViewSettings.subLight = checkBox3.Checked;
            ViewSettings.allLight = checkBox4.Checked; 
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            setViewSettings();
            drawScene(null); 
        }        

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            setViewSettings();
            drawScene(null); 
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            setViewSettings();
            drawScene(null);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox4.Checked)
            {
                checkBox3.Checked = false;
                checkBox3.Enabled = false;
            }
            else
                checkBox3.Enabled = true; 
            setViewSettings();
            drawScene(null); 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (i == j)
                        dataGridView1.Rows[i].Cells[j].Value = "1";
                    else
                        dataGridView1.Rows[i].Cells[j].Value = "0";
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Camera cam = workScene.getActiveCamera();
            cam.setMatrixes();
            cam.direction.applyMatrix(cam.mToZfD, null); 
            cam.direction.y = cam.direction.z * 0.1;
            cam.direction.applyMatrix(cam.mFromZfD, null);
            drawScene(activeObj); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Camera cam = workScene.getActiveCamera();
            cam.setMatrixes();
            cam.direction.applyMatrix(cam.mToZfD, null);
            cam.direction.y = cam.direction.z * (-0.1);
            cam.direction.applyMatrix(cam.mFromZfD, null);
            drawScene(activeObj); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Camera cam = workScene.getActiveCamera();
            cam.setMatrixes();
            cam.direction.applyMatrix(cam.mToZfD, null);
            cam.direction.x = cam.direction.z * 0.1;
            cam.direction.applyMatrix(cam.mFromZfD, null);
            drawScene(activeObj); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Camera cam = workScene.getActiveCamera();
            cam.setMatrixes();
            cam.direction.applyMatrix(cam.mToZfD, null);
            cam.direction.x = cam.direction.z * (-0.1);
            cam.direction.applyMatrix(cam.mFromZfD, null);
            drawScene(activeObj); 
        }

        private void resetTransform()
        {
            string dStr;
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    textBox5.Enabled = false;
                    textBox6.Enabled = false;
                    textBox7.Enabled = false;
                    label10.Text = "";
                    label11.Text = "";
                    label12.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    textBox7.Text = "";
                    textBox2.Text = "0";
                    textBox3.Text = "0";
                    textBox4.Text = "0";
                    dStr = "Изменение ";
                    label7.Text = dStr + "X";
                    label8.Text = dStr + "Y";
                    label9.Text = dStr + "Z";
                    break;
                case 1:
                    textBox5.Enabled = true;
                    textBox6.Enabled = true;
                    textBox7.Enabled = true;
                    label10.Text = "X центра";
                    label11.Text = "Y центра";
                    label12.Text = "Z центра";
                    textBox5.Text = "0";
                    textBox6.Text = "0";
                    textBox7.Text = "0";
                    textBox2.Text = "1";
                    textBox3.Text = "1";
                    textBox4.Text = "1";
                    dStr = "Коэффициент по ";
                    label7.Text = dStr + "X";
                    label8.Text = dStr + "Y";
                    label9.Text = dStr + "Z";
                    break;
                case 2:
                    textBox5.Enabled = true;
                    textBox6.Enabled = true;
                    textBox7.Enabled = true;
                    label10.Text = "X центра";
                    label11.Text = "Y центра";
                    label12.Text = "Z центра";
                    textBox5.Text = "0";
                    textBox6.Text = "0";
                    textBox7.Text = "0";
                    textBox2.Text = "0";
                    textBox3.Text = "0";
                    textBox4.Text = "0";
                    dStr = "Угол в градусах по ";
                    label7.Text = dStr + "X";
                    label8.Text = dStr + "Y";
                    label9.Text = dStr + "Z";
                    break;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetTransform(); 
        }

        private void button8_Click(object sender, EventArgs e)
        {
            resetTransform(); 
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (activeObj != null)
            {
                try
                {
                    Matrix operation, to0, from0; 
                    switch (comboBox3.SelectedIndex)
                    {
                        case 0:
                            double dx = double.Parse(textBox2.Text);
                            double dy = double.Parse(textBox3.Text);
                            double dz = double.Parse(textBox4.Text);
                            operation = Matrix.getMove(dx, dy, dz); 
                            break;
                        case 1:
                            double kx = double.Parse(textBox2.Text);
                            double ky = double.Parse(textBox3.Text);
                            double kz = double.Parse(textBox4.Text);
                            double cmx = double.Parse(textBox5.Text);
                            double cmy = double.Parse(textBox6.Text);
                            double cmz = double.Parse(textBox7.Text);
                            to0 = Matrix.getMove(-cmx, -cmy, -cmz);
                            from0 = Matrix.getMove(cmx, cmy, cmz); 
                            operation = Matrix.getScale(kx, ky, kz); 
                            break; 
                        case 2:
                            double angleX = double.Parse(textBox2.Text) * Math.PI / 180;
                            double angleY = double.Parse(textBox3.Text) * Math.PI / 180;
                            double angleZ = double.Parse(textBox4.Text) * Math.PI / 180;
                            double crx = double.Parse(textBox5.Text);
                            double cry = double.Parse(textBox6.Text);
                            double crz = double.Parse(textBox7.Text);
                            to0 = Matrix.getMove(-crx, -cry, -crz);
                            from0 = Matrix.getMove(crx, cry, crz); 
                            operation = to0 * Matrix.matrixRotateXGet(angleX)
                                * Matrix.matrixRotateYGet(angleY)
                                * Matrix.matrixRotateZGet(angleZ) * from0; 
                            break; 
                        default:
                            operation = null;
                            break; 
                    }
                    activeObj.applyMatrix(operation, null);
                    drawScene(activeObj); 
                }
                catch
                {
                    MessageBox.Show("Введите корректные значения!"); 
                }
            }
            else
            {
                MessageBox.Show("Выберите объект!"); 
            }
        }

        

    }
}
