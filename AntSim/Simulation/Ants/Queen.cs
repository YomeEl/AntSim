﻿using AntSim.Simulation.Map;
using System;

namespace AntSim.Simulation.Ants
{
    class Queen : Ant
    {
        public Queen(uint antId, uint factionId, SFML.Graphics.Texture texture, byte width, byte height) :
            base(antId, factionId, texture, width, height)
        {
        }

        public override Direction Move(Cell[,] vicinity)
        {
            throw new NotImplementedException();
        }
    }
}
