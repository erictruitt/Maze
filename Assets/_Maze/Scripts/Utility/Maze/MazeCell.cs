using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Maze.Utility.Utilities;

namespace Maze.Utility
{
    public class MazeCell : MonoBehaviour
    {
        public bool wasVisited;
        public List<MazeCell> neighbors;

        [SerializeField]
        private GameObject northWall;
        [SerializeField]
        private GameObject eastWall;
        [SerializeField]
        private GameObject southWall;
        [SerializeField]
        private GameObject westWall;

        public MazeCell()
        {
            neighbors = new List<MazeCell>();
        }

        public MazeCell(Vector2 _position)
        {
            neighbors = new List<MazeCell>();
        }

        public void AddNeighbor(MazeCell _newNeighbor)
        {
            if (neighbors.Contains(_newNeighbor) == false)
            {
                neighbors.Add(_newNeighbor);
            }
        }

        public void RemoveWall(WallDirection _direction)
        {
            switch (_direction)
            {
                case WallDirection.North:
                    Destroy(northWall);
                    break;
                case WallDirection.East:
                    Destroy(eastWall);
                    break;
                case WallDirection.South:
                    Destroy(southWall);
                    break;
                case WallDirection.West:
                    Destroy(westWall);
                    break;
                default:
                    break;

            }
        }

    }
}
