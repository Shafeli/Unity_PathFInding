using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PathWeightedVolume : MonoBehaviour
{

    [SerializeField] private SceneManager SceneManager;

    // the distance between the current cell and the neighbour 
    // tentativeGCost = currentCell _gCost + CalculateDistanceCost(currentCell, neighbour) * VolumeWeight; base weight is 1
    [SerializeField] private int VolumeWeight = 2;

    private PathFinding _pathFindingGrid;
    private Collider2D _colliderVolume;
    private bool _volumeApplied = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SceneManager != null)
            _pathFindingGrid = SceneManager.GetPathFindingGrid();

        _colliderVolume = GetComponent<Collider2D>();
        if (_colliderVolume == null)
        {
            _colliderVolume = gameObject.AddComponent<Collider2D>();
        }

        if (_pathFindingGrid != null)
        {
            ApplyToGrid();
            _volumeApplied = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_volumeApplied) return;

        if (SceneManager != null)
        {
            if (_pathFindingGrid == null)
            {
                _pathFindingGrid = SceneManager.GetPathFindingGrid();
                ApplyToGrid();
                _volumeApplied = true;
            }

            if (_pathFindingGrid != null && _volumeApplied == false)
            {
                ApplyToGrid();
                _volumeApplied = true;
            }
        }
    }

    private void ApplyToGrid()
    {
        // Calculate affected cells
        Bounds bounds = _colliderVolume.bounds;
        _pathFindingGrid.XY(bounds.min, out int minX, out int minY);
        _pathFindingGrid.XY(bounds.max, out int maxX, out int maxY);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (x < 0 || y < 0 || x >= _pathFindingGrid.GetGrid().GetWidth() || y >= _pathFindingGrid.GetGrid().GetHeight())
                {
                    continue; // Skip cell if out of range
                }

                Grid<PathCell>.Cell cell = _pathFindingGrid.GetCell(x, y);
                if (cell != null)
                {
                    cell.UserValue.Weight = VolumeWeight;
                }
            }
        }
    }
}
