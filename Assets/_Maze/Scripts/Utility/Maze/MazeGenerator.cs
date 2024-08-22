using Maze.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Utility
{
    public class MazeGenerator : MonoBehaviour
    {
        public int m_randomSeed = 42;
        public int width = 10;
        public int height = 10;
        public GameObject mazeCellPrefab;
        public GameObject floorPrefab;
        public GameObject solutionPrefab;

        private Stack<Vector2Int> stack;
        private Vector2Int startPosition;
        private MazeCell[,] maze;
        private float m_deadEndProbability = 0.25f;

        void Start()
        {
            Random.InitState(m_randomSeed);
            InitializeMaze();
            GenerateMaze(startPosition);
        }

        void InitializeMaze()
        {
            maze = new MazeCell[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    GameObject newCell = Instantiate(mazeCellPrefab, new Vector3(i * 4, 0f, j * 4), Quaternion.identity);
                    maze[i, j] = newCell.GetComponent<MazeCell>();
                }
            }

            stack = new Stack<Vector2Int>();

            startPosition = new Vector2Int(width / 2, height / 2);
            Instantiate(solutionPrefab, new Vector3(startPosition.x, 0f, startPosition.y), Quaternion.identity);
            maze[startPosition.x, startPosition.y].wasVisited = true;
        }

        void GenerateMaze(Vector2Int _StartCell)
        {
            stack.Push(_StartCell);

            while (stack.Count > 0)
            {
                Vector2Int current = stack.Peek();

                List<Vector2Int> neighbors = GetUnvisitedNeighbors(current);

                if (neighbors.Count > 0)
                {
                    Vector2Int randomNeighbor = neighbors[Random.Range(0, neighbors.Count)];
                    maze[randomNeighbor.x, randomNeighbor.y].wasVisited = true;
                    CarvePassage(current, randomNeighbor);
                    stack.Push(randomNeighbor);
                }
                else
                {
                    stack.Pop();
                }
            }
        }

        private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int cell)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();

            if (cell.x > 0 && maze[cell.x - 1, cell.y].wasVisited == false)
            {
                neighbors.Add(new Vector2Int(cell.x - 1, cell.y));
            }
            if (cell.x < width - 1 && maze[cell.x + 1, cell.y].wasVisited == false)
            {
                neighbors.Add(new Vector2Int(cell.x + 1, cell.y));
            }
            if (cell.y > 0 && maze[cell.x, cell.y - 1].wasVisited == false)
            {
                neighbors.Add(new Vector2Int(cell.x, cell.y - 1));
            }
            if (cell.y < height - 1 && maze[cell.x, cell.y + 1].wasVisited == false)
            {
                neighbors.Add(new Vector2Int(cell.x, cell.y + 1));
            }

            return neighbors;
        }

        private void CarvePassage(Vector2Int cell1, Vector2Int cell2)
        {
            int dx = cell2.x - cell1.x;
            int dy = cell2.y - cell1.y;

            if (dx > 0)
            {
                maze[cell1.x, cell1.y].RemoveWall(Utilities.WallDirection.East);
                maze[cell2.x, cell2.y].RemoveWall(Utilities.WallDirection.West);
            }
            else if (dx < 0)
            {
                maze[cell1.x, cell1.y].RemoveWall(Utilities.WallDirection.West);
                maze[cell2.x, cell2.y].RemoveWall(Utilities.WallDirection.East);
            }
            else if (dy > 0)
            {
                maze[cell1.x, cell1.y].RemoveWall(Utilities.WallDirection.South);
                maze[cell2.x, cell2.y].RemoveWall(Utilities.WallDirection.North);
            }
            else
            {
                maze[cell1.x, cell1.y].RemoveWall(Utilities.WallDirection.North);
                maze[cell2.x, cell2.y].RemoveWall(Utilities.WallDirection.South);
            }
        }

    }
}
