﻿using System;
using System.Linq;
using System.Text;
using Assets.GameAssets.Scripts.Maze.Helper;
using Assets.GameAssets.Scripts.Maze.Model;

namespace Assets.GameAssets.Scripts.Maze.MazeGeneration
{
    public class RecursiveBacktrackerAlgorithm: IRecursiveBacktrackerAlgorithm
    {
        private readonly IDirectionsFlagParser _directionsFlagParser;
        private readonly IRandomPointGenerator _randomPointGenerator;

        public RecursiveBacktrackerAlgorithm(IDirectionsFlagParser directionsFlagParser, IRandomPointGenerator randomPointGenerator)
        {
            _directionsFlagParser = directionsFlagParser;
            _randomPointGenerator = randomPointGenerator;
        }

        public AlgorithmRunResults GenerateMaze(IMazeCarver maze, MazeGenerationSettings settings)
        {
            var randomPoint = _randomPointGenerator.RandomPoint(maze.Size);
            maze.JumpToPoint(randomPoint);
            RecursiveBackTracker(maze);
            return new AlgorithmRunResults
            {
                Carver = maze
            };
        }

        public void RecursiveBackTracker(IMazeCarver carver)
        {
            var directions = carver.CarvableDirections().ToList();
            directions.Shuffle();
            var currentPoint = carver.CurrentPoint;
            foreach (var direction in directions)
            {
                carver.JumpInDirection(direction);
                var carvedDirections = carver.AlreadyCarvedDirections();
                if (!carvedDirections.Any())
                {
                    var oppositeDirection = _directionsFlagParser.OppositeDirection(direction);
                    carver.CarveInDirection(oppositeDirection);
                    RecursiveBackTracker(carver);
                }
                carver.JumpToPoint(currentPoint);
            }
        }
    }
}