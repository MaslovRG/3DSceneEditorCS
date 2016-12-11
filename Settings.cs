using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using _3DSceneEditorCS.Classes; 

namespace _3DSceneEditorCS
{
    public partial class Settings : Form
    {
        private double bRadius;
        private MyColor bColor;
        private bool isOk; 

        public Settings()
        {
            InitializeComponent();
            bRadius = TransferSettings.radius;
            bColor = TransferSettings.color; 
            textBox1.Text = TransferSettings.radius.ToString();
            button1.BackColor = ((MyColorVS)TransferSettings.color).color;
            isOk = false; 
        }

        private void toBackup()
        {
            TransferSettings.radius = bRadius;
            TransferSettings.color = bColor; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK)
                return;
            button1.BackColor = colorDialog1.Color;
            TransferSettings.color = new MyColorVS(colorDialog1.Color); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double nRad;
            if (Double.TryParse(textBox1.Text, out nRad))
            {
                TransferSettings.radius = nRad;
                isOk = true; 
                Close();  
            }
            else
                MessageBox.Show("Радиус точки - вещественное число!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close(); 
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!(isOk))
                toBackup(); 
        }
    }
}
