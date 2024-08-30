using System.Collections.Generic;
using UnityEngine;
using TrixieGames.Maze.Utility;

namespace TrixieGames.Maze
{
    public class MazeGenerator : MonoBehaviour
    {
        public int m_mazeWidth;
        public int m_mazeHeight;
        public GameObject m_mazeCellPrefab;

        private MazeCell[,] m_maze;
        private List<MazeCell> m_cellsAvailable;
        Stack<MazeCell> m_stack = new Stack<MazeCell>();

        private void Start()
        {
            InitMaze();
            GenerateMaze();
        }

        private void InitMaze()
        {
            Random.InitState(Utilities.RandomSeed);
            m_maze = new MazeCell[m_mazeWidth, m_mazeHeight];
            m_cellsAvailable = new List<MazeCell>(m_maze.Length);

            for (int r = 0; r < m_mazeWidth; r++)
            {
                for (int c = 0; c < m_mazeHeight; c++)
                {
                    GameObject newCell = Instantiate(m_mazeCellPrefab, this.gameObject.transform.position + new Vector3(r * 4, 0f, c * 4), Quaternion.identity, this.gameObject.transform);
                    newCell.name = r + "," + c;
                    m_maze[r, c] = newCell.GetComponent<MazeCell>();
                    m_maze[r, c].SetupMazeCell(new Vector2Int(r, c), m_mazeWidth, m_mazeHeight);
                    m_cellsAvailable.Add(m_maze[r, c]);
                }
            }

            MazeCell randomCellInit = GetRandomCell();
            InitCell(randomCellInit.m_coordinates);
        }

        private MazeCell GetRandomCell()
        {
            MazeCell returnCell = null;

            if (m_cellsAvailable.Count > 0)
            {
                returnCell = m_cellsAvailable[Random.Range(0, m_cellsAvailable.Count)];
            }

            return returnCell;
        }

        private void InitCell(Vector2Int _coordinates)
        {
            MazeCell cell = GetCellFromMazeArray(_coordinates);

            cell.m_isPlacedInMaze = true;
            m_cellsAvailable.Remove(cell);
        }

        private MazeCell GetCellFromMazeArray(Vector2Int _coordinates)
        {
            return m_maze[_coordinates.x, _coordinates.y];
        }

        private void GenerateMaze()
        {
            MazeCell currentCell;
            List<Vector2Int> currNeighbors;
            Vector2Int randomNeighbor;

            while (m_cellsAvailable.Count > 0)
            {
                currentCell = GetRandomCell();

                while (currentCell.m_isPlacedInMaze == false)
                {
                    if (m_stack.Contains(currentCell) == false)
                    {
                        m_stack.Push(currentCell);
                    }

                    currNeighbors = currentCell.neighbors;

                    if (currNeighbors.Count > 0)
                    {
                        randomNeighbor = currNeighbors[Random.Range(0, currNeighbors.Count)];

                        RemoveNeighbors(currentCell, randomNeighbor);

                        if (m_stack.Contains(GetCellFromMazeArray(randomNeighbor)) == false)
                        {
                            currentCell = GetCellFromMazeArray(randomNeighbor);
                        }
                    }
                    else
                    {
                        MazeCell deadEndCell = m_stack.Pop();
                        currentCell = m_stack.Peek();
                        deadEndCell.AddNeighbor(currentCell.m_coordinates);
                        RemoveNeighbors(currentCell, deadEndCell.m_coordinates);
                    }
                }
                m_stack.Push(currentCell);

                int stackCount = m_stack.Count;
                MazeCell popCell, peekCell;
                for (int i = 0; i < stackCount - 1; i++)
                {
                    popCell = m_stack.Pop();
                    peekCell = m_stack.Peek();
                    CarvePassage(popCell.m_coordinates, peekCell.m_coordinates);
                    InitCell(popCell.m_coordinates);
                }
                popCell = m_stack.Pop();
                InitCell(popCell.m_coordinates);
            }
        }

        private void RemoveNeighbors(MazeCell _current, Vector2Int _neighbor)
        {
            _current.RemoveNeighbor(_neighbor);
        }

        private void CarvePassage(Vector2Int cell1, Vector2Int cell2)
        {
            int dx = cell2.x - cell1.x;
            int dy = cell2.y - cell1.y;

            if (dx > 0)
            {
                m_maze[cell1.x, cell1.y].RemoveWall(Utilities.WallDirection.East);
                m_maze[cell2.x, cell2.y].RemoveWall(Utilities.WallDirection.West);
            }
            else if (dx < 0)
            {
                m_maze[cell1.x, cell1.y].RemoveWall(Utilities.WallDirection.West);
                m_maze[cell2.x, cell2.y].RemoveWall(Utilities.WallDirection.East);
            }
            else if (dy > 0)
            {
                m_maze[cell1.x, cell1.y].RemoveWall(Utilities.WallDirection.North);
                m_maze[cell2.x, cell2.y].RemoveWall(Utilities.WallDirection.South);
            }
            else
            {
                m_maze[cell1.x, cell1.y].RemoveWall(Utilities.WallDirection.South);
                m_maze[cell2.x, cell2.y].RemoveWall(Utilities.WallDirection.North);
            }
        }

    }
}
