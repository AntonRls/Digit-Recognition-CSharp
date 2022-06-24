using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace NeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            Network network = new Network();
            network.Init(new Network.DataNetwork
            {
                L = 3,
                size = new int[] { 784, 256, 10 }
            });
            network.ReadWeights("1HideLayerWeights.txt"); //1HideLayerWeights.txt - веса с одним скрытым слоем

            Console.WriteLine("1 - Открытие интерфейса\n2- обучение");
            if (Console.ReadLine() == "1")
            {
                Application.Run(new DrawNumberForm(network));
            }
            else
            {

                //Обрабатываем данные
                string[] lines = File.ReadAllLines("lib60k.txt");
                List<double> image = new List<double>();
                int currentAnwer = 0;
                List<GG> gg = new List<GG>();
                foreach (string line1 in lines)
                {
                    string line = line1.Trim();

                    if (line.Length > 2)
                    {
                        foreach (string bt in line.Split(' '))
                        {

                            image.Add(double.Parse(bt.Trim().Replace('.', ',')));
                        }
                    }
                    else if (line.Length == 1)
                    {
                        if (image.Count != 0)
                        {
                            gg.Add(new GG
                            {
                                answer = currentAnwer,
                                image = image.ToArray()
                            });
                        }
                        currentAnwer = int.Parse(line);
                        image.Clear();
                    }

                }



                //начинаем процесс обучения
                int rigthCount = 0;
                int epoch = 0;
                Console.WriteLine(gg.Count);
                while (rigthCount / gg.Count * 100 < 100)
                {
                    Console.Title = epoch.ToString();
                    rigthCount = 0;
                    foreach (var item in gg)
                    {
                        network.SetInput(item.image);
                        double result = network.ForwardFeed();
                   
                        if (result != item.answer)
                        {
                            network.BackPropogation(item.answer);
                            network.WeightsUpdater(0.15 * Math.Exp(-epoch / 20));
                        }
                        else
                        {
                            rigthCount++;
                        }


                    }
                    network.SaveWeights();
                    Console.WriteLine(float.Parse(rigthCount.ToString()) / float.Parse(gg.Count.ToString()) * 100f);
                    epoch++;

                }

                network.SaveWeights();
            }
            Console.ReadLine();
        }
      
        public struct GG
        {
            public double answer;
            public double[] image;
        } 
    }
}
