using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Grid
{
    class Cell
    {
        public TextMesh TextMesh;
        public int CellIndex;
        public int X, Y;
        public int Value = 0;
    }

    private GameObject _pOwner;
    private List<Cell> _cells;
    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _centerPosition;
    private int[,] _gridCells;


    // Public
    ////////////////////////////////////////////////////
    
    public Grid(int width, int height, float cellSize, Vector3 centerPosition, GameObject pOwner)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _centerPosition = centerPosition; // Store the center position
        _cells = new List<Cell>(width * height);
        _gridCells = new int[width, height];

        int counter = 0;
        for (int x = 0; x < _gridCells.GetLength(0); ++x)
        {
            for (int y = 0; y < _gridCells.GetLength(1); ++y)
            {
                Cell tempCell = new Cell
                {
                    TextMesh = GeneralUtility.WorldTextGenerator.CreateWorldText(_gridCells[x, y].ToString(), pOwner.transform,
                        WorldPosition(x, y) + new Vector3(_cellSize, _cellSize) * 0.5f, 10, Color.white, TextAnchor.MiddleCenter),
                    CellIndex = counter,
                    X = x,
                    Y = y
                };
                _cells.Add(tempCell);
                ++counter;
            }
        }

        _pOwner = pOwner;

    }

    public void UpdateGrid()
    {
        foreach (var cell in _cells)
        {
            string textStr = /*"Index: " + cell.CellIndex + */"\nValue: " + cell.Value;

            if (cell.TextMesh != null)
                cell.TextMesh.text = textStr;
        }
    }

    public void XY(Vector3 worldPosition, out int x, out int y)
    {
        // Offset the world position by the center position to get local grid coordinates
        Vector3 localPosition = worldPosition + _centerPosition;

        // Calculate the grid coordinates based on the cell size
        x = Mathf.FloorToInt((localPosition.x + (_width * _cellSize) / 2f) / _cellSize);
        y = Mathf.FloorToInt((localPosition.y + (_height * _cellSize) / 2f) / _cellSize);
    }



    public void SetCellValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            _gridCells[x, y] = value;
            _cells[CellIndex(x, y)].Value = value;
        }
    }

    public void SetCellValue(Vector3 worldPosition, int value)
    {
        int x, y = 0;
        XY(worldPosition, out x, out y);
        SetCellValue(x, y, value);
    }

    public void DrawDebugLines()
    {
        // Draw vertical lines
        for (int x = 0; x <= _width; ++x)
        {
            Debug.DrawLine(WorldPosition(x, 0), WorldPosition(x, _height), Color.white, 100f);
        }

        // Draw horizontal lines
        for (int y = 0; y <= _height; ++y)
        {
            Debug.DrawLine(WorldPosition(0, y), WorldPosition(_width, y), Color.white, 100f);
        }

        // Top and Bottom
        Debug.DrawLine(WorldPosition(0, _height), WorldPosition(_width, _height), Color.white, 100f);
        Debug.DrawLine(WorldPosition(_width, 0), WorldPosition(_width, _height), Color.white, 100f);
    }

    // Private
    ////////////////////////////////////////////////////

    private Vector3 WorldPosition(int x, int y)
    {
        // Offset to center the grid
        float offsetX = (x - _width / 2f) * _cellSize; // Horizontal offset
        float offsetY = (y - _height / 2f) * _cellSize; // Vertical offset

        // Combine the offsets with the center
        Vector3 worldPosition = new Vector3(offsetX, offsetY, 0) + _centerPosition;

        return worldPosition;
    }

    private int CellIndex(int x, int y)
    {
        return y * _width + x;
    }
}
