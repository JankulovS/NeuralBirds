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
    class Pipe : GameObject
    {
        public Sprite SpriteBottom;
        public Sprite SpriteTop;
        public bool ForDeletion;

        public Pipe()
        {
            ForDeletion = false;
            Position = new Vector2f(WorldRules.WindowWidth, (WorldRules.WindowHeight / 4) + Program.rand.Next(0, (int)(WorldRules.WindowHeight / 2)));
            Velocity = new Vector2f(-WorldRules.PipeSpeed, 0);

            SpriteBottom = new Sprite();
            SpriteTop = new Sprite();
            Texture textureBottom = new Texture(Directory.GetCurrentDirectory() + "/data/pipe_bottom.png");
            Texture textureTop = new Texture(Directory.GetCurrentDirectory() + "/data/pipe_top.png");
            SpriteBottom.Texture = textureBottom;
            SpriteTop.Texture = textureTop;
            SpriteBottom.Origin = new Vector2f(32, 0);
            SpriteTop.Origin = new Vector2f(32, 500);
            
        }

        public override void Update()
        {
            Position += Velocity;

            if (Position.X < -32)
            {
                ForDeletion = true;
            }
        }

        public override void Render()
        {

            SpriteBottom.Position = Position;
            SpriteTop.Position = Position;

            SpriteBottom.Position += new Vector2f(0, WorldRules.PipeGap);
            SpriteTop.Position += new Vector2f(0, - WorldRules.PipeGap);

            Program.mainWindow.Draw(SpriteBottom);
            Program.mainWindow.Draw(SpriteTop);
        }
    }
}
