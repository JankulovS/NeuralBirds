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
        public static Text score;

        static void Main(string[] args)
        {
            mainWindow = new RenderWindow(new VideoMode(WorldRules.WindowWidth, WorldRules.WindowHeight), "Neural Bird");
            mainWindow.Closed += EventClosed;
            mainWindow.KeyPressed += EventKeyPressed;
            mainWindow.SetFramerateLimit(60);
            score = new Text();
            score.Position = new Vector2f(60, WorldRules.WindowHeight - 60);
            score.Color = Color.Black;
            Font font = new Font(Directory.GetCurrentDirectory() + "/data/font.ttf");
            score.Font = font;
            score.CharacterSize = 32;
            score.DisplayedString = playerPoints.ToString();

            playerAlive = true;

            // DEBUG
            ResetGame();

            // END DEBUG

            MainLoop();
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
            if (playerAlive == false)
            {
                ResetGame();
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
            }
        }

        private static void ResetGame()
        {
            gameObjects.Clear();

            Bird obj = new Bird();
            gameObjects.Add(obj);
            playerBird = obj;

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

            foreach(var obj in gameObjects)
            {
                obj.Render();
            }
            score.DisplayedString = playerPoints.ToString();
            mainWindow.Draw(score);
            mainWindow.Display();
        }
    }
}
