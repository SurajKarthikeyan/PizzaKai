using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private int width;
    private int height;
    private int[,] gridArray;
    private float cellSize;
    #region Constructor

    /// <summary>
    /// Constructor for the Grid Class. 
    /// It takes in two integers, 'width' and, 'height'.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public Grid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridArray = new int[width, height]; // gridArray declaration


        for(int x = 0; x < gridArray.GetLength(0); x++) 
        {
            for(int y = 0; y < gridArray.GetLength(1); y++) 
            {
                UtilsClass.CreateWorldText(gridArray[x,y].ToString(), null, GetWorldPosition(x,y) + new Vector3(cellSize, cellSize)*0.5f, 30, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);



    }
    #endregion

    #region NonMonoBehavior Methods
    private Vector3 GetWorldPosition(int x, int y) 
    {
        return new Vector3(x,y) * cellSize;
    }
    #endregion

}


