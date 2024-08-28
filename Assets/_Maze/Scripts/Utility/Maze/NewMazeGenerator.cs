using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrixieGames.Maze.Utility;
using static Codice.CM.Common.CmCallContext;
using UnityEditor.Experimental.GraphView;

namespace TrixieGames.Maze
{
    public class NewMazeGenerator : MonoBehaviour
    {
        public int m_mazeWidth;
        public int m_mazeHeight;
        public GameObject m_mazeCellPrefab;

        private Stack<Vector2Int> m_stack;
        private MazeCell[,] m_maze;
        private List<MazeCell> m_cellList;
        private int m_numCellsRemaining;

        private void Start()
        {
            InitMaze();
            StartNewMazeWalk();
        }

        private void InitMaze()
        {
            Random.InitState(Utilities.RandomSeed);
            m_stack = new Stack<Vector2Int>();
            m_maze = new MazeCell[m_mazeWidth, m_mazeHeight];
            m_cellList = new List<MazeCell>(m_maze.Length);
            m_numCellsRemaining = m_maze.Length;

            for (int r = 0; r < m_mazeWidth; r++)
            {
                for (int c = 0; c < m_mazeHeight; c++)
                {
                    GameObject newCell = Instantiate(m_mazeCellPrefab, this.gameObject.transform.position + new Vector3(r * 4, 0f, c * 4), Quaternion.identity, this.gameObject.transform);
                    m_maze[r, c] = newCell.GetComponent<MazeCell>();
                    m_maze[r, c].SetupMazeCell(new Vector2Int(r, c), m_mazeWidth, m_mazeHeight);
                    m_cellList.Add(m_maze[r, c]);
                }
            }

            //initalize Random Cell for first cell to hit
            Vector2Int randomCellInit = GetRandomCell();
            m_cellList.Remove(m_maze[randomCellInit.x, randomCellInit.y]);
            m_numCellsRemaining -= 1;
            InitCell(randomCellInit);
        }

        private void StartNewMazeWalk()
        {
            if (m_numCellsRemaining > 0)
            {
                Vector2Int m_randomStartCell = GetRandomCell();
                m_numCellsRemaining -= 1;
                InitCell(m_randomStartCell);
                m_stack.Push(m_randomStartCell);

                WalkThruMaze(m_randomStartCell);
            }
        }

        private void InitCell(Vector2Int _cell)
        {
            m_maze[_cell.x, _cell.y].m_isPlacedInMaze = true;
            m_cellList.Remove(m_maze[_cell.x, _cell.y]);
        }

        private Vector2Int GetRandomCell()
        {
            Vector2Int returnCell = new Vector2Int(-1, -1);

            if (m_cellList.Count > 0 && m_numCellsRemaining > 0)
            {
                MazeCell randomCell = m_cellList[Random.Range(0, m_cellList.Count)];
                returnCell = randomCell.m_coordinates;

                if (randomCell.m_isPlacedInMaze == true)
                    Debug.LogError("ILLEGAL CELL RETURNED FROM MazeGenerator.GetRandomCell()");
            }

            return returnCell;
        }

        private void WalkThruMaze(Vector2Int _currCell)
        {
            Vector2Int currentCell = m_stack.Peek();

            List<Vector2Int> neighbors = m_maze[currentCell.x, currentCell.y].neighbors;

            if (neighbors.Count == 0 && m_numCellsRemaining > 0)
            {
                m_stack.Pop();
                return;
            }
            if (m_numCellsRemaining > 0)
            {
                Vector2Int randomNeighbor = neighbors[Random.Range(0, neighbors.Count)];

                if (m_maze[randomNeighbor.x, randomNeighbor.y].m_isPlacedInMaze == true)
                {
                    CarvePassage(currentCell, randomNeighbor);
                    EndWalk();
                    StartNewMazeWalk();
                }
                else
                {
                    m_numCellsRemaining -= 1;
                    m_stack.Push(randomNeighbor);

                    CarvePassage(currentCell, randomNeighbor);
                    WalkThruMaze(randomNeighbor);
                    m_maze[currentCell.x, currentCell.y].RemoveNeighbor(randomNeighbor);
                }


            }

        }

        private List<Vector2Int> GetValidNeighbors(Vector2Int cell)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();
            int nX, nY;

            nX = cell.x - 1;
            if (nX >= 0 && nX < m_mazeWidth && m_stack.Contains(m_maze[nX, cell.y].m_coordinates) == false)
            {
                neighbors.Add(new Vector2Int(nX, cell.y));
            }

            nX = cell.x + 1;
            if (nX >= 0 && nX < m_mazeWidth && m_stack.Contains(m_maze[nX, cell.y].m_coordinates) == false)
            {
                neighbors.Add(new Vector2Int(nX, cell.y));
            }

            nY = cell.y + 1;
            if (nY >= 0 && nY < m_mazeHeight && m_stack.Contains(m_maze[cell.x, nY].m_coordinates) == false)
            {
                neighbors.Add(new Vector2Int(cell.x, nY));
            }

            nY = cell.y - 1;
            if (nY >= 0 && nY < m_mazeHeight && m_stack.Contains(m_maze[cell.x, nY].m_coordinates) == false)
            {
                neighbors.Add(new Vector2Int(cell.x, nY));
            }

            return neighbors;
        }

        private void EndWalk()
        {
            foreach (Vector2Int cell in m_stack)
            {
                InitCell(cell);
            }

            m_stack.Clear();
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
                m_maze[cell1.x, cell1.y].RemoveWall(Utilities.WallDirection.South);
                m_maze[cell2.x, cell2.y].RemoveWall(Utilities.WallDirection.North);
            }
            else
            {
                m_maze[cell1.x, cell1.y].RemoveWall(Utilities.WallDirection.North);
                m_maze[cell2.x, cell2.y].RemoveWall(Utilities.WallDirection.South);
            }
        }

    }
}
