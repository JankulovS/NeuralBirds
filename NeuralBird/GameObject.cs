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
        }



        // main loop update function which updates position logic (velocity, position, out of bounds...)
        public virtual void Update()
        {
            
        }

        // this function renders the gameobject
        public virtual void Render()
        {
        }
    }
}
