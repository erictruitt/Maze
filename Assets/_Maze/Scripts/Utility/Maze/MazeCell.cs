using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TrixieGames.Maze.Utility.Utilities;

namespace TrixieGames.Maze.Utility
{
    public class MazeCell : MonoBehaviour
    {
        public bool m_isPlacedInMaze;
        public List<Vector2Int> neighbors;
        public Vector2Int m_coordinates;

        [SerializeField]
        private GameObject northWall;
        [SerializeField]
        private GameObject eastWall;
        [SerializeField]
        private GameObject southWall;
        [SerializeField]
        private GameObject westWall;

        public void SetupMazeCell(Vector2Int _coordinates, int _mazeWidth, int _mazeHeight)
        {
            m_coordinates = _coordinates;
            neighbors = new List<Vector2Int>();

            SetupNeighbors(_mazeWidth, _mazeHeight);
        }

        private void SetupNeighbors(int _mazeWidth, int _mazeHeight)
        {
            int nX, nY;

            nX = m_coordinates.x - 1;
            if (nX >= 0 && nX < _mazeWidth)
            {
                neighbors.Add(new Vector2Int(nX, m_coordinates.y));
            }

            nX = m_coordinates.x + 1;
            if (nX >= 0 && nX < _mazeWidth)
            {
                neighbors.Add(new Vector2Int(nX, m_coordinates.y));
            }

            nY = m_coordinates.y + 1;
            if (nY >= 0 && nY < _mazeHeight)
            {
                neighbors.Add(new Vector2Int(m_coordinates.x, nY));
            }

            nY = m_coordinates.y - 1;
            if (nY >= 0 && nY < _mazeHeight)
            {
                neighbors.Add(new Vector2Int(m_coordinates.x, nY));
            }
        }

        public void RemoveNeighbor(Vector2Int _newNeighbor)
        {
            if (neighbors.Contains(_newNeighbor) == true)
            {
                neighbors.Remove(_newNeighbor);
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
