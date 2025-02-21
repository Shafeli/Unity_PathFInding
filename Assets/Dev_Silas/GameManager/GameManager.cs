using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int _worldGridMapWidth = 4;
    [SerializeField] private int _worldGridMapHeight = 4;
    [SerializeField] private float _worldGridMapCellSize = 10.0f;

    private Grid _grid;

    void Start()
    {
        _grid = new Grid(_worldGridMapWidth, _worldGridMapHeight, _worldGridMapCellSize, transform.position, gameObject);

    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            _grid.SetCellValue(GeneralUtility.MouseUtility.GetWorldPosition(), 500);
        }

        _grid.UpdateGrid();
        _grid.DrawDebugLines();
    }
}
