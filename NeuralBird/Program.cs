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

        static void Main(string[] args)
        {
            mainWindow = new RenderWindow(new VideoMode(WorldRules.WindowWidth, WorldRules.WindowHeight), "Neural Bird");
            mainWindow.Closed += EventClosed;
            mainWindow.KeyPressed += EventKeyPressed;
            mainWindow.SetFramerateLimit(60);

            // DEBUG
            Bird obj = new Bird();
            gameObjects.Add(obj);

            // END DEBUG

            MainLoop();
            string str = Console.ReadLine();
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
            foreach(var obj in gameObjects)
            {
                obj.Update();
            }
        }

        public static void UpdateGraphics()
        {
            mainWindow.Clear(Color.White);

            foreach(var obj in gameObjects)
            {
                obj.Render();
            }

            mainWindow.Display();
        }
    }
}
