using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralBird
{
    [Serializable]
    public class WeightsInfo
    {
        // hidden layer weights
        public double[,] weights1;

        // output layer weights
        public double[,] weights2;
        

        public WeightsInfo(double[,] weights1, double[,] weights2)
        {
            double[,] _weights1 = new double[weights1.GetLength(0), weights1.GetLength(1)];
            double[,] _weights2 = new double[weights2.GetLength(0), weights2.GetLength(1)];


            for (int i = 0; i < weights1.GetLength(0); i++)
                for (int j = 0; j < weights1.GetLength(1); j++)
                    _weights1[i, j] = weights1[i, j];


            for (int i = 0; i < weights2.GetLength(0); i++)
                for (int j = 0; j < weights2.GetLength(1); j++)
                    _weights2[i, j] = weights2[i, j];

            this.weights1 = _weights1;
            this.weights2 = _weights2;
        }

        public WeightsInfo()
        {
            this.weights1 = new double[NeuralNetwork.inputSize, NeuralNetwork.hiddenSize];
            this.weights2 = new double[NeuralNetwork.hiddenSize, NeuralNetwork.outputSize];
        }
    }

    [Serializable]
    public class NeuralNetwork
    {
        public static int inputSize = 4;
        public static int hiddenSize = 5;
        public static int outputSize = 1;
        public int _id;
        private static int _nextId = 0;
        Random rand;




        double[,] input;
        public double[,] output;

        public WeightsInfo Weights = new WeightsInfo();

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

            Weights.weights1 = _weights1;
            Weights.weights2 = _weights2;
        }

        public double Process(double y, double dist, double pipeY, double velocity)
        {
            input = new double[1, inputSize];
            input[0, 0] = y;
            input[0, 1] = dist;
            input[0, 2] = pipeY;
            input[0, 3] = velocity;
            

            double[,] hiddenInputs = multiplyArrays(input, Weights.weights1);
            double[,] hiddenOutputs = applySigmoid(hiddenInputs);

            // calculate output certainty
            output = applySigmoid(multiplyArrays(hiddenOutputs, Weights.weights2));

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

        // slightly tweak weights so new behaviour arrises or old gets refined (or degraded)
        internal void Mutate()
        {
            if (rand.NextDouble() < WorldRules.MutationChance)
            {
                for (int i = 0; i < Weights.weights1.GetLength(0); i++)
                    for (int j = 0; j < Weights.weights1.GetLength(1); j++)
                      if (rand.NextDouble() < 0.5)
                          Weights.weights1[i,j] += WorldRules.MutationSeverity;
                      else
                          Weights.weights1[i,j] -= WorldRules.MutationSeverity;

                for (int i = 0; i < Weights.weights2.GetLength(0); i++)
                    for (int j = 0; j < Weights.weights2.GetLength(1); j++)
                        if (rand.NextDouble() < 0.5)
                            Weights.weights2[i, j] += WorldRules.MutationSeverity;
                        else
                            Weights.weights2[i, j] -= WorldRules.MutationSeverity;

            }
        }
    }
}
