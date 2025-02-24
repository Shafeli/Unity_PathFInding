using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private int _worldGridMapWidth = 4;
    [SerializeField] private int _worldGridMapHeight = 4;
    [SerializeField] private float _worldGridMapCellSize = 10.0f;

    [SerializeField] private bool _debugWorldGridValue = true;
    [SerializeField] private bool _debugWorldGridLines = true;
    [SerializeField] private bool _debugDrawPath = true;

    private PathFinding _pathFindingGrid;
    private List<Vector3> _bankedVecPath; // Debugging path 

    void Start()
    {
        _pathFindingGrid = new PathFinding(_worldGridMapWidth, _worldGridMapHeight, _worldGridMapCellSize,
            transform.position, gameObject, _debugWorldGridValue);

        _pathFindingGrid.ToggleValueText(_debugWorldGridValue);
        var cells = _pathFindingGrid.GetCellList();

        foreach (var cell in cells)
        {
            cell.UserValue = new PathCell(_pathFindingGrid.GetGrid(), cell.X, cell.Y);
        }
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            int x, y;
            Vector3 mousePosition = GeneralUtility.MouseUtility.GetWorldPosition();
            _pathFindingGrid.XY(mousePosition, out x, out y);
            _bankedVecPath = FindPath(_pathFindingGrid.WorldPosition(0,0), mousePosition);
        }

        if (Input.GetMouseButtonDown(1))
        {
            int x, y;
            Vector3 mousePosition = GeneralUtility.MouseUtility.GetWorldPosition();
            _pathFindingGrid.XY(mousePosition, out x, out y);

            var cellValue = _pathFindingGrid.GetCellValue(x, y);
            cellValue.Walkable = false;

            var pathCell = _pathFindingGrid.GetCell(x, y);
            if (pathCell.TextMesh != null)
                pathCell.TextMesh.gameObject.SetActive(false);

        }

        _pathFindingGrid.UpdateGrid();

    }

    void OnGUI()
    {
        if (_pathFindingGrid.GetCellList().First().TextMesh != null)
        {
            if (_debugWorldGridValue)
            {
                var gridCellObject = _pathFindingGrid.GetCellList().First().TextMesh.gameObject;
                if (!gridCellObject.activeSelf)
                {
                    _pathFindingGrid.ToggleValueText(true);
                }
            }
            else
            {
                var gridCellObject = _pathFindingGrid.GetCellList().First().TextMesh.gameObject;
                if (gridCellObject.activeSelf)
                {
                    _pathFindingGrid.ToggleValueText(false);
                }
            }
        }

        if (_debugWorldGridLines)
            _pathFindingGrid.DrawDebugLines();

        if (_debugDrawPath)
        {
            if (_bankedVecPath != null &&  _bankedVecPath.Count > 1) 
            {
                for (int i = 0; i < _bankedVecPath.Count - 1; i++) // Loop up to right before last cell
                {
                    Vector3 start = _bankedVecPath[i];
                    Vector3 end = _bankedVecPath[i + 1];
                    Debug.DrawLine(start, end, Color.red, 0.5f);
                }

                _bankedVecPath.Clear();
            }
        }
    }

    public List<Vector3> FindPath(Vector3 startWorld, Vector3 endWorld)
    {
        _pathFindingGrid.XY(startWorld, out int startX, out int startY);
        _pathFindingGrid.XY(endWorld, out int endX, out int endY);

        List<PathCell> path = _pathFindingGrid.FindPath(startX, startY, endX, endY);
        if (path == null) return null;

        List<Vector3> vPath = new List<Vector3>();
        foreach (var cell in path)
        {
            vPath.Add(_pathFindingGrid.WorldPosition(cell._x, cell._y) + new Vector3(_worldGridMapCellSize, _worldGridMapCellSize) * 0.5f);
        }

        return vPath;

    }

    public PathFinding GetPathFindingGrid()
    {
        return _pathFindingGrid;
    }
    // Private
    ////////////////////////////////////////////////////

}
