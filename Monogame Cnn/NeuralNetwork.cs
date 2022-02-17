using System;
using System.Collections.Generic;
using System.Text;

namespace Monogame_Cnn
{
    public class NeuralNetwork
    {
        private Random random;
        public float[][][] weights { get; set; }
        public float[][] neurons { get; set; }
        public int[] connections { get; set; }
        public int[] layers { get; set; }
        public float fitness;
        public NeuralNetwork(int[] LAYERS)
        {
            random = new Random();
            layers = new int[LAYERS.Length];

            for (int i = 0; i < LAYERS.Length; i++)
            {
                layers[i] = LAYERS[i];
            }

            InitConnections();
            InitNeurons();
            InitWeights();


        }
        private void InitConnections()
        {
            connections = new int[layers.Length - 1];
            for (int i = 1; i < layers.Length; i++)
            {
                connections[i - 1] = layers[i] * layers[i - 1];
            }
        }
        private void InitNeurons()
        {
            neurons = new float[layers.Length][];
            float[] temp;

            for (int i = 0; i < layers.Length; i++)
            {
                temp = new float[layers[i]];
                for (int j = 0; j < layers[i]; j++)
                {
                    temp[j] = j + 1;
                }
                neurons[i] = temp;
            }
        }
        private void InitWeights()
        {
            weights = new float[connections.Length][][];
            float[][] tmp;
            float[] temp;

            // the amount of weights are 2 
            for (int i = 0; i < weights.Length; i++)
            {// then for the first row set the weight values
                tmp = new float[layers[i]][];
                for (int j = 0; j < layers[i]; j++)
                {// get the amount of weights by looking at all the connection points in the next row of neurons.
                    temp = new float[layers[i + 1]];
                    for (int k = 0; k < layers[i + 1]; k++)
                    {   // set the weight to a random value between -1 & 1
                        temp[k] = ((float)random.NextDouble() * 2) - 1f;
                    }
                    tmp[j] = temp;
                }
                weights[i] = tmp;
            }
        }
        public float[] FeedForward(float[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                neurons[0][i] = input[i];
            }
            // neurons 2 , 4 , 2

            // 4
            for (int i = 1; i < layers.Length; i++)
            {                       // 4 4 2
                for (int j = 0; j < neurons[i].Length; j++)
                {
                    float value = 0.25f;  // 2 4 4
                    for (int k = 0; k < neurons[i - 1].Length; k++)
                    {
                        value += neurons[i - 1][k] * weights[i - 1][k][j];
                    }

                    neurons[i][j] = (float)Math.Tanh(value);
                }
            }
            return neurons[neurons.Length - 1];
        }
        public void Mutate()
        {
            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        float weight = weights[i][j][k];
                        //Mutate that weight.
                        float randomNumber = (float)random.NextDouble() * 10f;

                        if (randomNumber <= 2f)
                        {
                            weight *= -1;
                        }
                        else if (randomNumber <= 4)
                        {
                            weight = ((float)random.NextDouble() * 2f) - 1f;
                        }
                        else if (randomNumber <= 6)
                        {
                            weight *= ((float)random.NextDouble() + 1);
                        }
                        else if (randomNumber <= 8)
                        {
                            weight *= (float)random.NextDouble();
                        }

                        weights[i][j][k] = weight;
                    }
                }
            }
        }
        public void PrintLayers()
        {
            Console.WriteLine("layers");
            for (int i = 0; i < layers.Length; i++)
            {
                Console.Write(layers[i] + " ");
            }
            Console.WriteLine();
        }
        public void AddFitness(float VALUE)
        {
            fitness += VALUE;
        }
        public int CompareTo(NeuralNetwork other)
        {
            if (other == null) return 1;
            if (fitness > other.fitness) return 1;
            else if (fitness < other.fitness) return -1;
            else return 0;
        }
        public void WriteWeights(NeuralNetwork other)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        weights[i][j][k] = other.weights[i][j][k];
                    }
                }
            }
        }
        public float[][][] CopyWeights()
        {
            return weights;
        }
        public void PrintNeurons()
        {
            Console.WriteLine("neurons");
            for (int i = 0; i < neurons.Length; i++)
            {
                for (int j = 0; j < neurons[i].Length; j++)
                {
                    Console.Write(neurons[i][j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public void PrintWeights()
        {
            Console.WriteLine("weights");
            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        Console.Write(weights[i][j][k] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public void CopyFitness(NeuralNetwork other)
        {
            fitness = other.fitness;
        }
    }
}
