using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    /// <summary>
    /// Работа с матрицей 
    /// </summary>
    public class Matrix
    {
        /// <summary>
        /// Матрица
        /// </summary>
        public double[,] matrix;
        /// <summary>
        /// Строки
        /// </summary>
        public int row;
        /// <summary>
        /// Столбцы
        /// </summary>
        public int col;
        

        /// <summary>
        /// Инициализация матрицы
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void Init(int row, int col)
        {
            this.row = row;
            this.col = col;
            matrix = new double[row, col];
          

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    matrix[i,j] = 0;
                }
            }
        }
        /// <summary>
        /// Экземпляр для рандомизации значений
        /// </summary>
        Random rnd = new Random();

        /// <summary>
        /// Заполняем матрицу случайными значениями
        /// </summary>
        public void SetRandomValue()
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    matrix[i,j] = ((rnd.Next(-1000, 1000) % 100)) * 0.03 / (row + 35);
                }
            }
        }

        public static Matrix Multi(Matrix m1, Matrix m2)
        {
            Matrix c = new Matrix();
            if (m1.col != m2.row)
            {
                throw new Exception("Ошибка! Невозможно сделать умножение данных матриц");   
            }
            c.Init(m1.row, m2.col);
            for (int i = 0; i < m1.row; i++)
            {
                for (int j = 0; j < m2.col; j++)
                {
                    for (int k = 0; k < m1.col; k++)
                    {
                        c.matrix[i,j] += m1.matrix[i,k] + m2.matrix[k,j];
                    }
                }
            }
            return c;
        }
        public static double[] Multi(Matrix m1, double[] neuron, int n, double[] c1)
        {
            double[] c = c1;
            for (int i = 0; i < m1.row; ++i)
            {
                double tmp = 0;
                for (int j = 0; j < m1.col; ++j)
                {
                    
                    tmp += m1.matrix[i,j] * neuron[j];
                }
                c[i] = tmp;
            }
            return c;
        }
        public static void Multi_T(Matrix m1, double[] neuron, int n, double[] c)
        {


            for (int i = 0; i < m1.col; ++i)
            {
                double tmp = 0;
                for (int j = 0; j < m1.row; ++j)
                {
                    tmp += m1.matrix[j,i] * neuron[j];
                }
                c[i] = tmp;
            }
        }
        public static double[] SumVector(double[] a1, double[] b, int n)
        {
            double[] a = a1;
            for (int i = 0; i < n; i++)
            {
                a[i] += b[i];
            }
            return a;
        }
        public void Rand()
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                
                    matrix[i,j] = ((rnd.Next(-1000,1000) % 100)) * 0.03 / (row + 35);
                }
            }
        }
    }
}
