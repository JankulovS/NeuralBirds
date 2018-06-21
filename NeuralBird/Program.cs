using ArtificialNeuralNetwork;
using ArtificialNeuralNetwork.Factories;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralBird
{


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
        public static int numberOfAliveBirds = 0;
        public static List<Bird> deadBirds = new List<Bird>();

        static void Main(string[] args)
        {


            PlayerChoiceMenu();


            mainWindow = new RenderWindow(new VideoMode(WorldRules.WindowWidth, WorldRules.WindowHeight), "Neural Bird");
            mainWindow.SetFramerateLimit(60);
            score = new Text();
            score.Position = new Vector2f(60, WorldRules.WindowHeight - 60);
            score.Color = Color.Black;
            Font font = new Font(Directory.GetCurrentDirectory() + "/data/font.ttf");
            score.Font = font;
            score.CharacterSize = 32;
            score.DisplayedString = playerPoints.ToString();
            mainWindow.Closed += EventClosed;

            // player plays
            if (isPlayerPlaying)
            {
                mainWindow.KeyPressed += EventKeyPressed;
                playerAlive = true;
            }



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
                Console.WriteLine("2 - let neural network play");
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
            if (e.Code == Keyboard.Key.Space)
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
        }


        private static void EventClosed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        static void MainLoop()
        {
            while (mainWindow.IsOpen())
            {
                // Process events
                mainWindow.DispatchEvents();


                // update logic loop
                UpdateWorld();

                // update graphics
                UpdateGraphics();

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

            playerPoints = playerBird.Points;

            for(int i = 0; i < gameObjects.Count; i++)
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

            // sort by points
            deadBirds = deadBirds.OrderBy(o => o.Points).ToList();
            deadBirds.Reverse();

            Bird obj;
            for (int i = 0; i < WorldRules.BirdsPerGeneration; i++)
            {
                obj = new Bird();

                int chosenBirdBrain = i / 2;

                INeuralNetwork network;
                network = NeuralNetworkFactory.GetInstance().Create(deadBirds.ElementAt(chosenBirdBrain).brain.network.GetGenes());
                obj.brain.network = network;


                obj.brain.Mutate();
                gameObjects.Add(obj);
                playerBird = obj;
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
            deadBirds = new List<Bird>();
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
                Bird obj;
                for (int i = 0; i < WorldRules.BirdsPerGeneration; i++)
                {
                    obj = new Bird();
                    gameObjects.Add(obj);
                    playerBird = obj;
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
            score.DisplayedString = playerPoints.ToString();
            mainWindow.Draw(score);


            mainWindow.Display();
        }
    }
}
