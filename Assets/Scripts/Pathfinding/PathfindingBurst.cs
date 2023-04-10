using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class PathFindingBurst : MonoBehaviour
{
    #region Variables
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    #endregion

    #region MonoBehavior

    private void Start()
    {
        FunctionPeriodic.Create(() =>
        {
            int2 start = new int2(0, 0);
            int2 end = new int2(19, 19);
            float startTime = Time.realtimeSinceStartup;
            for (int i = 0; i < 5; i++)
            {
                FindPath(start, end);
            }
            Debug.Log("Time: " + ((Time.realtimeSinceStartup-startTime) * 1000f));
        }, 1f);


    }

    #endregion




    #region NonMonoBehavior
    private void FindPath(int2 startingPosition, int2 endPosition) 
    {
        int2 gridSize = new int2(20, 20);

        NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x* gridSize.y, Allocator.Temp);

        for(int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++) 
            {
                PathNode pathNode = new PathNode(); 
                pathNode.x = x;
                pathNode.y = y;
                pathNode.index = CalculateIndex(x, y, gridSize.x);

                pathNode.gCost = int.MaxValue;
                pathNode.hCost = CalculateDistanceCost(new int2(x, y), endPosition);
                pathNode.CalculateFCost();

                pathNode.isWalkable = true;
                pathNode.cameFromNodeIndex = -1;

                pathNodeArray[pathNode.index] = pathNode;
            }
        }

        /*
         * Place walls
         
            PathNode walkableNode = pathNodeArray[CalculateIndex(1, 0, gridSize.x)];
            walkableNode.SetIsWalkable(false);
            pathNodeArray[CalculateIndex(1, 0, gridSize.x)] = walkableNode;
            
            walkableNode = pathNodeArray[CalculateIndex(1, 1, gridSize.x)];
            walkableNode.SetIsWalkable(false);
            pathNodeArray[CalculateIndex(1, 1, gridSize.x)] = walkableNode;
            
            walkableNode = pathNodeArray[CalculateIndex(1, 2, gridSize.x)];
            walkableNode.SetIsWalkable(false);
            pathNodeArray[CalculateIndex(1, 2, gridSize.x)] = walkableNode;
            
         */


        NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(new int2[]
        {
            // Left, left down, left up
            new int2(-1,0), 
            new int2(-1,1), 
            new int2(-1,-1),

            // center, center, center up
            new int2(0,-1),
            new int2(0,0),
            new int2(0,1),

            // right, right down, right up
            new int2(1,-1),
            new int2(1,0),
            new int2(1,1),
        }, Allocator.Temp);
        int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, gridSize.x);

        PathNode startNode = pathNodeArray[CalculateIndex(startingPosition.x, startingPosition.y, gridSize.x)];
        startNode.gCost = 0;
        startNode.CalculateFCost();
        pathNodeArray[startNode.index] = startNode;

        NativeList<int> openList = new NativeList<int>(Allocator.Temp);
        NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

        openList.Add(startNode.index);

        while(openList.Length > 0) 
        {
            int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);
            PathNode currentNode = pathNodeArray[currentNodeIndex];

            if(currentNodeIndex == endNodeIndex)
            {
                break; // reached destination
            }


            // Remove current node from open list
            for(int i = 0; i < openList.Length; i++) 
            {
                if (openList[i] == currentNodeIndex)
                {
                    openList.RemoveAtSwapBack(i);
                    break;
                }
            }

            closedList.Add(currentNodeIndex);

            for(int i = 0; i < neighbourOffsetArray.Length; i++)
            {
                int2 neighbourOffset = neighbourOffsetArray[i];
                int2 neightbourPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);
                if (!IsPositionInsideGrid(neightbourPosition, gridSize))
                {
                    continue; // invalid position
                }

                int neighbourNodeIndex = CalculateIndex(neightbourPosition.x, neightbourPosition.y, gridSize.x);

                if(closedList.Contains(neighbourNodeIndex)) 
                {
                    continue; // we've already touched this node
                }

                PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];
                if(!neighbourNode.isWalkable)
                {
                    continue; // unwalkable node
                }

                int2 currentNodePosition = new int2(currentNode.x, currentNode.y);

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neightbourPosition);
                 
                if(tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNodeIndex = currentNodeIndex;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.CalculateFCost();
                    pathNodeArray[neighbourNodeIndex] = neighbourNode;

                    if(!openList.Contains(neighbourNode.index)) 
                    {
                        openList.Add(neighbourNode.index);
                    }
                }
            }

        }

        PathNode endNode = pathNodeArray[endNodeIndex];
        if(endNode.cameFromNodeIndex == -1) 
        {
            // no path found
            Debug.Log("No Path");

        }
        else
        {
            NativeList<int2> path = CalculatePath(pathNodeArray, endNode);
            foreach(int2 pathPos in path)
            {
                Debug.Log(pathPos);
            }
            path.Dispose();
        }

        //Disposing of native arrays
        openList.Dispose();
        closedList.Dispose();
        pathNodeArray.Dispose();
        neighbourOffsetArray.Dispose();

    }

    private NativeList<int2> CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode)
    {
        if(endNode.cameFromNodeIndex == -1)
        {
            return new NativeList<int2>(Allocator.Temp);
        }
        else 
        {
            NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
            path.Add(new int2(endNode.x, endNode.y));

            PathNode currentNode = endNode;

            while(currentNode.cameFromNodeIndex != -1) 
            {
                PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
                path.Add(new int2(cameFromNode.x, cameFromNode.y));
                currentNode = cameFromNode;
            }
            return path;
        }
    }

    private bool IsPositionInsideGrid(int2 gridPos, int2 gridSize)
    {
        return 
            gridPos.x >= 0 && 
            gridPos.y >= 0 && 
            gridPos.x < gridSize.x &&
            gridPos.y < gridSize.y;
    }

    private int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray) 
    {
        PathNode lowestCostPathNode = pathNodeArray[openList[0]];
        for(int i = 1; i < openList.Length; i++)
        {
            PathNode testNode = pathNodeArray[openList[i]];
            if(testNode.fCost < lowestCostPathNode.fCost)
            {
                lowestCostPathNode = testNode;
            }
        }
        return lowestCostPathNode.index;
    }

    private int CalculateIndex(int x, int y, int gridWidth)
    {
        return x + y * gridWidth;
    }

    private int CalculateDistanceCost(int2 aPosition, int2 bPosition) 
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

        public void SetIsWalkable(bool newBool)
        {
            isWalkable = newBool;
        }
    }

    #endregion

}
