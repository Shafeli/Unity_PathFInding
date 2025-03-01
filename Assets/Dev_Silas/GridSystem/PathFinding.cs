using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    private const int kMoveStraightCost = 10;
    private const int kMoveDiagonalCost = 14; // 10 * sqrt(2)
    private float _worldGridMapCellSize = 10.0f;

    private List<PathCell> _openCells;
    private List<PathCell> _closedCells;
    private Grid<PathCell> PathFindingGrid { get; }

    public PathFinding
    (
        int _worldGridMapWidth,
        int _worldGridMapHeight,
        float _worldGridMapCellSize,
        Vector3 position,
        GameObject gameObject,
        bool _debugWorldGridValue
    )
    {
        PathFindingGrid = new Grid<PathCell>(_worldGridMapWidth, _worldGridMapHeight, _worldGridMapCellSize,
            position, gameObject, _debugWorldGridValue);

        PathFindingGrid.ToggleValueText(_debugWorldGridValue);
        var cells = PathFindingGrid.GetCellsList();

        foreach (var cell in cells)
        {
            cell.UserValue = new PathCell(PathFindingGrid, cell.X, cell.Y);
        }
    }

    public void UpdateGrid()
    {
        PathFindingGrid.UpdateGrid();
    }

    public Grid<PathCell>.Cell GetCell(int x, int y)
    {
        return PathFindingGrid.GetCell(x, y);
    }

    public void ToggleValueText(bool isOn)
    {
        PathFindingGrid.ToggleValueText(isOn);
    }
    public List<Vector3> FindPath(Vector3 startWorld, Vector3 endWorld)
    {
        PathFindingGrid.XY(startWorld, out int startX, out int startY);
        PathFindingGrid.XY(endWorld, out int endX, out int endY);

        List<PathCell> path = FindPath(startX, startY, endX, endY);
        if (path == null) return null;

        List<Vector3> vPath = new List<Vector3>();
        foreach (var cell in path)
        {
            vPath.Add(PathFindingGrid.WorldPosition(cell._x, cell._y) + new Vector3(_worldGridMapCellSize, _worldGridMapCellSize) * 0.5f);
        }

        return vPath;

    }
    public List<PathCell> FindPath(int startX, int startY, int endX, int endY)
    {
        PathCell startCell = PathFindingGrid.GetCellValue(startX, startY);

        _openCells = new List<PathCell> { startCell };
        _closedCells = new List<PathCell>();

        foreach (var cell in PathFindingGrid.GetCellsList())
        {
            cell.UserValue._gCost = int.MaxValue;
            cell.UserValue.CalculateFCost();
            cell.UserValue.LastCell = null;

        }


        startCell._gCost = 0;
        startCell._hCost = CalculateDistanceCost(startCell, PathFindingGrid.GetCellValue(endX, endY));
        startCell.CalculateFCost();

        while (_openCells.Count > 0)
        {
            PathCell currentCell = GetLowestFCostCell();
            if (currentCell == PathFindingGrid.GetCellValue(endX, endY)) // if reached the end
            {
                return CalculatePath(currentCell);
            }

            _openCells.Remove(currentCell);
            _closedCells.Add(currentCell);

            // Check all the neighbours of the current cell
            var neighbours = GetNeighbor(currentCell);
            foreach (var neighbour in neighbours)
            {
                // Check is the neighbour is already checked
                if (_closedCells.Contains(neighbour))
                {
                    continue;
                }
                if (!neighbour.Walkable)
                {
                    _closedCells.Add(neighbour);
                    continue;
                }

                // Get the distance between the current cell and the neighbor
                int tentativeGCost = currentCell._gCost + CalculateDistanceCost(currentCell, neighbour) * neighbour.Weight;

                // Check if the cost on neighbour is less than the current cost
                if (tentativeGCost < neighbour._gCost)
                {
                    // Set the last cell to the current cell
                    neighbour.LastCell = currentCell;
                    neighbour._gCost = tentativeGCost;
                    neighbour._hCost = CalculateDistanceCost(neighbour, PathFindingGrid.GetCellValue(endX, endY));
                    neighbour.CalculateFCost();

                    // Check if the neighbor
                    if (!_openCells.Contains(neighbour))
                    {
                        _openCells.Add(neighbour);
                    }
                }
            }

        }

        return null; // Return null no path is found
    }

    public List<Grid<PathCell>.Cell> GetCellList()
    {
        return PathFindingGrid.GetCellsList();
    }

    public PathCell GetCellValue(int x, int y)
    {
        return PathFindingGrid.GetCellValue(x, y);
    }

    public void XY(Vector3 worldPosition, out int x, out int y)
    {
        PathFindingGrid.XY(worldPosition, out x, out y);
    }

    public Vector3 WorldPosition(int x, int y)
    {
        return PathFindingGrid.WorldPosition(x, y);
    }

    public void DrawDebugLines()
    {
        PathFindingGrid.DrawDebugLines();
    }

    // Private
    ////////////////////////////////////////////////////

    private List<PathCell> GetNeighbor(PathCell currentCell)
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
                neighbours.Add(PathFindingGrid.GetCellValue(checkX, checkY));
            }
        }

        return neighbours; // Return the list of valid neighbors
    }

    private bool IsInsideGrid(int x, int y)
    {
        return x >= 0 && x < PathFindingGrid.GetWidth() && y >= 0 && y < PathFindingGrid.GetHeight();
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

    public Grid<PathCell> GetGrid()
    {
        return PathFindingGrid;
    }
}
