using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    /// <summary>
    /// Активационные функции
    /// </summary>
    class ActivateFunction
    {
        ActivateFucntionEnum func = ActivateFucntionEnum.ReLU;

        public double[] Use(double[] value1, int n)
        {
            double[] value = value1;
            if (func == ActivateFucntionEnum.ReLU)
            {
                for (int i = 0; i < n; i++)
                {
                    if (value[i] < 0) { value[i] *= 0.01; }
                    else if (value[i] > 1) { value[i] = 1d + 0.01 * (value[i] - 1d); }
                }
            }
            return value;
        }
        public double UseDer(double value)
        {
            if (value < 0) return 0.01;
            else if (value > 1) return 0.01;
            else return 1;
        }
        /// <summary>
        /// Активационные функции
        /// TODO: Реализовать sigmoid и thx
        /// </summary>
        public enum ActivateFucntionEnum
        {
            Sigmoid,
            ReLU,
            thx
        }

    }
}
