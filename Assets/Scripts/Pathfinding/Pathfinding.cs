using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pathfinding
{

    #region VARIABLES
    [SerializeField] private const int MOVE_STRAIGHT_COST = 10;
    [SerializeField] private const int MOVE_DIAGONAL_COST = 14;
    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    #endregion
    /// <summary>
    /// Constructor for Pathfinding Class
    /// Creates a grid of PathNodes
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// 
    public Pathfinding(int width, int height)
    {
        grid = new Grid<PathNode>(width, height, 10f, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }



    #region NonMonobehavior

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY) 
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);
        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for(int x = 0; x < grid.GetWidth(); x++) 
        {
            for(int y = 0; y < grid.GetHeight(); y++) 
            {
                PathNode pathNode = grid.GetGridObject(x,y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }
        startNode.gCost = 0;
        startNode.hCost = CalculateHCost(startNode, endNode);
        startNode.fCost = startNode.gCost + startNode.hCost;

        while(openList.Count > 0) 
        {
            PathNode currentNode = GetLowestFCostNode(openList);

            if(currentNode == endNode) 
            {
                //reached final node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighbourNode in GetNeighbourList(currentNode)) 
            {
                if (closedList.Contains(neighbourNode)) continue;

                int tentativeGCost = currentNode.gCost + CalculateHCost(neighbourNode, neighbourNode);
                if(tentativeGCost < neighbourNode.gCost) 
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateHCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)) 
                    {
                        openList.Add(neighbourNode);
                    }                
                }
            }


        }

        // Out of nodes on the open list
        return null;
    }

    private int CalculateHCost(PathNode a, PathNode b) 
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs (a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList) 
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for(int i = 1; i < pathNodeList.Count; i++) 
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) 
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

    private List<PathNode> CalculatePath(PathNode endNode) 
    {
        List<PathNode> path = new List<PathNode> ();
        path.Add (endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null) 
        {
            path.Add (currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path; 
    }

    private List<PathNode> GetNeighbourList( PathNode currentNode) 
    {
        List<PathNode> neighbourList = new List<PathNode>();
        if(currentNode.x - 1 >= 0) 
        {
            //Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));

            //Left Down
            if(currentNode.y - 1 >= 0) { neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1)); }

            //Left Up
            if (currentNode.y + 1 < grid.GetHeight()) { neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1)); }
        }


        if (currentNode.x + 1 < grid.GetWidth()) 
        {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));

            //Right Down
            if (currentNode.y - 1 >= 0) { neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1)); }

            //Right Up
            if (currentNode.y + 1 < grid.GetHeight()) { neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1)); }

        }
        // Down
        if (currentNode.y - 1 >= 0) { neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1)); }
        // Up
        if (currentNode.y +1 < grid.GetHeight()) { neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1)); }

        return neighbourList; 
    }

    private PathNode GetNode(int x, int y) 
    {
        return grid.GetGridObject(x, y);
    }

    public Grid<PathNode> GetGrid() { return grid; }


    #endregion
}
