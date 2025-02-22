using UnityEngine;

public class PathCell
{
    public int _gCost;
    public int _hCost;
    public int _fCost;

    public PathCell LastCell;

    private Grid<PathCell> _grid;
    private int _x;
    private int _y;

    public PathCell(Grid<PathCell> grid, int x, int y)
    {
        _grid = grid;
        _x = x;
        _y = y;
    }

    public override string ToString()
    {
        return _x + ", " + _y;
    }
}
