using TrixieGames.Maze.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrixieGames.Maze.Utility
{
    public class MazeGenerator : MonoBehaviour
    {
        public int m_randomSeed;
        public int width;
        public int height;
        public GameObject mazeCellPrefab;
        public GameObject floorPrefab;
        public GameObject solutionPrefab;

        //private Stack<Vector2Int> stack;
        //private Vector2Int startPosition;
        private MazeCell[,] maze;
        int unvisitedMazeCells;
        private float m_deadEndProbability = 0.25f;

        Stack<Vector2Int> currStack = new Stack<Vector2Int>();
        Vector2Int current;
        Vector2Int randomStartCell;

        void Start()
        {
            Random.InitState(m_randomSeed);
            InitializeMaze();
            GenerateMaze();
        }

        private void InitializeMaze()
        {
            maze = new MazeCell[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    GameObject newCell = Instantiate(mazeCellPrefab, this.gameObject.transform.position + new Vector3(i * 4, 0f, j * 4), Quaternion.identity, this.gameObject.transform);
                    maze[i, j] = newCell.GetComponent<MazeCell>();
                }
            }

            unvisitedMazeCells = maze.Length;

            //startPosition = new Vector2Int(width / 2, height / 2);
            //Instantiate(solutionPrefab, new Vector3(startPosition.x, 0f, startPosition.y), Quaternion.identity);
        }

        private void GenerateMaze()
        {
            //initalize Random Cell for first cell to hit
            Vector2Int randomCellInit = new Vector2Int(Random.Range(0, width - 1), Random.Range(0, height - 1));
            maze[randomCellInit.x, randomCellInit.y].m_isPlacedInMaze = true;
            AdjustUnvisitedCellCount(-1);


            while (unvisitedMazeCells > 0)
            {
                //pick random start position
                randomStartCell = GetRandomStartCell();
                //maze[randomStartCell.x, randomStartCell.y].wasVisited = true;
                currStack.Push(randomStartCell);
                AdjustUnvisitedCellCount(-1);

                RunGenerationLoop(randomStartCell);
            }
        }

        private void AdjustUnvisitedCellCount(int _adjustment)
        {
            if (_adjustment > 0)
            {
                unvisitedMazeCells += _adjustment;
            }
            else
            {
                unvisitedMazeCells -= _adjustment;
            }
        }


        private void RunGenerationLoop(Vector2Int _currCell)
        {

            current = currStack.Peek();

            List<Vector2Int> unvisitedNeighbors = GetUnvisitedNeighbors(current);

            if (unvisitedNeighbors.Count > 0)
            {
                Vector2Int randomNeighbor = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                Debug.Log("Random Neighbor: " + randomNeighbor);

                if (maze[randomNeighbor.x, randomNeighbor.y].m_isPlacedInMaze == false)
                {
                    Debug.Log("Random Neightbor was ok to add");
                    //maze[randomNeighbor.x, randomNeighbor.y].wasVisited = true;
                    AdjustUnvisitedCellCount(-1);
                    CarvePassage(current, randomNeighbor);
                    currStack.Push(randomNeighbor);
                    RunGenerationLoop(randomNeighbor);

                }
                else if (currStack.Contains(randomNeighbor))
                {
                    Debug.Log("Random Neightbor was ok to add");
                    currStack.Pop();
                    return;
                }
                else
                {
                    return;
                }
            }
            //else
            //{
            //    currStack.Pop();
            //    return;
            //}

            //Vector2Int current = stack.Peek();


            //if (neighbors.Count > 0) {
            //    Vector2Int randomNeighbor = neighbors[Random.Range(0, neighbors.Count)];
            //    maze[randomNeighbor.x, randomNeighbor.y].wasVisited = true;
            //    CarvePassage(current, randomNeighbor);
            //    stack.Push(randomNeighbor);
            //}
            //else {
            //    stack.Pop();
            //}

            //Get next randomStartCell
            //randomStartCell = GetRandomStartCell();
            //maze[randomStartCell.x, randomStartCell.y].wasVisited = true;
            //AdjustUnvisitedCellCount(-1);
        }

        private void Walk()
        {
            Vector2Int randomStartCell = GetRandomStartCell();

            if (randomStartCell.x == -1)
                return;



        }

        private Vector2Int GetRandomStartCell()
        {
            Vector2Int randomCell = new Vector2Int(-1, -1);

            if (unvisitedMazeCells > 0)
            {
                randomCell = new Vector2Int(Random.Range(0, width - 1), Random.Range(0, height - 1));

                while (maze[randomCell.x, randomCell.y].m_isPlacedInMaze == true)
                {
                    randomCell = new Vector2Int(Random.Range(0, width - 1), Random.Range(0, height - 1));
                }
            }

            return randomCell;
        }

        private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int cell)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();

            //if (cell.x > 0 && currStack.Contains(new Vector2Int(cell.x - 1, cell.y)) == false)
            //{
            //    neighbors.Add(new Vector2Int(cell.x - 1, cell.y));
            //}
            //if (cell.x < width - 1 && currStack.Contains(new Vector2Int(cell.x + 1, cell.y)) == false)
            //{
            //    neighbors.Add(new Vector2Int(cell.x + 1, cell.y));
            //}
            //if (cell.y > 0 && currStack.Contains(new Vector2Int(cell.x, cell.y - 1)) == false)
            //{
            //    neighbors.Add(new Vector2Int(cell.x, cell.y - 1));
            //}
            //if (cell.y < height - 1 && currStack.Contains(new Vector2Int(cell.x, cell.y + 1)) == false)
            //{
            //    neighbors.Add(new Vector2Int(cell.x, cell.y + 1));
            //}

            int absNegX = Mathf.Abs(cell.x - 1);
            int absPosX = Mathf.Abs(cell.x + 1);

            int absNegY = Mathf.Abs(cell.y - 1);
            int absPosY = Mathf.Abs(cell.y + 1);



            if (absNegX < width - 1)
            {
                neighbors.Add(new Vector2Int(cell.x - 1, cell.y));
            }
            if (absPosX < width - 1)
            {
                neighbors.Add(new Vector2Int(cell.x + 1, cell.y));
            }
            if (absNegY < height - 1)
            {
                neighbors.Add(new Vector2Int(cell.x, cell.y - 1));
            }
            if (absPosY < height - 1)
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
