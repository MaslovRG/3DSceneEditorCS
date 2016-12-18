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
        private double bVRadius, bERadius, bCRadius, bSRadius;
        private MyColor bVColor, bEColor, bCColor, bSColor;
        private bool isOk; 

        public Settings()
        {
            InitializeComponent();
            bVRadius = TransferSettings.vradius;
            bVColor = TransferSettings.vcolor;
            bERadius = TransferSettings.eradius; 
            bEColor = TransferSettings.ecolor;
            bCRadius = TransferSettings.cradius; 
            bCColor = TransferSettings.ccolor;
            bSRadius = TransferSettings.sradius; 
            bSColor = TransferSettings.scolor;
            textBox1.Text = TransferSettings.vradius.ToString();
            textBox2.Text = TransferSettings.sradius.ToString();
            textBox3.Text = TransferSettings.cradius.ToString();
            textBox4.Text = TransferSettings.eradius.ToString(); 
            pictureBox1.BackColor = ((MyColorVS)TransferSettings.vcolor).color;
            pictureBox2.BackColor = ((MyColorVS)TransferSettings.scolor).color;
            pictureBox3.BackColor = ((MyColorVS)TransferSettings.ccolor).color;
            pictureBox4.BackColor = ((MyColorVS)TransferSettings.ecolor).color; 
            isOk = false; 
        }

        private void toBackup()
        {
            TransferSettings.vradius = bVRadius;
            TransferSettings.vcolor = bVColor;
            TransferSettings.sradius = bSRadius;
            TransferSettings.scolor = bSColor;
            TransferSettings.cradius = bCRadius;
            TransferSettings.ccolor = bCColor;
            TransferSettings.eradius = bERadius;
            TransferSettings.ecolor = bEColor;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                TransferSettings.vradius = double.Parse(textBox1.Text);
                TransferSettings.sradius = double.Parse(textBox2.Text);
                TransferSettings.cradius = double.Parse(textBox3.Text);
                TransferSettings.eradius = double.Parse(textBox4.Text);
                isOk = true; 
                Close();
            }
            catch
            {
                MessageBox.Show("Радиуc - вещественное число!"); 
            }            
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK)
                return;
            pictureBox1.BackColor = colorDialog1.Color;
            TransferSettings.vcolor = new MyColorVS(colorDialog1.Color);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK)
                return;
            pictureBox2.BackColor = colorDialog1.Color;
            TransferSettings.scolor = new MyColorVS(colorDialog1.Color); 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK)
                return;
            pictureBox3.BackColor = colorDialog1.Color;
            TransferSettings.ccolor = new MyColorVS(colorDialog1.Color); 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK)
                return;
            pictureBox4.BackColor = colorDialog1.Color;
            TransferSettings.ecolor = new MyColorVS(colorDialog1.Color); 
        }
    }
}
