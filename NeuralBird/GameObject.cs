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

        public int JumpRecharge;

        public GameObject()
        {
            Velocity = new Vector2f(0, 0);
            JumpRecharge = 0;
        }

        public void Jump()
        {
            if (JumpRecharge == 0)
            {
                Velocity += new Vector2f(0, WorldRules.JumpForce);
                if (Velocity.Y < -WorldRules.MaxSpeed)
                {
                    Velocity = new Vector2f(0, -WorldRules.MaxSpeed);
                }
                JumpRecharge = WorldRules.JumpRecharge;

            }
        }

        // main loop update function which updates position logic (velocity, position, out of bounds...)
        public void Update()
        {
            // jump tick reset
            if (JumpRecharge > 0)
            {
                JumpRecharge--;
            }

            // apply position change
            Velocity += new Vector2f(0, WorldRules.Gravity);
            if (Velocity.Y > WorldRules.MaxSpeed)
            {
                Velocity = new Vector2f(0, WorldRules.MaxSpeed);
            }
            Position += Velocity;

            // out of bounds check
            if (Position.Y < 0)
            {
                Position = new Vector2f(Position.X, 0);
                Velocity = new Vector2f(0, 0);
            }
            if (Position.Y > WorldRules.WindowHeight)
            {
                Position = new Vector2f(Position.X, WorldRules.WindowHeight);
                Velocity = new Vector2f(0, 0);
            }
        }

        // this function renders the gameobject
        public void Render()
        {
            Sprite.Position = Position;
            Sprite.Rotation = (float)Math.Asin(Velocity.Y / (WorldRules.MaxSpeed * 1.5)) * (float)(180/Math.PI);
            Program.mainWindow.Draw(Sprite);
        }
    }
}
