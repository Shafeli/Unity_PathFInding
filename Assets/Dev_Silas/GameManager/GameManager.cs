using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int _worldGridMapWidth = 4;
    [SerializeField] private int _worldGridMapHeight = 4;
    [SerializeField] private float _worldGridMapCellSize = 10.0f;
    [SerializeField] private bool _debugWorldGridValue = true;

    // private Grid<int> _grid;
    private Grid<PathCell> _pathFindingGrid;

    void Start()
    {
        _pathFindingGrid = new Grid<PathCell>(_worldGridMapWidth, _worldGridMapHeight, _worldGridMapCellSize, transform.position, gameObject);

        _pathFindingGrid.ToggleValueText(_debugWorldGridValue);
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            //_grid.SetCellValue(GeneralUtility.MouseUtility.GetWorldPosition(), 500);

            int x, y;
            Vector3 mousePosition = GeneralUtility.MouseUtility.GetWorldPosition();
           // _grid.XY(mousePosition, out x, out y);

            //Debug.Log("Mouse Click Position was over index: " + _grid.CellIndex(x, y));
        }

        _pathFindingGrid.UpdateGrid();

    }

    void OnGUI()
    {
        _pathFindingGrid.DrawDebugLines();
    }
}
