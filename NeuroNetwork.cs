using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroNetwork
{
    public class NeuroNetwork
    {
        public Topology Topology { get; }
        public List<Layer> Layers { get; }
        public NeuroNetwork(Topology topology)
        {
            Topology = topology;

            Layers = new List<Layer>();
            CreateInputLayer();
            CreateHiddenLayer();
            CreateOutputLayer();
        }

        public Neuron FeedForward(List<double> inputSignals)
        {
            SendSignalsToInputNeurons(inputSignals);
            FeedForwardAllAfterInput();

            if (Topology.OutputCount == 1)
            {
                return Layers.Last().Neurons[0];
            }
            else
            {
                return Layers.Last().Neurons.OrderByDescending(n => n.Output).First();
            }
        }

        private void FeedForwardAllAfterInput()
        {
            for (int i = 1; i < Layers.Count; i++)
            {
                var previousLayerSignals = Layers[i - 1].GetSignals();
                var layer = Layers[i];

                foreach (var neuron in layer.Neurons)
                {
                    neuron.FeedForward(previousLayerSignals);
                }
            }
        }

        private void SendSignalsToInputNeurons(List<double> inputSignals)
        {
            for (int i = 0; i < inputSignals.Count; i++)
            {
                var signal = new List<double> { inputSignals[i] };
                var neuron = Layers[0].Neurons[i];

                neuron.FeedForward(signal);
            }
        }

        private void CreateInputLayer()
        {
            List<Neuron> inputNeurons = new List<Neuron>();
            for (int i = 0; i < Topology.InputCount; i++)
            {
                Neuron neuron = new Neuron(1, NeuronType.Input);
                inputNeurons.Add(neuron);
            }
            Layer inputLayer = new Layer(inputNeurons, NeuronType.Input);
            Layers.Add(inputLayer);
        }

        private void CreateHiddenLayer()
        {
            for (int j = 0; j < Topology.HiddenLayers.Count; j++)
            {
                List<Neuron> hiddenNeurons = new List<Neuron>();
                for (int i = 0; i < Topology.HiddenLayers[j]; i++)
                {
                    Neuron neuron = new Neuron(1);
                    hiddenNeurons.Add(neuron);
                }
                Layer hiddenLayer = new Layer(hiddenNeurons);
                Layers.Add(hiddenLayer);
            }
        }

        private void CreateOutputLayer()
        {
            List<Neuron> outputNeurons = new List<Neuron>();
            Layer lastLayer = Layers.Last();
            for (int i = 0; i < Topology.OutputCount; i++)
            {
                Neuron neuron = new Neuron(lastLayer.Count, NeuronType.Output);
                outputNeurons.Add(neuron);
            }
            Layer outputLayer = new Layer(outputNeurons, NeuronType.Output);
            Layers.Add(outputLayer);
        }
    }
}
