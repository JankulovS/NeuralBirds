using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralBird
{
    class GameObject
    {
        public Vector2f Position { get; set; }
        public Sprite Sprite { get; set; }
        public Vector2f Velocity { get; set; }

        public GameObject()
        {
            Velocity = new Vector2f(0, 0);
        }

        public void Jump()
        {
            Velocity += new Vector2f(0, WorldRules.JumpForce);
        }

        public void Update()
        {
            Velocity += new Vector2f(0, WorldRules.Gravity);
            Position += Velocity;
        }

        public void Render()
        {
            Sprite.Position = Position;
            Program.mainWindow.Draw(Sprite);
        }
    }
}
