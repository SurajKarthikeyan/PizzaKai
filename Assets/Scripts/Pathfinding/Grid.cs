using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Generic Grid Class to instantiate a grid object
/// Use whatever generic type to create it
/// ints, floats, bools whatever you want
/// </summary>
/// <typeparam name="TGridObject"></typeparam>
public class Grid<TGridObject>
{
    #region VARIABLES
    private int width;
    private int height;
    private TGridObject[,] gridArray;
    private TextMesh[,] debugTextArray;
    private Vector3 originPosition;
    private float cellSize;

    #endregion




    #region Constructor

    /// <summary>
    /// Constructor for the Grid Class. 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="cellSize"></param>
    /// <param name="originPosition"></param>
    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>,int,int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;


        // Initializies gridArray var
        gridArray = new TGridObject[width, height]; // gridArray declaration

        for(int x = 0; x <gridArray.GetLength(0); x++) 
        {
            for (int y = 0; y < gridArray.GetLength(1); y++) 
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        //Debug only
        bool showDebug = true;
        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width,height];
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(),
                        null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 30, Color.white, TextAnchor.MiddleCenter);
                    // The question mark two lines above prior to the "ToString" call is a null check. Cannot "ToString" a null value
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }




    }
    #endregion

    #region NonMonoBehavior Methods
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }


    public TGridObject GetGridObject(int x, int y) 
    {
        if (x >= 0 && y >= 0 && x < width && y < height) 
        {
            return gridArray[x, y];
        }
        else 
        {
            return default(TGridObject); 
        }

    }

    public TGridObject GetGridObject(Vector3 worldPosition) 
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public int GetWidth() { return width; }

    public int GetHeight() { return height; }

    public float GetCellSize() { return cellSize; }
    #endregion

}


