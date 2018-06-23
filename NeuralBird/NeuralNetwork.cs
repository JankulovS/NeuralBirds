using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralBird
{
    [Serializable]
    public class NeuralNetwork
    {
        private static int inputSize = 4;
        private static int hiddenSize = 2;
        private static int outputSize = 1;
        public int _id;
        private static int _nextId = 0;
        Random rand;

        [Serializable]
        public class WeightsInfo
        {
            // hidden layer weights
            public double[,] weights1;

            // output layer weights
            public double[,] weights2;

            public WeightsInfo(double[,] weights1, double[,] weights2)
            {
                this.weights1 = weights1;
                this.weights2 = weights2;
            }

            public WeightsInfo()
            {
                this.weights1 = new double[inputSize, hiddenSize];
                this.weights2 = new double[hiddenSize, outputSize];
            }
        }


        double[,] input;
        public double[,] output;

        WeightsInfo weights = new WeightsInfo();
        //WeightsInfo nextWeights = new WeightsInfo();

        public NeuralNetwork()
        {
            _id = _nextId++;
            rand = new Random(_id);
            InitNetwork();
        }

        public void InitNetwork()
        {
            double[,] _weights1 = new double[inputSize, hiddenSize];

            for (int i = 0; i < _weights1.GetLength(0); i++)
                for (int j = 0; j < _weights1.GetLength(1); j++)
                    _weights1[i, j] = rand.NextDouble() * 2 - 1;


            double[,] _weights2 = new double[hiddenSize, outputSize];

            for (int i = 0; i < _weights2.GetLength(0); i++)
                for (int j = 0; j < _weights2.GetLength(1); j++)
                    _weights2[i, j] = rand.NextDouble() * 2 - 1;

            weights.weights1 = _weights1;
            weights.weights2 = _weights2;
        }

        public double Process(double y, double dist, double pipeY, double velocity)
        {
            input = new double[1, inputSize];
            input[0, 0] = y;
            input[0, 1] = dist;
            input[0, 2] = pipeY;
            input[0, 3] = velocity;

            //double[,] output;

            double[,] hiddenInputs = multiplyArrays(input, weights.weights1);
            double[,] hiddenOutputs = applySigmoid(hiddenInputs);

            // then the final output
            output = applySigmoid(multiplyArrays(hiddenOutputs, weights.weights2));

            return output[0,0];
        }


        // NN math functions
        double[,] applySigmoid(double[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    array[i, j] = sigmoid(array[i, j]);

            return array;
        }

        double sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        double[,] multiplyArrays(double[,] a1, double[,] a2)
        {
            double[,] a3 = new double[a1.GetLength(0), a2.GetLength(1)];

            for (int i = 0; i < a3.GetLength(0); i++)
                for (int j = 0; j < a3.GetLength(1); j++)
                {
                    a3[i, j] = 0;
                    for (int k = 0; k < a1.GetLength(1); k++)
                        a3[i, j] = a3[i, j] + a1[i, k] * a2[k, j];
                }
            return a3;
        }
    }
}
