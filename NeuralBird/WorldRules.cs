using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralBird
{
    public static class WorldRules
    {
        public const float Gravity = 0.2f;
        public const float MaxSpeed = 10;
        public const float JumpForce = -7f;
        public const int JumpRecharge = 10;

        public const uint WindowHeight = 600;
        public const uint WindowWidth = 1000;
    }
}
