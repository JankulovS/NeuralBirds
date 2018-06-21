using ArtificialNeuralNetwork;
using ArtificialNeuralNetwork.ActivationFunctions;
using ArtificialNeuralNetwork.Factories;
using ArtificialNeuralNetwork.Genes;
using ArtificialNeuralNetwork.WeightInitializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralBird
{
    class Brain
    {
        public INeuralNetwork network { get; set; }
        
        public Brain()
        {
            var numInputs = 4;
            var numOutputs = 1;
            var numHiddenLayers = 4;
            var numNeuronsInHiddenLayer = 20;
            network = NeuralNetworkFactory.GetInstance().Create(numInputs, numOutputs, numHiddenLayers, numNeuronsInHiddenLayer);
        }
        

        // this function returns the network's prediction (0 or 1)
        public int Think(double birdY, double dist, double pipeY, double verticalSpeed)
        {
            network.SetInputs(new[] {birdY, dist, pipeY, verticalSpeed });
            network.Process();
            double[] outputs = network.GetOutputs();

            if (outputs[0] > 0.5)
            {
                return 0;
            }
            else
            {
                return 1; 
            }
            
        }

        public void Mutate()
        {
            NeuralNetworkGene genes = network.GetGenes();
            Random rand = new Random();

            for (int i = 0; i < genes.HiddenGenes.Count; i++)
            {
                
                    var gene = genes.HiddenGenes.ElementAt(i);
                    for (int j = 0; j < genes.HiddenGenes.Count; j++)
                    {
                    if (rand.NextDouble() < 0.1)
                    {
                        var neuron = gene.Neurons.ElementAt(j);
                        List<double> newWeightsAxon = new List<double>();
                        foreach (var weight in neuron.Axon.Weights)
                        {
                            if (rand.NextDouble() > 0.5)
                                newWeightsAxon.Add(weight * 1.01);
                            else
                                newWeightsAxon.Add(weight * 0.99);
                        }
                        gene.Neurons.RemoveAt(j);
                        neuron.Axon.Weights = newWeightsAxon;

                        if (rand.NextDouble() > 0.5)
                            neuron.Soma.Bias = neuron.Soma.Bias * 1.01;
                        else
                            neuron.Soma.Bias = neuron.Soma.Bias * 0.99;
                        gene.Neurons.Insert(j, neuron);
                    }

                    }
                
            }

            network = NeuralNetworkFactory.GetInstance().Create(genes);
        }
        
    }
}
