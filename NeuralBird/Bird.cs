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
    class Bird : GameObject
    {
        public int Points;
        public bool IsDead;

        public Bird()
        {
            Velocity = new Vector2f(0, 0);
            JumpRecharge = 0;
            Points = 0;
            IsDead = false;


            Sprite = new Sprite();
            Texture texture = new Texture(Directory.GetCurrentDirectory() + "/data/bird.png");
            Sprite.Texture = texture;
            Sprite.Origin = new Vector2f(16, 16);
            Sprite.Color = new Color((byte)(Program.rand.Next() % 255), (byte)(Program.rand.Next() % 255), (byte)(Program.rand.Next() % 255));
            Position = new Vector2f(100, 100);
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

        public void CheckForCollision()
        {
            foreach (var obj in Program.gameObjects)
            {
                if ((obj.Position.X - 32) - (Position.X + 16) < 0 && (obj.Position.X + 32) - (Position.X - 16) > 0)
                {
                    if (Math.Sqrt(Math.Pow(obj.Position.X - Position.X, 2) + Math.Pow(obj.Position.Y - Position.Y, 2)) > WorldRules.PipeGap - 16)
                    {
                        // collision detected!
                        IsDead = true;

                        // check if player playing
                        Program.playerAlive = false;
                    }
                }
            }
        }

        public override void Update()
        {

            // points for being alive
            Points++;


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

            CheckForCollision();
        }

        public override void Render()
        {
            Sprite.Position = Position;
            Sprite.Rotation = (float)Math.Asin(Velocity.Y / (WorldRules.MaxSpeed * 1.5)) * (float)(180 / Math.PI);
            Program.mainWindow.Draw(Sprite);
        }
    }
}
