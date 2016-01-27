﻿using System.Collections.Generic;
using System.Linq;
using MazeGeneration.Factory;
using MazeGeneration.Helper;

namespace MazeGeneration.Model
{
    public class MazeModel3 : MazeBase, IBuilder
    {
        private readonly IMazePointFactory _pointFactory;
        private readonly IMovementHelper _movementHelper;
        private Dictionary<MazePoint, MazeCell> Maze { get; set; }

        public MazeModel3(IDirectionsFlagParser flagParser, IMazePointFactory pointFactory, IMovementHelper movementHelper) : base(flagParser)
        {
            _pointFactory = pointFactory;
            _movementHelper = movementHelper;
        }

        protected override void Initialise(MazeSize size, bool allVertexes)
        {
            Size = size;
            Maze = new Dictionary<MazePoint, MazeCell>();
            var width = Enumerable.Range(0, size.Width).ToList();
            var height = Enumerable.Range(0, size.Height).ToList();
            var depth = Enumerable.Range(0, size.Depth).ToList();
            foreach (var point in width.SelectMany(x =>  height.SelectMany(y => depth.Select(z => _pointFactory.MakePoint(x, y, z)))))
            {
                Maze.Add(point, new MazeCell
                {
                    Directions = Direction.None
                });
            }
        }

        public void PlaceVertex(MazePoint p, Direction d)
        {
            MazeCell startCell;
            MazePoint final;
            MazeCell finalCell;
            if (Maze.TryGetValue(p, out startCell) && _movementHelper.CanMove(p, d, Size, out final) && Maze.TryGetValue(final, out finalCell))
            {
                startCell.Directions = FlagParser.AddDirectionsToFlag(startCell.Directions, d);
                finalCell.Directions = FlagParser.AddDirectionsToFlag(finalCell.Directions,
                    FlagParser.OppositeDirection(d));
            }
        }

        public void RemoveVertex(MazePoint p, Direction d)
        {
            MazeCell startCell;
            MazePoint final;
            MazeCell finalCell;
            if (Maze.TryGetValue(p, out startCell) && _movementHelper.CanMove(p, d, Size, out final) && Maze.TryGetValue(final, out finalCell))
            {
                startCell.Directions = FlagParser.RemoveDirectionsFromFlag(startCell.Directions, d);
                finalCell.Directions = FlagParser.RemoveDirectionsFromFlag(finalCell.Directions,
                    FlagParser.OppositeDirection(d));
            }
        }

        public override Direction GetFlagFromPoint(MazePoint p)
        {
            MazeCell startCell;
            if (Maze.TryGetValue(p, out startCell))
            {
                return startCell.Directions;
            }
            throw new CellNotFoundException();
        }
    }
}