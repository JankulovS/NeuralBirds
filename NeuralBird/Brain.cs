﻿using ArtificialNeuralNetwork;
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
            //var somaFactory = SomaFactory.GetInstance(new SimpleSummation());
            //var axonFactory = AxonFactory.GetInstance(new TanhActivationFunction());
            //var randomInit = new RandomWeightInitializer(new Random(Program.rand.Next()));
            //var hiddenSynapseFactory = SynapseFactory.GetInstance(new RandomWeightInitializer(new Random(Program.rand.Next())), axonFactory);
            //var ioSynapseFactory = SynapseFactory.GetInstance(new RandomWeightInitializer(new Random(Program.rand.Next())), axonFactory);
            //var biasInitializer = new RandomWeightInitializer(new Random(Program.rand.Next()));
            //var neuronFactory = NeuronFactory.GetInstance();
            //INeuralNetworkFactory factory = NeuralNetworkFactory.GetInstance(somaFactory, axonFactory, hiddenSynapseFactory, ioSynapseFactory, biasInitializer, neuronFactory);



            var numInputs = 4;
            var numOutputs = 1;
            var numHiddenLayers = 2;
            var numNeuronsInHiddenLayer = 6;
            network = NeuralNetworkFactory.GetInstance().Create(numInputs, numOutputs, numHiddenLayers, numNeuronsInHiddenLayer);
        }

        public void SetGenes(NeuralNetworkGene genes)
        {
            network = NeuralNetworkFactory.GetInstance().Create(genes);
        }

        double Lerp(double v0, double v1, double t)
        {
            return (1 - t) * v0 + t * v1;
        }

        // this function returns the network's prediction (0 or 1)
        public int Think(double birdY, double dist, double pipeY, double verticalSpeed)
        {
            double[] inputs = { birdY / WorldRules.WindowHeight, dist / WorldRules.WindowWidth, pipeY/ WorldRules.WindowHeight, verticalSpeed / WorldRules.MaxSpeed};
            //double[] inputs = { birdY, dist, pipeY, verticalSpeed };
            network.SetInputs(inputs);
            network.Process();
            double[] outputs = network.GetOutputs();

            //if (Lerp(0,1,(outputs[0] + 1)/2) > Program.rand.NextDouble() / 2 + 0.5)
            if (Lerp(0, 1, (outputs[0] + 1) / 2) > 0.5)
            {
                return 1;
            }
            else
            {
                return 0; 
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
                            if (rand.NextDouble() < 0.05)
                            {
                                if (rand.NextDouble() > 0.5)
                                    newWeightsAxon.Add(weight + 0.1);
                                else
                                    newWeightsAxon.Add(weight - 0.1);
                            }
                            newWeightsAxon.Add(weight);
                        }
                        gene.Neurons.RemoveAt(j);
                        neuron.Axon.Weights = newWeightsAxon;

                        if (rand.NextDouble() > 0.5)
                            neuron.Soma.Bias = neuron.Soma.Bias * 1.1;
                        else
                            neuron.Soma.Bias = neuron.Soma.Bias * 0.9;
                        gene.Neurons.Insert(j, neuron);
                    }

                    }
                
            }

            network = NeuralNetworkFactory.GetInstance().Create(genes);
        }
        
    }
}
