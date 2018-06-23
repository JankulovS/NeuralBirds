﻿using ArtificialNeuralNetwork;
using ArtificialNeuralNetwork.Factories;
using ArtificialNeuralNetwork.Genes;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NeuralBird
{
    public class Gene
    {
        public List<List<double>> Weights;
        public List<double> Bias;

        public Gene()
        {
            Weights = new List<List<double>>();
            Bias = new List<double>();
        }
    }

    class Program
    {
        public static RenderWindow mainWindow;
        public static Color backgroundColor = Color.White;
        public static List<GameObject> gameObjects = new List<GameObject>();
        public static Random rand = new Random();
        public static bool playerAlive = true;
        public static int playerPoints = 0;
        public static Bird playerBird = null;
        public static bool isPlayerPlaying = false;
        public static Text score;
        public static Text highScore;
        public static int numberOfAliveBirds = 0;
        public static List<Bird> deadBirds = new List<Bird>();
        public static int highScorePoints = 0;
        public static bool isPretrained = false;

        static void Main(string[] args)
        {


            PlayerChoiceMenu();


            mainWindow = new RenderWindow(new VideoMode(WorldRules.WindowWidth, WorldRules.WindowHeight), "Neural Bird");
            mainWindow.SetFramerateLimit(60);
            score = new Text();
            score.Position = new Vector2f(20, WorldRules.WindowHeight - 100);
            score.Color = Color.Black;
            Font font = new Font(Directory.GetCurrentDirectory() + "/data/font.ttf");
            score.Font = font;
            score.CharacterSize = 32;
            score.DisplayedString = playerPoints.ToString();
            highScore = new Text();
            highScore.Position = new Vector2f(20, WorldRules.WindowHeight - 60);
            highScore.Color = Color.Black;
            highScore.Font = font;
            highScore.CharacterSize = 32;
            highScore.DisplayedString = playerPoints.ToString();
            mainWindow.Closed += EventClosed;

            // player plays
            if (isPlayerPlaying)
            {
                playerAlive = true;
            }
            
            mainWindow.KeyPressed += EventKeyPressed;

            // DEBUG
            ResetGame();

            // END DEBUG

            MainLoop();
        }

        private static void PlayerChoiceMenu()
        {
            int i = 0;

            while(i == 0)
            {
                Console.Clear();
                Console.WriteLine("-- NEURAL NETWORK FLAPPY BIRD --");
                Console.WriteLine("1 - play flappy bird yourself");
                Console.WriteLine("2 - let neural network learn how to play");
                Console.WriteLine("3 - let pretrained neural network play");
                Console.Write(" >> ");
                string input = Console.ReadLine();


                try
                {
                    i = Convert.ToInt32(input);
                    switch(i)
                    {
                        case 1:
                            isPlayerPlaying = true;
                            break;
                        case 2:
                            isPlayerPlaying = false;
                            break;
                        case 3:
                            isPlayerPlaying = false;
                            isPretrained = true;
                            break;
                    }

                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private static void EventKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Space && isPlayerPlaying)
            {
                
                foreach(var obj in gameObjects)
                {
                    if (obj is Bird)
                    {
                        var obj2 = obj as Bird;
                        obj2.Jump();
                    }
                }
            }
            if (e.Code == Keyboard.Key.Add && isPlayerPlaying == false)
            {
                WorldRules.SimulationsPerUpdate++;
                if (WorldRules.SimulationsPerUpdate > 10)
                {
                    WorldRules.SimulationsPerUpdate = 10;
                }
                else
                {
                    Console.WriteLine("Speed now " + WorldRules.SimulationsPerUpdate + "00%");
                }
            }

            if (e.Code == Keyboard.Key.Subtract && isPlayerPlaying == false)
            {
                WorldRules.SimulationsPerUpdate--;
                if (WorldRules.SimulationsPerUpdate < 1)
                {
                    WorldRules.SimulationsPerUpdate = 1;
                }
                else
                {

                    Console.WriteLine("Speed now " + WorldRules.SimulationsPerUpdate + "00%");
                }
            }

            if (e.Code == Keyboard.Key.S && isPlayerPlaying == false)
            {
                SaveNetworks();
            }
        }

        private static void SaveNetworks()
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Gene>));
            var fStream = new FileStream(Directory.GetCurrentDirectory() + "/data/saved.brains", FileMode.Create, FileAccess.Write, FileShare.None);
            List<Gene> newHiddenGenes = new List<Gene>(); ;

            // alive birds
            //foreach (var bird in gameObjects)
            //{
            //    if (bird is Bird)
            //    {

            //        var hiddenGenes = ((Bird)bird).brain.Network.GetGenes().HiddenGenes;
            //        Gene newGene;
            //        foreach(var gene in hiddenGenes)
            //        {
            //            newGene = new Gene();
            //            var newWeights = new List<List<Double>>();
            //            var newBias = new List<Double>();
            //            foreach(var neuron in gene.Neurons)
            //            {
            //                newWeights.Add(neuron.Axon.Weights.ToList());
            //                newBias.Add(neuron.Soma.Bias);
            //            }
            //            newGene.Bias = newBias;
            //            newGene.Weights = newWeights;

            //            newHiddenGenes.Add(newGene);
            //        }
            //    }
            //}

            // dead birds
            //foreach (var bird in deadBirds)
            //{
            //    if (bird is Bird)
            //    {

            //        var hiddenGenes = ((Bird)bird).brain.Network.GetGenes().HiddenGenes;
            //        Gene newGene;
            //        foreach (var gene in hiddenGenes)
            //        {
            //            newGene = new Gene();
            //            var newWeights = new List<List<Double>>();
            //            var newBias = new List<Double>();
            //            foreach (var neuron in gene.Neurons)
            //            {
            //                newWeights.Add(neuron.Axon.Weights.ToList());
            //                newBias.Add(neuron.Soma.Bias);
            //            }
            //            newGene.Bias = newBias;
            //            newGene.Weights = newWeights;

            //            newHiddenGenes.Add(newGene);
            //        }
            //    }
            //}

            ser.Serialize(fStream, newHiddenGenes);
            fStream.Close();

            Console.WriteLine("Current network network genes saved!");
        }

        private static void LoadNetworks()
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Gene>));
            var fStream = new FileStream(Directory.GetCurrentDirectory() + "/data/saved.brains", FileMode.Open, FileAccess.Read, FileShare.None);
            List<Gene> newGenes = (List < Gene > )ser.Deserialize(fStream);
            int counter = 0;

            //foreach (var bird in gameObjects)
            //{
            //    if (bird is Bird)
            //    {
            //        var genes = ((Bird)bird).brain.Network.GetGenes();
            //        foreach(var gene in genes.HiddenGenes)
            //        {
            //            for(int j = 0; j < gene.Neurons.Count; j++)
            //            {
            //                gene.Neurons.ElementAt(j).Axon.Weights = newGenes.ElementAt(counter).Weights.ElementAt(j);
            //                gene.Neurons.ElementAt(j).Soma.Bias = newGenes.ElementAt(counter).Bias.ElementAt(j);
            //            }
            //        }
            //        counter++;
            //    }
            //}

            Console.WriteLine("Loaded neural network genes");
        }

        private static void EventClosed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        static void MainLoop()
        {
            while (mainWindow.IsOpen())
            {


                // update logic loop
                for (int i = 0; i < WorldRules.SimulationsPerUpdate; i++)
                    UpdateWorld();

                // update graphics
                UpdateGraphics();

                // Process events
                mainWindow.DispatchEvents();
            }

        }

        public static void UpdateWorld()
        {
            if (playerAlive == false && isPlayerPlaying == true)
            {
                ResetGame();
            }

            if (numberOfAliveBirds <= 0 && isPlayerPlaying == false)
            {
                NewGeneration();
            }

            foreach(var obj in gameObjects)
            {
                obj.Update();
            }

            if (isPlayerPlaying)
            {
                playerPoints = playerBird.Points;
            }
            
            else
            {
                playerPoints = ((Bird)gameObjects.ElementAt(0)).Points;
            }


            if (playerPoints > highScorePoints)
            {
                highScorePoints = playerPoints;
            }

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if(gameObjects.ElementAt(i) is Pipe)
                {
                    var obj = gameObjects.ElementAt(i) as Pipe;
                    if (obj.ForDeletion)
                    {
                        gameObjects.RemoveAt(i);
                        var pipe = new Pipe();
                        gameObjects.Add(pipe);
                    }
                }
                else
                {
                    // store dead birds for good neural networks
                    var obj = gameObjects.ElementAt(i) as Bird;
                    if (obj.IsDead)
                    {
                        gameObjects.RemoveAt(i);
                        numberOfAliveBirds--;
                        deadBirds.Add(obj);
                    }
                }
            }
        }

        private static void NewGeneration()
        {
            gameObjects.Clear();
            double maxScore = 0;

            double scoreSum = 0;
            foreach (var bird in deadBirds)
            {
                if (bird.Points > maxScore)
                {
                    maxScore = bird.Points;
                }
                scoreSum += bird.Points;
            }



            // sort by points
            //deadBirds = deadBirds.OrderBy(o => o.Points).ToList();
            //deadBirds.Reverse();

            List<KeyValuePair<Bird, double>> elements = new List<KeyValuePair<Bird, double>>();
            foreach (Bird bird in deadBirds)
            {
                elements.Add(new KeyValuePair<Bird, double>(bird, bird.Points / scoreSum));
            }




            Bird obj = null;
            for (int i = 0; i < WorldRules.BirdsPerGeneration; i++)
            {
                Bird bird = new Bird();

                double diceRoll = rand.NextDouble();


                double cumulative = 0.0;
                for (int j = 0; j < elements.Count; j++)
                {
                    cumulative += elements[j].Value;
                    if (diceRoll < cumulative)
                    {
                        obj = elements[j].Key;
                        break;
                    }
                }

                //int chosenBirdBrain = i / 2;

                //INeuralNetwork network;
                //network = NeuralNetworkFactory.GetInstance().Create(deadBirds.ElementAt(chosenBirdBrain).brain.network.GetGenes());
                //obj.brain.network = network;

                //bird.brain.SetGenes(obj.brain.Network.GetGenes());
                //bird.brain.Mutate();
                if (maxScore < 200)
                {
                    bird = new Bird();
                }
                gameObjects.Add(bird);
                playerBird = bird;
            }
            numberOfAliveBirds = WorldRules.BirdsPerGeneration;

            //for (int i = 0; i < WorldRules.BirdsPerGeneration; i++)
            //{
            //    var obj = new Bird();
            //    gameObjects.Add(obj);
            //}
            //numberOfAliveBirds = WorldRules.BirdsPerGeneration;

            var pipe = new Pipe();
            pipe.Position += new Vector2f(0, 0);
            gameObjects.Add(pipe);
            pipe = new Pipe();
            pipe.Position += new Vector2f(333, 0);
            gameObjects.Add(pipe);
            pipe = new Pipe();
            pipe.Position += new Vector2f(666, 0);
            gameObjects.Add(pipe);

            playerAlive = true;
            deadBirds.Clear();
        }

        private static void ResetGame()
        {
            gameObjects.Clear();

            if (isPlayerPlaying)
            {
                Bird obj = new Bird();
                gameObjects.Add(obj);
                playerBird = obj;
            }
            else
            {
                if (isPretrained)
                {
                    Bird obj;
                    for (int i = 0; i < WorldRules.BirdsPerGeneration; i++)
                    {
                        obj = new Bird();
                        gameObjects.Add(obj);
                        playerBird = obj;
                    }

                    LoadNetworks();
                }
                else
                {
                    Bird obj;
                    for (int i = 0; i < WorldRules.BirdsPerGeneration; i++)
                    {
                        obj = new Bird();
                        gameObjects.Add(obj);
                        playerBird = obj;
                    }
                }
                
                numberOfAliveBirds = WorldRules.BirdsPerGeneration;
            }

            var pipe = new Pipe();
            pipe.Position += new Vector2f(0, 0);
            gameObjects.Add(pipe);
            pipe = new Pipe();
            pipe.Position += new Vector2f(333, 0);
            gameObjects.Add(pipe);
            pipe = new Pipe();
            pipe.Position += new Vector2f(666, 0);
            gameObjects.Add(pipe);

            playerAlive = true;
        }

        public static void UpdateGraphics()
        {
            mainWindow.Clear(Color.White);

            // render all gameobjects
            foreach(var obj in gameObjects)
            {
                obj.Render();
            }

            // update ui
            score.DisplayedString = "Current: " + playerPoints.ToString();
            mainWindow.Draw(score);
            highScore.DisplayedString = "Best:      " + highScorePoints.ToString();
            mainWindow.Draw(highScore);


            mainWindow.Display();
        }
    }
}
