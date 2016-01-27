﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeGeneration.Model;

namespace MazeGeneration
{
    public interface IMazeJumper: IMaze
    {
        IEnumerable<Direction> JumpableDirections();
        Direction JumpableFlag();

        bool CanJumpInDirection(Direction d);
        bool TryJumpInDirection(Direction d);
        void JumpInDirection(Direction d);

        bool CanJumpToPoint(MazePoint p);
        bool TryJumpToPoint(MazePoint p);
        void JumpToPoint(MazePoint p);

        IMaze JumpingFinished();
    }
}
