using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.DataVisualization.Charting.Utilities;

namespace ImageDetector
{
    public partial class Form1 : Form
    {
        
        static int hist_size = 256;
        double Mean_koef = 0.03;  //< Диаграмма или граф

        public Form1()
        {
            InitializeComponent();
        }

        private void Draw_Histogramm_With_Result_in_File(double[] hist,bool isFoto)
        {
            chart1.Series[0].Points.Clear();
            for (int index = 0; index < hist.GetLength(0); index++)
            {
                chart1.Series[0].Points.AddXY(index, hist[index]);
            }
            chart1.Invalidate();
            // Calculate Standard Deviation from the Variance
            //double variance = chart1.DataManipulator.Statistics.Variance("Series1", true);
            //double dispersion = Math.Sqrt(variance);
            
            double mean = chart1.DataManipulator.Statistics.Mean("Series1");
            label2.Text = "Среднее = " + mean.ToString();
            string text;
            text = mean.ToString();
            if (isFoto == true)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\Dropbox\Visual Studio 2012\Projects\ImageDetector\Foto.txt", true))
                {
                    file.WriteLine(text);
                }
            }
            else
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\Dropbox\Visual Studio 2012\Projects\ImageDetector\Graphics.txt", true))
                {
                    file.WriteLine(text);
                }
            }
        }

        private void DrawHistogramm(double[] hist)
        {
            chart1.Series[0].Points.Clear();
            for (int index = 0; index < hist.GetLength(0); index++)
            {
                chart1.Series[0].Points.AddXY(index, hist[index]);
            }
            chart1.Invalidate();
            // Calculate Standard Deviation from the Variance
            //double variance = chart1.DataManipulator.Statistics.Variance("Series1", true);
            //double dispersion = Math.Sqrt(variance);

            double mean = chart1.DataManipulator.Statistics.Mean("Series1");
            label2.Text = "Среднее = " + mean.ToString();
            //if ((ad > Average_derivative_koef) && (mean < Mean_koef))
            if (mean<Mean_koef)
            {
                MessageBox.Show("Это Диаграмма или График");
            }
            else
            {
                MessageBox.Show("Это Фото");
            }
        }


        private double[] GetHistogramm(Bitmap image)
        {
            double[] result = new double[hist_size];
            for (int x = 0; x < image.Width; x++)
                for (int y = 0; y < image.Height; y++)
                {
                    int i = (int)(255 * image.GetPixel(x, y).GetBrightness());
                    result[i]++;
                }

            double max = result.Max();
            for (short i = 0; i < hist_size ; i++)
                result[i] = result[i] / max;

            return result;
        }


        private void button1_Click(object sender, EventArgs q)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                double[] hist;
                Image image = Image.FromFile(ofd.FileName);
                hist = GetHistogramm(image as Bitmap);
                Draw_Histogramm_With_Result_in_File(hist,true);
                Bitmap bmp = new Bitmap(ofd.FileName);
                pictureBox1.Image = bmp;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                double[] hist;
                Image image = Image.FromFile(ofd.FileName);
                hist = GetHistogramm(image as Bitmap);
                Draw_Histogramm_With_Result_in_File(hist,false);
                Bitmap bmp = new Bitmap(ofd.FileName);
                pictureBox1.Image = bmp;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                double[] hist;
                Image image = Image.FromFile(ofd.FileName);
                hist = GetHistogramm(image as Bitmap);
                Bitmap bmp = new Bitmap(ofd.FileName);
                pictureBox1.Image = bmp;
                DrawHistogramm(hist);
            }
        }

    }
}
