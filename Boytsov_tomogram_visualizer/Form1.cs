using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Boytsov_tomogram_visualizer
{
    public partial class Form1 : Form
    {
        int FrameCount;
        DateTime NextFPSUpdate = DateTime.Now.AddSeconds(1);

        bool loaded = false;
        bool needReload = false;
        bool Quads1 = false;
        bool Texture1 = false;
        Bin bin = new Bin();
        View view = new View();
        int currentLayer = 1;
        int min1 = 0;
        int width1 = 1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;

        }
        private void открытьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string str = dialog.FileName;
                bin.readBIN(str);
                view.SetupView(glControl1.Width, glControl1.Height);
                loaded = true;
                glControl1.Invalidate();
            }
        }
        void displayFPS()
        {
            if (DateTime.Now >= NextFPSUpdate)
            {
                this.Text = String.Format("CT Visualizer (fps={0})", FrameCount);

                NextFPSUpdate = DateTime.Now.AddSeconds(1);
                FrameCount = 0;
            }
            FrameCount++;
        }

        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                displayFPS();
                glControl1.Invalidate();
            }
        }
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (loaded)
            {
                if (Quads1 == true)
                {
                    view.DrawQuads(currentLayer, min1, width1);
                    //Texture1 = false;
                }
                if (Texture1 == true)
                {
                    // Quads1 = false;
                    if (needReload)
                    {
                        view.generatetextureImage(currentLayer, min1, width1);
                        view.Load2DTexture();
                        needReload = false;

                    }
                    view.DrawTexture();
                }

                glControl1.SwapBuffers();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            min1 = trackBar1.Value * 25;
            needReload = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Quads1 = true;
            Texture1 = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Texture1 = true;
            Quads1 = false;
        }

        private void trackBar2_Scroll_1(object sender, EventArgs e)
        {
            width1 = trackBar2.Value * 350;
            needReload = true;
        }
    }
}