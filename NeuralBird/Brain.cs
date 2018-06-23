using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralBird
{
    
    public class Brain
    {
        public NeuralNetwork Network { get; set; }


        public Brain()
        {
            Network = new NeuralNetwork();
        }

        public void SetGenes(WeightsInfo weights)
        {
            Network.Weights = new WeightsInfo(weights.weights1, weights.weights2);
        }


        // this function returns the network's prediction (0 or 1)
        public int Think(double birdY, double dist, double pipeY, double verticalSpeed)
        {
            double[] inputs = { birdY / WorldRules.WindowHeight, dist / WorldRules.WindowWidth, pipeY / WorldRules.WindowHeight, verticalSpeed / WorldRules.MaxSpeed };

            double output = Network.Process(inputs[0], inputs[1], inputs[2], inputs[3]);

            if (output > 0.5)
            {
                return 1;
            }
            else
            {
                return 0; 
            }
            
        }


        internal WeightsInfo GetGenes()
        {
            return Network.Weights;
        }
    }
}
