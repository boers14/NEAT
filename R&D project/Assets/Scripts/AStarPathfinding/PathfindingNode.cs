using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode
{
    private int gridX, gridY;

    private bool isWall;
    private Vector3 pos;

    private PathfindingNode parent;

    private int gCost, hCost;

    private int fCost { get { return gCost + hCost; } }

    public PathfindingNode(bool isWall, Vector3 pos, int  gridX, int gridY)
    {
        this.isWall = isWall;
        this.pos = pos;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int GetGCost()
    {
        return gCost;
    }

    public int GetFCost()
    {
        return fCost;
    }

    public int GetHCost()
    {
        return hCost;
    }

    public int GetGridX()
    {
        return gridX;
    }

    public int GetGridY()
    {
        return gridY;
    }

    public PathfindingNode GetParent()
    {
        return parent;
    }

    public bool IsWall()
    {
        return isWall;
    }

    public void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }

    public void SetHcost(int hCost)
    {
        this.hCost = hCost;
    }

    public void SetParent(PathfindingNode parent)
    {
        this.parent = parent;
    }
}
