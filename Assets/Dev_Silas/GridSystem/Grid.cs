using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Grid<T>
{
    public class Cell
    {
        public TextMesh TextMesh;
        public int CellIndex;
        public int X, Y;
        public T UserValue;
    }

    private List<Cell> Cells { get; }
    private int Width { get; }
    private int Height { get; }

    private GameObject _pOwner;
    private float _cellSize;
    private Vector3 _centerPosition;

    // Public
    ////////////////////////////////////////////////////
    
    public Grid(int width, int height, float cellSize, Vector3 centerPosition, GameObject pOwner)
    {
        Width = width;
        Height = height;
        _cellSize = cellSize;
        _centerPosition = centerPosition; // Store the center position
        Cells = new List<Cell>(width * height);
        int[,] tempGrid = new int[width, height];

        int counter = 0;
        for (int y = 0; y < tempGrid.GetLength(1); ++y)
        {
            for (int x = 0; x < tempGrid.GetLength(0); ++x)
            {
                Cell tempCell = new Cell
                {
                    TextMesh = GeneralUtility.WorldTextGenerator.CreateWorldText(tempGrid[x, y].ToString(), pOwner.transform,
                        WorldPosition(x, y) + new Vector3(_cellSize, _cellSize) * 0.5f, 10, Color.white, TextAnchor.MiddleCenter),
                    CellIndex = counter,
                    X = x,
                    Y = y
                };
                Cells.Add(tempCell);
                ++counter;
            }
        }

        _pOwner = pOwner;

    }

    public void UpdateGrid()
    {   
        foreach (var cell in Cells)
        {

            if (cell.TextMesh != null)
            {
                // string textStr = "Index: " + cell.CellIndex + "\nValue: " + cell.Value;
                if (cell.UserValue != null)
                {
                    string textStr = cell.UserValue.ToString();
                    cell.TextMesh.text = textStr;
                }
            }
        }
    }

    public void XY(Vector3 worldPosition, out int x, out int y)
    {
        // Offset the world position by the center position to get local grid coordinates
        Vector3 localPosition = worldPosition - _centerPosition;

        // Grid coordinates based on the cell size
        x = Mathf.FloorToInt((localPosition.x + (Width * _cellSize) / 2f) / _cellSize);
        y = Mathf.FloorToInt((localPosition.y + (Height * _cellSize) / 2f) / _cellSize);
    }

    public Cell GetCell(int x, int y)
    {
        var cell = Cells[CellIndex(x, y)];
        return cell;
    }

    public Cell GetCell(Vector3 worldPosition)
    {
        int x, y = 0;
        XY(worldPosition, out x, out y);
        return GetCell(x, y);
    }

    public T GetCellValue(int x, int y)
    {
        var cell = Cells[CellIndex(x,y)];
        return cell.UserValue;
    }

    public T GetCellValue(Vector3 worldPosition)
    {
        int x, y = 0;
        XY(worldPosition, out x, out y);
        return GetCellValue(x, y);
    }

    public void SetCellValue(int x, int y, T value)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
        {
            Cells[CellIndex(x, y)].UserValue = value;
        }
    }

    public void SetCellValue(Vector3 worldPosition, T value)
    {
        int x, y = 0;
        XY(worldPosition, out x, out y);
        SetCellValue(x, y, value);
    }

    public void DrawDebugLines()
    {
        // Draw vertical lines
        for (int x = 0; x <= Width; ++x)
        {
            Debug.DrawLine(WorldPosition(x, 0), WorldPosition(x, Height), Color.white, 100f);
        }

        // Draw horizontal lines
        for (int y = 0; y <= Height; ++y)
        {
            Debug.DrawLine(WorldPosition(0, y), WorldPosition(Width, y), Color.white, 100f);
        }

        // Top and Bottom
        Debug.DrawLine(WorldPosition(0, Height), WorldPosition(Width, Height), Color.white, 100f);
        Debug.DrawLine(WorldPosition(Width, 0), WorldPosition(Width, Height), Color.white, 100f);
    }

    public void ToggleValueText(bool value)
    {

        foreach (var cell in Cells)
        {
            if (cell.TextMesh != null)
                cell.TextMesh.gameObject.SetActive(value);
        }
    }

    public List<Cell> GetCellsList()
    {
        return Cells;
    }

    public int CellIndex(int x, int y)
    {
        return y * Width + x;
    }

    public int GetWidth()
    {
        return Width;
    }

    public int GetHeight()
    {
        return Height;
    }

    // Private
    ////////////////////////////////////////////////////

    public Vector3 WorldPosition(int x, int y)
    {
        // Offset to center the grid; adjust Y to start from the bottom left
        float offsetX = (x - Width * 0.5f) * _cellSize;
        float offsetY = (y - Height * 0.5f) * _cellSize;

        return new Vector3(offsetX, offsetY, 0) + _centerPosition;
    }


}
