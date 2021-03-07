using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Grid grid;

    private Transform startPos, targetPos;

    private bool found = false;

    private float completeDist;
    private int targetIndex = 0;

    private Manager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<Manager>();
        grid = GetComponent<Grid>();
        startPos = grid.GetStartPos();
        SetTarget(targetIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (!found)
        {
            FindPath(startPos.position, targetPos.position);
        }
    }

    private void FindPath(Vector3 a_startPos, Vector3 a_targetPos)
    {
        PathfindingNode startNode = grid.NodeFromWorldPos(a_startPos);
        PathfindingNode targetNode = grid.NodeFromWorldPos(a_targetPos);

        List<PathfindingNode> openList = new List<PathfindingNode>();
        HashSet<PathfindingNode> closedList = new HashSet<PathfindingNode>();

        openList.Add(startNode);
        while (openList.Count > 0)
        {
            PathfindingNode currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].GetFCost() <= currentNode.GetFCost() && openList[i].GetHCost() < currentNode.GetHCost())
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                GetFinalPath(startNode, targetNode);
                break;
            }

            foreach (PathfindingNode node in grid.GetNeighborNodes(currentNode))
            {
                if (!node.IsWall() || closedList.Contains(node))
                {
                    continue;
                }

                int moveCost = currentNode.GetGCost() + GetManhattenDist(currentNode, node);

                if (moveCost < node.GetFCost() || !openList.Contains(node))
                {
                    node.SetGCost(moveCost);
                    node.SetHcost(GetManhattenDist(node, targetNode));
                    node.SetParent(currentNode);
                    openList.Add(node);
                }
            }
        }
    }

    private void GetFinalPath(PathfindingNode startNode, PathfindingNode endNode)
    {
        List<PathfindingNode> finalPath = new List<PathfindingNode>();
        PathfindingNode currentNode = endNode;

        while (currentNode != startNode)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.GetParent();
        }

        completeDist += finalPath.Count;

        if (targetIndex < grid.GetTargetList().Count - 1)
        {
            targetIndex += 1;
            startPos = targetPos;
            SetTarget(targetIndex);
        }
        else
        {
            found = true;
            manager.CalculateTime(completeDist);
            manager.EnablePathfinding(false);
        }
    }

    private int GetManhattenDist(PathfindingNode currentNode, PathfindingNode neighborNode)
    {
        int x = Mathf.Abs(currentNode.GetGridX() - neighborNode.GetGridX());
        int y = Mathf.Abs(currentNode.GetGridY() - neighborNode.GetGridY());

        return x + y;
    }

    public void SetFound(bool found)
    {
        this.found = found;
    }

    private void SetTarget(int index)
    {
        targetPos = grid.GetTargetList()[index];
    }

    public Grid GetGrid()
    {
        return grid;
    }

    public void ResetPathfinding()
    {
        found = false;
        completeDist = 0;
        targetIndex = 0;

        startPos = grid.GetStartPos();
        SetTarget(targetIndex);
    }
}
