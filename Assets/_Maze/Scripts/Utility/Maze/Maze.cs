using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Maze.Utility.Utilities;

namespace Maze.Utility
{
    public class Maze
    {
        public Stack<Vector2Int> m_cells { get; private set; }
        public int[,] m_grid { get; private set; }

        public Maze(int _rows, int _columns)
        {
            m_grid = new int[_rows, _columns];
            for (int i = 0; i < _rows; i++) {
                for (int j = 0; j < _columns; j++)
                {
                    m_grid[i, j] = (int)CellType.Wall;
                }
            }

            m_cells = new Stack<Vector2Int>();
            m_cells.Push(new Vector2Int(Random.Range(0, _rows), Random.Range(0, _columns)));

            while (m_cells.Count > 0)
            {
                Vector2Int current = m_cells.Peek();

                if (current.x == _rows * 0.5f && current.y == _columns * 0.5f)
                {
                    m_grid[current.x, current.y] = (int)CellType.Solution;
                    return;
                }


            }
        }
    }
}