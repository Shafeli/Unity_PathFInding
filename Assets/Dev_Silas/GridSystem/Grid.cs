using System;
using UnityEngine;
using System.Collections.Generic;

public class Grid
{
    private int _width;
    private int _height;

    private float _cellSize;

    private int[,] _gridCells;

    public Grid(int width, int height, float cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;

        _gridCells = new int[width, height];

        for (int x = 0; x < _gridCells.GetLength(0); ++x)
        {
            for (int y = 0; y < _gridCells.GetLength(1); ++y)
            {
                //
                GeneralUtility.WorldTextGenerator.CreateWorldText(_gridCells[x, y].ToString(), null,
                    WorldPosition(x, y), 20, Color.white, TextAnchor.MiddleCenter);
            }
        }
    }

    private Vector3 WorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize;
    }
    
}
