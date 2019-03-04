using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
	public bool displayGridGizmos;
	public LayerMask unwalkableMask;
	public float nodeRadius;
	public Transform player;
	Node[,] grid;
	public Vector3 gridWorldSize;
	int gridSizeX;
	int gridSizeZ;

	float nodeDiameter;

	public int MaxSize
	{
		get{ return gridSizeX*gridSizeZ; }
	}

	void Awake()
	{
		nodeDiameter = 2*nodeRadius;
	}

	public void AddNode(Node node)
	{
		grid[node.gridX, node.gridY] = node;
	}
	public void CreateGrid(int _gridSizeX, int _gridSizeZ)
	{
		gridSizeX = _gridSizeX;
		gridSizeZ = _gridSizeZ;
		gridWorldSize = new Vector3(gridSizeX, 1f, gridSizeZ);
		grid = new Node[gridSizeX, gridSizeZ];
	}

	public List<Node> GetNeighbors(Node node)
	{
		List<Node> neighbors = new List<Node>();
		for(int x = -1; x <= 1; ++x)
		{
			for(int y = -1; y <= 1; ++y)
			{
				if(x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkZ = node.gridY + y;

				if(checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ)
				{
					neighbors.Add(grid[checkX, checkZ]);
				}
			}
		}

		return neighbors;
	}

	public Node GetNodeFromWorldPoint(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentZ = (worldPosition.z + gridWorldSize.z/2) / gridWorldSize.z;
		percentX = Mathf.Clamp01(percentX);
		percentZ = Mathf.Clamp01(percentZ);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
		return grid[x, z];
	}


	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, gridWorldSize);

		if(grid != null && displayGridGizmos)
		{
			Node playerNode = GetNodeFromWorldPoint(player.position-transform.position);
			foreach(Node n in grid)
			{
				if(n != null)
				{
					Gizmos.color = (n.walkable)?Color.white : Color.red;
					if(playerNode == n)
						Gizmos.color = Color.cyan;
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-0.1f));
				}
			}
		}
	}
}
