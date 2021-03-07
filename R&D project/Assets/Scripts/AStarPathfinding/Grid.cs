using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private Transform startPos;

    [SerializeField]
    private List<Transform> targets = new List<Transform>();

    [SerializeField]
    private LayerMask wallMask;

    [SerializeField]
    private GameObject ground;

    private Vector2 gridWorldSize;

    [SerializeField]
    private float nodeRadius;

    private float dist, nodeDiameter;

    private PathfindingNode[,] grid;

    private int gridSizeX, gridSizeY;

    private void Start()
    {
        gridWorldSize = new Vector2(ground.transform.localScale.x * 10, ground.transform.localScale.z * 10);
        transform.position = startPos.position;
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();

        for(int i = 0; i < targets.Count; i++)
        {
            if (targets[i].GetComponent<MoveTargetPos>() != null)
            {
                targets[i].GetComponent<MoveTargetPos>().GetListNumber(i);
            }
        }
    }

    private void CreateGrid()
    {
        grid = new PathfindingNode[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool wall = true;

                if (Physics.CheckSphere(worldPoint, nodeRadius, wallMask))
                {
                    wall = false;
                }

                grid[x, y] = new PathfindingNode(wall, worldPoint, x, y);
            }
        }
    }

    public PathfindingNode NodeFromWorldPos(Vector3 worldPos)
    {
        float xPoint = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float yPoint = (worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y;

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        return grid[x, y];
    }

    public List<PathfindingNode> GetNeighborNodes(PathfindingNode node)
    {
        List<PathfindingNode> neighboringNodes = new List<PathfindingNode>();
        int xCheck, yCheck;

        xCheck = node.GetGridX() + 1;
        yCheck = node.GetGridY();

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        xCheck = node.GetGridX() - 1;
        yCheck = node.GetGridY();

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        xCheck = node.GetGridX();
        yCheck = node.GetGridY() + 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        xCheck = node.GetGridX();
        yCheck = node.GetGridY() - 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        return neighboringNodes;
    }

    public List<Transform> GetTargetList()
    {
        return targets;
    }

    public Transform GetStartPos()
    {
        return startPos;
    }
}
