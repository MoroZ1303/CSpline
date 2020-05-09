using MathNet.Numerics.Differentiation;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class Data
    {
        public class Point
        {
            double x;
            double y;
            public Point(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
            public double getX() { return x; }
            public double getY() { return y; }
        }

        public List<Point> getPointsFromFile(System.IO.Stream stream)
        {
            List<Point> points = new List<Point>();
            using (StreamReader reader = new StreamReader(stream))
            {
                string l;

                while ((l = reader.ReadLine()) != null)
                {
                    string[] parts = l.Split(new char[] { ',' });
                    double x = Double.Parse(parts[0]);
                    double y = Double.Parse(parts[0]);
                    points.Add(new Point(x, y));
                }
            }

            return points;
        }

        public List<Point> buildCubicSpline(List<Point> initialPoints, int totalPoints)
        {

            Dictionary<double, double> dict = new Dictionary<double, double>();
            foreach (Point point in initialPoints)
            {
                if (dict.ContainsKey(point.getX()))
                {
                    throw new Exception();
                }
                dict[point.getX()] = point.getY();
            }

            List<Point> sorted = dict.Select(x => new Point(x.Key, x.Value)).ToList();
            sorted.Sort((a, b) => a.getX().CompareTo(b.getY()));
            Point[] points = sorted.ToArray();

            for (int i = 1; i < points.Length; i++)
            {

            }
            return null;
        }

        public class Polinom
        {
            private double[] coefficients;
            public Polinom(double[] coefficients)
            {
                this.coefficients = coefficients;
            }
            public double f(double x)
            {
                double res = 0;
                double xx = 1;
                foreach (double c in coefficients)
                {
                    res += c * xx;
                    xx *= x;
                }
                return res;
            }

            public Point[] getPoints(double start, double end, int numberOfPoints = 100)
            {
                Point[] res = new Point[numberOfPoints];
                double step = (end - start) / numberOfPoints;
                for (int i = 0; i < numberOfPoints; i++)
                {
                    double x = start + i * step;
                    res[i] = new Point(x, f(x));
                }

                return res;
            }


            public new string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("f(x) = ");
                for (int i = coefficients.Length - 1; i > 0; i--)
                {
                    double coeff = coefficients[i];

                    if (coeff == 0)
                        continue;

                    if (i != coefficients.Length - 1)
                    {
                        if (coeff >= 0)
                            sb.Append(" + ");
                        else
                            sb.Append(" - ");
                    }
                    sb.Append(Math.Abs(coeff).ToString("0.0"));
                    sb.Append("*x^").Append(i);
                }

                if (coefficients.Length > 0 && coefficients[0] != 0)
                {
                    if (coefficients[0] >= 0)
                        if (coefficients.Length > 1)
                            sb.Append(" + ");
                        else
                            sb.Append(" - ");
                    sb.Append(Math.Abs(coefficients[0]).ToString("0.0"));
                }
                return sb.ToString();
            }
        }

        class TriDiagonaMatrixElement
        {
            public double a;
            public double b;
            public double c;
        }

        class TriDiagonalMatrix
        {
            private TriDiagonaMatrixElement[] elements;
            public TriDiagonalMatrix(int n)
            {
                elements = new TriDiagonaMatrixElement[n];
            }
            public TriDiagonaMatrixElement this [int i] {
                get => elements[i];
            }

            public int Length
            {
                get => elements.Length;
            } 
        }

        public class CubicSpline
        {
            private double start;
            private double end;
            private Tuple<double, double>[] ranges;
            private Point [] points;
            private Polinom[] splines;
            public CubicSpline(Point[] inputPoints)
            {
                buildRanges(inputPoints);
                splines = new Polinom[ranges.Length];
                double[] h = new double[points.Length];
                for (int i = 1; i < points.Length; i++)
                {
                    h[i] = points[i].getX() - points[i-1].getX();
                }

                int N = ranges.Length;
                TriDiagonalMatrix matrix = new TriDiagonalMatrix(N);
                double[] F = new double[N];
                // Построение системы динейных уравнений относительно coeff_c[i],  коэффициент при x^2
                
                matrix[0].a = 0;
                matrix[0].b = h[1];
                matrix[0].c = 2 * (h[0] + h[1]);

                matrix[N - 1].a = h[N - 2];
                matrix[N - 1].b = 0;
                matrix[N-1].c = 2 * (h[N-2] + h[N-1]);

                for (int i = 1; i < N - 1; i++)
                {
                    matrix[i].a = h[i];
                    matrix[i].c = 2 * (h[i] + h[i + 1]);
                    matrix[i].b = h[i + 1];
                    F[i] = 3 * ((points[i + 1].getY() - points[i].getY()) / h[i + 1] -
                        (points[i].getY() - points[i - 1].getY()) / h[i]);
                }

                // Решение методом прогонки, находим coeff_c[i]

                double[] alpha = new double[points.Length];
                double[] beta = new double[points.Length];
                double[] x = new double[points.Length];
                alpha[1] = -B[0] / C[0];
                beta[1] = F[0] / C[0];
                for (int i = 1; i < points.Length-1; i++)
                {
                    double t = A[i - 1] * alpha[i - 1] + C[i - 1];
                    alpha[i] = B[i - 1] / t;
                    beta[i] = (F[i - 1] - A[i - 1] * B[i - 1]) / t;
                }

                coeff_c[coeff_c]
                for (int i = points.Length - 2; i >= 0; i--)
                {
                    coeff_c[i] = alpha[i + 1] * x[i + 1] + beta[i + 1];
                }

                // Вычисляем оставшмеся коэффициетны и создаем сплайны
                for (int i = 1; i < points.Length-1; i++)
                {
                    double a = points[i].getX(); // свободный коэффициент
                    double d = coeff_c[i] - coeff_c[i - 1]; // коэффициент при x^3
                    double b = (points[i].getX() - points[i - 1].getX()) / h[i] + (2 * coeff_c[i] + coeff_c[i - 1]) / 3; // коэффициент при x^1
                    splines[i] = new Polinom(new double[] { a, b, coeff_c[i], d });
                }
            }

            // Строим интервалы между точками
            private void buildRanges(Point[] pts)
            {
                // Исключаем повторяющуеся точки
                Dictionary<double, double> dict = new Dictionary<double, double>();
                for (int i = 0; i < pts.Length; i++)
                {
                    if (dict.ContainsKey(pts[i].getX()))
                    {
                        // Проверяем если точка дубликат одной из обработаных
                        // Если дубликат то пропускаем
                        if (dict[pts[i].getX()] == pts[i].getY())
                            continue;

                        // Для одного и того же X существует 2 точки с различными Y 
                        throw new Exception();
                    }
                    // Добавляем точку в словарь
                    dict[pts[i].getX()] = pts[i].getY();
                }

                // Преобразуем словарь в список и сортируем по убыванию X
                List<Point> sorted = dict.Select(x => new Point(x.Key, x.Value)).ToList();
                sorted.Sort((a, b) => a.getX().CompareTo(b.getY()));

                // Преобразуем список в массив и строим интервалы
                points = sorted.ToArray();
                ranges = new Tuple<double, double>[points.Length - 1];
                for (int i = 0; i < points.Length-1; i++)
                {
                    ranges[i] = new Tuple<double, double>(points[i].getX(), points[i + 1].getX());
                }

                start = points[0].getX();
                end = points[points.Length - 1].getX();
            }

            int findRange(double point)
            {
                for (int i = 0; i < ranges.Length; i++)
                {
                    if (point >= ranges[i].Item1 && point <= ranges[i].Item1)
                        return i;
                }
                return -1;
            }

            public Point[] getPoints(int numberOfPoints = 100)
            {
                double step = (end - start) / numberOfPoints;
                Point[] res = new Point[numberOfPoints]; 
                for (int i = 0; i < numberOfPoints; i++)
                {
                    double x = start + i * step;
                    if (i == numberOfPoints - 1)
                        x = end;
                    int iRange = findRange(x);
                    double y = splines[iRange].f(x - ranges[i].Item1);
                    res[i] = new Point(x, y);
                }
                return res;
            }
        }
    }
}
