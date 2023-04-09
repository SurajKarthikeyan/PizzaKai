using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    #region VARIABLES
    private Grid<PathNode> grid;
    public int x;
    public int y;

    public bool isWalkable;
    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode cameFromNode;

    #endregion
    public PathNode(Grid<PathNode> grid, int x, int y) 
    {
        this.y = y;
        this.x = x;
        this.grid = grid;
        isWalkable = true;
    }
    public void CalculateFCost() 
    {
        fCost = gCost + hCost;
    }
    public override string ToString()
    {
        return x + "," + y;
    }
}
