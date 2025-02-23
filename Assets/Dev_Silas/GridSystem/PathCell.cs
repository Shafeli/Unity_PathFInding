using UnityEngine;

public class PathCell
{
    public int _gCost;
    public int _hCost;
    public int _fCost;
    public int _x { get; private set; }
    public int _y { get; private set; }

    public PathCell LastCell;
    private Grid<PathCell> _grid;

    public PathCell(Grid<PathCell> grid, int x, int y)
    {
        _grid = grid;
        _x = x;
        _y = y;
    }

    public void CalculateFCost()
    {
        _fCost = _gCost + _hCost;
    }

    public override string ToString()
    {
        return _x + ", " + _y;
    }
}
