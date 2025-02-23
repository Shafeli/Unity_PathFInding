using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int kMoveStraightCost = 10;
    private const int kMoveDiagonalCost = 14; // 10 * sqrt(2)

    [SerializeField] private int _worldGridMapWidth = 4;
    [SerializeField] private int _worldGridMapHeight = 4;
    [SerializeField] private float _worldGridMapCellSize = 10.0f;
    [SerializeField] private bool _debugWorldGridValue = true;
    [SerializeField] private bool _debugWorldGridLines = true;

    // private Grid<int> _grid;

    private List<PathCell> _openCells;
    private List<PathCell> _closedCells;
    private Grid<PathCell> _pathFindingGrid;

    void Start()
    {
        _pathFindingGrid = new Grid<PathCell>(_worldGridMapWidth, _worldGridMapHeight, _worldGridMapCellSize,
            transform.position, gameObject);

        _pathFindingGrid.ToggleValueText(_debugWorldGridValue);
        var cells = _pathFindingGrid.GetCellsList();

        foreach (var cell in cells)
        {
            cell.UserValue = new PathCell(_pathFindingGrid, cell.X, cell.Y);
        }
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            //_grid.SetCellValue(GeneralUtility.MouseUtility.GetWorldPosition(), 500);

            int x, y;
            Vector3 mousePosition = GeneralUtility.MouseUtility.GetWorldPosition();
            //_grid.XY(mousePosition, out x, out y);
            _pathFindingGrid.XY(mousePosition, out x, out y);
            //Debug.Log("Mouse Click Position was over index: " + _grid.CellIndex(x, y));
            var path = FindPath(_pathFindingGrid.WorldPosition(0,0), mousePosition);

            if (path != null && path.Count > 1) // Ensure there are at least two points to draw lines
            {
                for (int i = 0; i < path.Count - 1; i++) // Loop up to the second-to-last cell
                {
                    Vector3 start = path[i];
                    Vector3 end = path[i + 1];
                    Debug.DrawLine(start, end, Color.green, 100.0f);
                }
            }
        }

        _pathFindingGrid.UpdateGrid();

    }

    void OnGUI()
    {
        if (_debugWorldGridLines)
            _pathFindingGrid.DrawDebugLines();
    }

    public List<Vector3> FindPath(Vector3 startWorld, Vector3 endWorld)
    {
        _pathFindingGrid.XY(startWorld, out int startX, out int startY);
        _pathFindingGrid.XY(endWorld, out int endX, out int endY);

        List<PathCell> path = FindPath(startX, startY, endX, endY);
        if (path == null) return null;

        List<Vector3> vPath = new List<Vector3>();
        foreach (var cell in path)
        {
            vPath.Add(_pathFindingGrid.WorldPosition(cell._x, cell._y));
        }

        return vPath;

    }
    public List<PathCell> FindPath(int startX, int startY, int endX, int endY)
    {
        PathCell startCell = _pathFindingGrid.GetCellValue(startX, startY);

        _openCells = new List<PathCell> { startCell };
        _closedCells = new List<PathCell>();

        foreach (var cell in _pathFindingGrid.GetCellsList())
        {
            cell.UserValue._gCost = int.MaxValue;
            cell.UserValue.CalculateFCost();
            cell.UserValue.LastCell = null;

        }


        startCell._gCost = 0;
        startCell._hCost = CalculateDistanceCost(startCell, _pathFindingGrid.GetCellValue(endX, endY));
        startCell.CalculateFCost();

        while (_openCells.Count > 0)
        {
            PathCell currentCell = GetLowestFCostCell();
            if (currentCell == _pathFindingGrid.GetCellValue(endX, endY)) // if reached the end
            {
                return CalculatePath(currentCell);
            }

            _openCells.Remove(currentCell);
            _closedCells.Add(currentCell);

            // Check all the neighbours of the current cell
            var neighbours = GetNeighbours(currentCell);
            foreach (var neighbour in neighbours)
            {
                // Check is the neighbour is already checked
                if (_closedCells.Contains(neighbour))
                {
                    continue;
                }

                // Get the distance between the current cell and the neighbour
                int tentativeGCost = currentCell._gCost + CalculateDistanceCost(currentCell, neighbour);

                // Check if the cost on neighbour is less than the current cost
                if (tentativeGCost < neighbour._gCost)
                {
                    // Set the last cell to the current cell
                    neighbour.LastCell = currentCell;
                    neighbour._gCost = tentativeGCost;
                    neighbour._hCost = CalculateDistanceCost(neighbour, _pathFindingGrid.GetCellValue(endX, endY));
                    neighbour.CalculateFCost();

                    // Check if the neighbour
                    if (!_openCells.Contains(neighbour))
                    {
                        _openCells.Add(neighbour);
                    }
                }
            }

        }

        return null; // Return null no path is found
    }

    private List<PathCell> GetNeighbours(PathCell currentCell)
    {
        List<PathCell> neighbours = new List<PathCell>();

        // directions for neighbors
        int[,] directions = new int[,]
        {
            {-1, -1}, {0, -1}, {1, -1},     // Top-left, Top, Top-right
            {-1,  0}, /*inCell*/ {1,  0},   // Left,       Right
            {-1,  1}, {0,  1}, {1,  1}      // Bottom-left, Bottom, Bottom-right
        };

        // Loop through each direction to check for valid neighbors
        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int checkX = currentCell._x + directions[i, 0];  // New X position
            int checkY = currentCell._y + directions[i, 1];  // New Y position

            // Check ifis inside 
            if (IsInsideGrid(checkX, checkY))
            {
                // Add the valid neighbor to the list
                neighbours.Add(_pathFindingGrid.GetCellValue(checkX, checkY));
            }
        }

        return neighbours; // Return the list of valid neighbors
    }
    private bool IsInsideGrid(int x, int y)
    {
        return x >= 0 && x < _pathFindingGrid.GetWidth() && y >= 0 && y < _pathFindingGrid.GetHeight();
    }
    
    private List<PathCell> CalculatePath(PathCell endCell)
    {
        // Starting at the end calculate a path back to the start
        List<PathCell> path = new List<PathCell>();
        path.Add(endCell);
        PathCell currentCell = endCell;

        while (currentCell.LastCell != null)
        {
            path.Add(currentCell.LastCell);
            currentCell = currentCell.LastCell;
        }

        // Path totalled up flip and return results
        path.Reverse(); 
        return path;
    }

    private PathCell GetLowestFCostCell()
    {
        PathCell lowestFCostCell = _openCells[0];
        for (int i = 1; i < _openCells.Count; i++)
        {
            if (_openCells[i]._fCost < lowestFCostCell._fCost)
            {
                lowestFCostCell = _openCells[i];
            }
        }
        return lowestFCostCell;
    }

    private int CalculateDistanceCost(PathCell startCell, PathCell getCellValue)
    {
        // Quick ay to calculate dist horizontal as much as possible then vertical
        int xDistance = Mathf.Abs(startCell._x - getCellValue._x);
        int yDistance = Mathf.Abs(startCell._y - getCellValue._y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        // The amount of diagonal moves is the minimum of xDistance and yDistance
        return kMoveDiagonalCost * Mathf.Min(xDistance, yDistance) + kMoveStraightCost * remaining;
    }
}
