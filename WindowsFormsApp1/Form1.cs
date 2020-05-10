using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.ShowDialog();
            //textBox1.Text = openFileDialog1.FileName;
            //Stream fileStream = openFileDialog1.OpenFile();
            //using (StreamReader reader = new StreamReader(fileStream))
            //{
            //    textBox1.Text = reader.ReadLine();
            //}
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void randomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            //Random rand = new Random();
            //int n = rand.Next(1, 10);
            //double[] coefficients = new double[n];
            //for (int i = 0; i < n; i++)
            //{
            //    coefficients[i] = 2*rand.NextDouble()-1;
            //}
            //Data.Polinom p = new Data.Polinom(coefficients);
            //string pName = p.ToString();
            //chart1.Series.Add(pName);
            //chart1.Series[pName].ChartType = SeriesChartType.Spline;
            //foreach (Data.Point point in p.getPoints(-2, 2))
            //{
            //    chart1.Series[pName].Points.AddXY(point.getX(), point.getY());
            //}

            Data.Point[] points = new Data.Point[]
            {
                new Data.Point(0,1),
                new Data.Point(1,3),
                new Data.Point(2,1),
                new Data.Point(3,4),
                new Data.Point(4,2)
            };

            Data.Point[] points1 = new Data.Point[]
            {
                new Data.Point(1,1.0002),
                new Data.Point(2,1.0341),
                new Data.Point(3,0.6),
                new Data.Point(4,0.40105),
                new Data.Point(5,0.1),
                new Data.Point(6,0.23975)
            };


            Data.Point[] points2 = new Data.Point[]
            {
                new Data.Point(1,1),
                new Data.Point(2,3),
                new Data.Point(2.5,3.5538),
                new Data.Point(3,3)
            };

            Data.Point[] points3 = new Data.Point[]
            {
                new Data.Point(1,1),
                new Data.Point(2,8),
                new Data.Point(3,27),
                new Data.Point(4,64),
                new Data.Point(5,125)
            };

            //Data.CubicSpline csplain = new Data.CubicSpline(p.getPoints(-2, 2, 10));
            Data.CubicSpline csplain = new Data.CubicSpline(points3);

            chart1.Series.Add("spline");

            chart1.Series["spline"].ChartType = SeriesChartType.Spline;
            foreach (Data.Point point in csplain.getPoints(100))
            {
                chart1.Series["spline"].Points.AddXY(point.getX(), point.getY());
            }

        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
