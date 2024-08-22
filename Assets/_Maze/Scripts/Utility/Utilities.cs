using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Utility
{
    public static class Utilities
    {
        public enum CellType { None, Wall, Floor, Solution }
        public enum WallDirection { North, East, South, West }
    }
}
