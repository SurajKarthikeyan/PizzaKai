using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class PathfindingBurst : MonoBehaviour
{
    #region Variables
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    #endregion


    #region NonMonoBehavior
    private void FindPath(int2 startingPosition, int2 endPosition) 
    {
        int2 gridSize = new int2(4, 4);

        NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>();

        for(int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++) 
            {
                PathNode node = new PathNode(); 
                node.x = x;
                node.y = y;
                node.index = CalculateIndex(x, y, gridSize.x);

                node.gCost = int.MaxValue;
                node.hCost = calculateDistanceCost(new int2(x, y), endPosition);
                node.CalculateFCost();

                node.isWalkable = true;
                node.cameFromNodeIndex = -1;

                pathNodeArray[node.index] = node;
            }
        }

        PathNode startNode = pathNodeArray[CalculateIndex(startingPosition.x, startingPosition.y, gridSize.x)];
        startNode.gCost = 0;
        startNode.CalculateFCost();
        pathNodeArray[startNode.index] = startNode;

        NativeList<int> openList = new NativeList<int>(Allocator.Temp);


        //Disposing of native arrays
        pathNodeArray.Dispose();

    }

    private int CalculateIndex(int x, int y, int gridWidth)
    {
        return x + y * gridWidth;
    }

    private int calculateDistanceCost(int2 aPosition, int2 bPosition) 
    {
        int xDistance = math.abs(aPosition.x - bPosition.x);
        int yDistance = math.abs(aPosition.y - bPosition.y);
        int remaining = math.abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    #endregion



    #region Structs
    private struct PathNode
    {
        public int x;
        public int y;

        public int index;

        public bool isWalkable;
        public int gCost;
        public int hCost;
        public int fCost;

        public int cameFromNodeIndex;


        public void CalculateFCost() 
        {
            fCost = gCost + hCost;
        }
    }

    #endregion

}
