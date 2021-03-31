using System;
using System.Collections.Generic;

namespace NeuroNetwork
{
    public class Layer
    {
        public List<Neuron> Neurons { get; }
        public int Count => Neurons?.Count ?? 0;

        public Layer(List<Neuron> neurons, NeuronType type = NeuronType.Hidden)
        {
            foreach (Neuron neuron in neurons)
            {
                if (neuron.NeuronType != type)
                {
                    throw new Exception($"Нейрон {neuron} имел неверный тип. Требовался {type}, получен {neuron.NeuronType}");
                }
            }

            Neurons = neurons;
        }

        public List<double> GetSignals()
        {
            var result = new List<double>();
            foreach (var neuron in Neurons)
            {
                result.Add(neuron.Output);
            }
            return result;
        }
    }
}
