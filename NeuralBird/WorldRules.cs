﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralBird
{
    public static class WorldRules
    {
        // bird rules
        public const float Gravity = 0.25f;
        public const float MaxSpeed = 10;
        public const float JumpForce = -7f;
        public const int JumpRecharge = 10;

        // pipe rules
        public const float PipeGap = 100f;
        public const float PipeSpeed = 3f;
        public const int NumberOfPipes = 3;

        // neural network rules
        public const int BirdsPerGeneration = 150;
        public const double MutationChance = 0.1;
        public const double MutationSeverity = 0.05;

        // window rules
        public const uint WindowHeight = 600;
        public const uint WindowWidth = 1000;
        public static int SimulationsPerUpdate = 1;
    
        
    }
}
