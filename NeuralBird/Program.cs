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
        public static Text help;
        public static Text help2;
        public static Text highScore;
        public static int numberOfAliveBirds = 0;
        public static List<Bird> deadBirds = new List<Bird>();
        public static int highScorePoints = 0;
        public static bool isPretrained = false;
        public static WeightsInfo bestNetworkWeightsAlltime;

        static void Main(string[] args)
        {

            while(true)
            {
                PlayerChoiceMenu();


                mainWindow = new RenderWindow(new VideoMode(WorldRules.WindowWidth, WorldRules.WindowHeight), "Neural Bird");
                mainWindow.SetFramerateLimit(60);

                InitUI();


                mainWindow.Closed += EventClosed;

                // player plays
                if (isPlayerPlaying)
                {
                    playerAlive = true;
                }

                mainWindow.KeyPressed += EventKeyPressed;


                ResetGame();

                MainLoop();
            }
            
        }

        private static void InitUI()
        {
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

            help = new Text();
            help.Position = new Vector2f(WorldRules.WindowWidth - 300, WorldRules.WindowHeight - 60);
            help.Color = Color.Black;
            help.Font = font;
            help.CharacterSize = 32;
            help.DisplayedString = "Change game speed (plus/minus)";

            help2 = new Text();
            help2.Position = new Vector2f(WorldRules.WindowWidth - 300, WorldRules.WindowHeight - 100);
            help2.Color = Color.Black;
            help2.Font = font;
            help2.CharacterSize = 32;
            help2.DisplayedString = "Save all bird behaviour (S)";
        }

        private static void PlayerChoiceMenu()
        {
            int i = 0;
            while(i == 0)
            {
                Console.Clear();
                Console.WriteLine("-- NEURAL NETWORK FLAPPY BIRD --");
                Console.WriteLine("1 - Play flappy bird yourself");
                Console.WriteLine("2 - Let neural network learn how to play");
                Console.WriteLine("3 - Let pretrained neural network play");
                Console.WriteLine("4 - Help!");
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

                        case 4:
                            Console.Clear();
                            Console.WriteLine("-- NEURAL NETWORK FLAPPY BIRD --\n\n" +
                                "This is a flappy bird clone game with a neural network AI implementation. \n\n" +
                                "The birds all spawn with random neural network behaviours and are given certain elements of the world as input (their position, velocity, the distance to the next pipe and the position of the center of the pipe's gap). \n" +
                                "There are " + WorldRules.BirdsPerGeneration + " birds per generation, each having its own independant network. " +
                                "After all birds lose the game, a new generation of birds is created. Every bird has a chance to be picked again equal to their score in comparison with the total combined score of all birds. The greater their score the higher the chance that they will be picked again for the next generation. \nAll birds have a chance to mutate their network's behaviour when picked. Every bird's network has a " + (WorldRules.MutationChance * 100).ToString("0.##") +"% chance to mutate random neurons, slightly tweaking their behaviour.\n" +
                                "The overall best bird of all previous generations is always picked.\n\n\n" +
                                "If the player is playing the game, jumping is done by hitting the Space key.\n" +
                                "Game simulation speed can be altered with the plus and minus keys while the network plays.\n" +
                                "All birds' network behaviour can be saved with the S key at any time while the network plays. This will save to the default save file (saved.brains). This will override the previous network save.\n" +
                                "The birds can also be loaded with already trained networks with the third option in the menu. This will load all birds with already (relatively) good trained networks. They will still continue evolving.\n\n" +
                                "Press any key to continue");
                            Console.ReadKey();
                            i = 0;
                            break;

                        default:
                            i = 0;
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
            XmlSerializer ser = new XmlSerializer(typeof(List<SerializableWeights>));
            var fStream = new FileStream(Directory.GetCurrentDirectory() + "/data/saved.brains", FileMode.Create, FileAccess.Write, FileShare.None);
            List<SerializableWeights> newWeightsList = new List<SerializableWeights>();

            

            // alive birds
            foreach (var bird in gameObjects)
            {
                if (bird is Bird)
                {
                    SerializableWeights weights = new SerializableWeights(((Bird)bird).brain.GetGenes());
                    newWeightsList.Add(weights);
                }
            }

            // dead birds
            foreach (var bird in deadBirds)
            {
                if (bird is Bird)
                {

                    SerializableWeights weights = new SerializableWeights(((Bird)bird).brain.GetGenes());
                    newWeightsList.Add(weights);
                }
            }

            ser.Serialize(fStream, newWeightsList);
            fStream.Close();

            Console.WriteLine("Network genes saved!");
        }

        private static void LoadNetworks()
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<SerializableWeights>));
            var fStream = new FileStream(Directory.GetCurrentDirectory() + "/data/pretrained.brains", FileMode.Open, FileAccess.Read, FileShare.None);
            List<SerializableWeights> newWeightsList = (List < SerializableWeights > )ser.Deserialize(fStream);
            int counter = 0;

            foreach (var bird in gameObjects)
            {
                if (bird is Bird)
                {
                    ((Bird)bird).brain.SetGenes(newWeightsList.ElementAt(counter).ToWeightsInfo());
                    counter++;
                }
            }

            Console.WriteLine("Loaded neural network genes");
        }

        private static void EventClosed(object sender, EventArgs e)
        {
            //Environment.Exit(0);
            mainWindow.Close();

            
            gameObjects = new List<GameObject>();
            rand = new Random();
            playerAlive = true;
            playerPoints = 0;
            playerBird = null;
            isPlayerPlaying = false;
            numberOfAliveBirds = 0;
            deadBirds = new List<Bird>();
            highScorePoints = 0;
            isPretrained = false;
            bestNetworkWeightsAlltime = null;
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

            // delete objects that are out of bounds or dead
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


            // update scores, highscore and keep track of the best performing bird from all previous generations
            double maxScore = 0;
            double scoreSum = 0;
            foreach (var bird in deadBirds)
            {
                if (bird.Points > maxScore)
                {
                    maxScore = bird.Points;
                    bestNetworkWeightsAlltime = new WeightsInfo(bird.brain.Network.Weights.weights1, bird.brain.Network.Weights.weights2);
                }
                scoreSum += bird.Points;
            }

            // add birds with their pick probability depending on their score
            List<KeyValuePair<Bird, double>> elements = new List<KeyValuePair<Bird, double>>();
            foreach (Bird bird in deadBirds)
            {
                elements.Add(new KeyValuePair<Bird, double>(bird, bird.Points / scoreSum));
            }



            // pick new birds, higher scoring birds have greater chances of being picked
            Bird pickedBird = null;
            for (int i = 0; i < WorldRules.BirdsPerGeneration -1; i++)
            {
                Bird bird = new Bird();

                double diceRoll = rand.NextDouble();


                double cumulative = 0.0;
                for (int j = 0; j < elements.Count; j++)
                {
                    cumulative += elements[j].Value;
                    if (diceRoll < cumulative)
                    {
                        pickedBird = elements[j].Key;
                        break;
                    }
                }

                pickedBird.brain.Network.Mutate();
                bird.brain.SetGenes(pickedBird.brain.GetGenes());
                bird.brain = pickedBird.brain;

                
                // in case all birds do very poorly, reset every bird to a new network
                if (maxScore < 200)
                {
                    bird = new Bird();
                }
                gameObjects.Add(bird);
                playerBird = bird;
            }

            // elitism, the best bird from all generations is always picked
            Bird bestBirdAlltime = new Bird();
            bestBirdAlltime.brain.SetGenes(bestNetworkWeightsAlltime);
            gameObjects.Add(bestBirdAlltime);

            numberOfAliveBirds = WorldRules.BirdsPerGeneration;



            // we reset the whole run, this includes new pipes
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
                // make sure to load in pretrained networks if load is selected
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
                // in case this is a fresh start, add new birds with random networks
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

            // add the pipes at the end

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
            if (!isPlayerPlaying)
            {
                mainWindow.Draw(help);
                mainWindow.Draw(help2);
            }


            mainWindow.Display();
        }
    }
}
