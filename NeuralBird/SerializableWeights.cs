using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralBird
{
    [Serializable]
    public class SerializableWeights
    {
        public List<List<double>> weights1;
        public List<List<double>> weights2;

        public SerializableWeights()
        {

        }

        public SerializableWeights(WeightsInfo weights)
        {
            weights1 = new List<List<double>>();
            weights2 = new List<List<double>>();

            for (int i = 0; i < weights.weights1.GetLength(0); i++)
            {
                var weightsList = new List<double>();
                for (int j = 0; j < weights.weights1.GetLength(1); j++)
                {
                    weightsList.Add(weights.weights1[i, j]);
                }
                weights1.Add(weightsList);
            }

            for (int i = 0; i < weights.weights2.GetLength(0); i++)
            {
                var weightsList = new List<double>();
                for (int j = 0; j < weights.weights2.GetLength(1); j++)
                {
                    weightsList.Add(weights.weights2[i, j]);
                }
                weights2.Add(weightsList);
            }
        }

        public WeightsInfo ToWeightsInfo()
        {
            WeightsInfo weights = new WeightsInfo();

            weights.weights1 = new double[NeuralNetwork.inputSize, NeuralNetwork.hiddenSize];
            weights.weights2 = new double[NeuralNetwork.hiddenSize, NeuralNetwork.outputSize];

            int i = 0;
            int j = 0;

            foreach(var w1 in weights1)
            {
                j = 0;
                foreach(double weight in w1)
                {
                    weights.weights1[i, j] = weight;
                    j++;
                }
                i++;
            }

            i = 0;
            
            foreach (var w2 in weights2)
            {
                j = 0;
                foreach (double weight in w2)
                {
                    weights.weights2[i, j] = weight;
                    j++;
                }
                i++;
            }

            return weights;
        }
    }
}
