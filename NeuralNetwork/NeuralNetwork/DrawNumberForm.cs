using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetwork
{
    public partial class DrawNumberForm : Form
    {
        public DrawNumberForm(Network net)
        {
            InitializeComponent();
            network = net;
        }

        Graphics gr = null;
        Bitmap bitmap;
        private void DrawNumberForm_Load(object sender, EventArgs e)
        {
            bitmap = new Bitmap(280, 280);
            gr = Graphics.FromImage(bitmap);
            gr.Clear(Color.Black);
            pictureBox1.Image = bitmap;



        }
        double[] pixels;
        Network network;
        private void button1_Click(object sender, EventArgs e)
        {
            gr.Clear(Color.Black);
            pictureBox1.Image = bitmap;
        }

        private void pictureBox1_Move(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //   e.Graphics.DrawEllipse(new Pen(Brushes.Black), new Rectangle(x,y, 15, 15));
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        int x;
        int y;

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) { return; }

            x = e.X;
            y = e.Y;
            gr.FillEllipse(Brushes.White, new Rectangle(x, y, 15, 15));
            pictureBox1.Image = bitmap;
            Bitmap newImg = new Bitmap(bitmap, 28, 28);
            pixels = new double[28 * 28];
            int index = 0;

            for (int y = 0; y < 28; y++)
            {
                for (int x = 0; x < 28; x++)
                {
                    Color color = newImg.GetPixel(x, y);

                    pixels[index] = double.Parse(color.B.ToString()) / 255d;
                    index++;
                }
            }
            pictureBox2.Image = newImg;
            network.SetInput(pixels);
            var res = network.ForwardFeed();

            label1.Text = string.Join("\n", res);

        }

        private void button2_Click(object sender, EventArgs e)
        {



        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int epoch = 0;
            double answer = double.Parse(textBox1.Text);

            double result = -1;
            while (answer != result)
            {
                network.SetInput(pixels);
                result = network.ForwardFeed();

                if (result != answer)
                {
                    network.BackPropogation(answer);
                    network.WeightsUpdater(0.15 * Math.Exp(-epoch / 20));
                }
                else
                {
                    ; label2.Text = "Успешно! Эпохи: " + epoch.ToString();
                    network.SaveWeights();
                    break;
                }
                if (epoch == 20)
                {
                    label2.Text = "Не получилось!";
                    break;
                }
                epoch++;
            }


        }
    }

}
