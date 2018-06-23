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
        private int numInputs;
        private int numNeuronsInHiddenLayer;
        private int numOutputs;

        public NeuralNetwork(int numInputs, int numNeuronsInHiddenLayer, int numOutputs)
        {
            this.numInputs = numInputs;
            this.numNeuronsInHiddenLayer = numNeuronsInHiddenLayer;
            this.numOutputs = numOutputs;
        }


    }
}
