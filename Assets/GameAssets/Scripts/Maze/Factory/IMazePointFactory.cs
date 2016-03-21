﻿using Assets.GameAssets.Scripts.Maze.Model;

namespace Assets.GameAssets.Scripts.Maze.Factory
{
    public interface IMazePointFactory
    {
        MazePoint MakePoint(int x, int y, int z);

        void Store(MazePoint p);
    }
}