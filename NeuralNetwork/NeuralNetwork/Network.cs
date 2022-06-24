using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Network
    {
        /// <summary>
        /// Количество Слоёв
        /// </summary>
        int L;
        /// <summary>
        /// Количество нейронов на каждом слое
        /// </summary>
        int[] Size;
        /// <summary>
        /// Активационная функция
        /// </summary>
        ActivateFunction ActivateFunction = new ActivateFunction();
        /// <summary>
        /// Матрица весов
        /// </summary>
        Matrix[] Weights;
        /// <summary>
        /// Веса смещения
        /// </summary>
        double[][] Bios;
        /// <summary>
        /// Значения нейронов
        /// </summary>
        double[][] Neurons_val;
        /// <summary>
        /// Ошибки для нейронов
        /// </summary>
        double[][] Neurons_err;
        /// <summary>
        /// Значение для нейронов смещения
        /// </summary>
        double[] Bios_val;

    
        public struct DataNetwork
        {
            /// <summary>
            /// Количество слоёв 
            /// </summary>
            public int L;
            /// <summary>
            /// Размер слоёв
            /// </summary>
            public int[] size;
        }

        public void Init(DataNetwork data)
        {


            Random rnd = new Random();
            L = data.L;
            Size = new int[L];
            for (int i = 0; i < L; i++)
            {
                Size[i] = data.size[i];
            }
            Weights = new Matrix[L - 1];
            Bios = new double[L - 1][];
            for (int i = 0; i < L - 1; i++)
            {
                Weights[i] = new Matrix();
                Weights[i].Init(Size[i + 1], Size[i]);

                Weights[i].Rand();
                for (int j = 0; j < Size[i + 1]; j++)
                {
                    Bios[i] = new double[Size[i]];
                    Bios[i][j] = ((rnd.Next(-1000, 1000) % 50)) * 0.06 / (Size[i] + 15);
                }
            }
            Neurons_val = new double[L][];
            Neurons_err = new double[L][];
            for (int i = 0; i < L; i++)
            {
                Neurons_val[i] = new double[Size[i]];
                Neurons_err[i] = new double[Size[i]];
            }
            Bios_val = new double[L - 1];
            for (int i = 0; i < L - 1; i++)
            {
                Bios_val[i] = 1;
            }
         
        }
        public void SetInput(double[] values)
        {
            for (int i = 0; i < Size[0]; i++)
            {
                Neurons_val[0][i] = values[i];
            }
        }
        public double ForwardFeed()
        {
            for (int k = 1; k < L; ++k)
            {
                Neurons_val[k] = Matrix.Multi(Weights[k - 1], Neurons_val[k - 1], Size[k - 1], Neurons_val[k]);
                Neurons_val[k] = Matrix.SumVector(Neurons_val[k], Bios[k - 1], Size[k]);
                Neurons_val[k] = ActivateFunction.Use(Neurons_val[k], Size[k]);
            }
            int pred = SearchMaxIndex(Neurons_val[L - 1].ToArray());
            return pred;
        }
        int SearchMaxIndex(double[] value)
        {
            double max = value[0];
            int prediction = 0;
            double tmp;
            for (int j = 1; j < Size[L - 1]; j++)
            {
                tmp = value[j];
                if (tmp > max)
                {
                    prediction = j;
                    max = tmp;
                }
            }
            return prediction;
        }
        public void PrintValues(int L)
        {
            for (int j = 0; j < Size[L]; j++)
            {
                Console.Write(j + " " + Neurons_val[L][j]);
            }
        }
        public void BackPropogation(double expect)
        {
            for (int i = 0; i < Size[L - 1]; i++)
            {
                if (i != (int)expect)
                    Neurons_err[L - 1][i] = -Neurons_val[L - 1][i] * ActivateFunction.UseDer(Neurons_val[L - 1][i]);
                else
                    Neurons_err[L - 1][i] = (1.0 - Neurons_val[L - 1][i]) * ActivateFunction.UseDer(Neurons_val[L - 1][i]);
            }
            for (int k = L - 2; k > 0; k--)
            {
                Matrix.Multi_T(Weights[k], Neurons_err[k + 1].ToArray(), Size[k + 1], Neurons_err[k].ToArray());
                for (int j = 0; j < Size[k]; j++)
                    Neurons_err[k][j] *= ActivateFunction.UseDer(Neurons_val[k][j]);
            }
        }
        public void WeightsUpdater(double lr)
        {
            for (int i = 0; i < L - 1; ++i)
            {
                for (int j = 0; j < Size[i + 1]; ++j)
                {
                    for (int k = 0; k < Size[i]; ++k)
                    {
                        Weights[i].matrix[j,k] += Neurons_val[i][k] * Neurons_err[i + 1][j] * lr;
                    }
                }
            }
            for (int i = 0; i < L - 1; i++)
            {
                for (int k = 0; k < Size[i + 1]; k++)
                {
                    Bios[i][k] += Neurons_err[i + 1][k] * lr;
                }
            }
        }
        public struct WeigthsToSaveStruct
        {
            public Matrix[] matrix;
            public double[][] Bios;
        }
        public void SaveWeights()
        {
            string json = JsonConvert.SerializeObject(new WeigthsToSaveStruct
            {
                matrix = Weights,
                Bios = Bios
            });
          
            File.WriteAllText(nameToSave, json);
        }

        string nameToSave = "weights.txt";
        public void ReadWeights(string name)
        {
            nameToSave = name;
            if (File.ReadAllText(name).Trim() != "")
            {
                var savedInfo = JsonConvert.DeserializeObject<WeigthsToSaveStruct>(File.ReadAllText(name));
                Weights = savedInfo.matrix;
                Bios = savedInfo.Bios;
            }
        }
    }
}
