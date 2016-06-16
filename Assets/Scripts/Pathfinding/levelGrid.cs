using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class levelGrid : MonoBehaviour {

    public float nodeRadius = 0.5f;
    private float nodeDiameter;
    int gridSizeX, gridSizeY;
    public int gridHeapSize
    {
        get { return gridSizeX * gridSizeY; }
    }
    public Vector2 size;
    public LayerMask unwalkableMask;
    public GameObject A;
    public GameObject B;
    Pathfinding pathfinding;
    Node[,] grid;

    void Awake() {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = (int)(size.x / nodeDiameter);
        gridSizeY = (int)(size.y / nodeDiameter);
        pathfinding = GetComponent<Pathfinding>();
        onCreateGrid();
    }

    void onCreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 gridBottomLeft = transform.position - (Vector3.right * gridSizeX / 2) - (Vector3.up * gridSizeY / 2);
        for (int x = 0; x < gridSizeX; x++)
            for (int y = 0; y < gridSizeY; y++)
            {
    
                Vector3 nodePosition = gridBottomLeft + (Vector3.right * (x * nodeDiameter + nodeRadius)) + Vector3.up * (y * nodeDiameter + nodeRadius);
                nodePosition = new Vector3(nodePosition.x, nodePosition.y, 0);
                bool walkable = ! (Physics2D.OverlapCircleAll(nodePosition, nodeDiameter).Length > 0);
                grid[x, y] = new Node(walkable, nodePosition, x, y);
            }

    }

    void Update() {

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 0));

        if (grid != null)
        {
            Node playerNode = getNodeFromWorldPoint(A.transform.localPosition);
            Node EnemyNode = getNodeFromWorldPoint(B.transform.localPosition);
            foreach (Node n in grid)
            {
                Gizmos.color = (playerNode == n || EnemyNode == n) ? Color.cyan : (n.isWalkable()) ? Color.white : Color.red;
                if (pathfinding.returnPath() != null)
                {
                    if (pathfinding.returnPath().Contains(n))
                        Gizmos.color = Color.black;
                }
                   
           
                   
                
              
                Gizmos.DrawCube(n.getPosition(), new Vector3(1, 1, 0) * (nodeDiameter - .1f));
            }
        }
    }



    public bool isCharacterInGrid(GameObject character)
    {
        if(grid != null)
        {
           
            float x = character.transform.position.x;
            float y = character.transform.position.y;
            if (x < Mathf.Ceil(grid[0, 0].getPosition().x) || x > Mathf.Ceil(grid[gridSizeX-1, 0].getPosition().x) )
                return false;
            else if ((y < grid[0, 0].getPosition().y || y > grid[0, gridSizeY - 1].getPosition().y))
                return false;
            else
                return true;
        }
        else
            return false;
       
            
    }



    public Node getNodeFromWorldPoint(Vector3 worldPoint)
    {
        float percentOnX = Mathf.Clamp01((worldPoint.x + size.x / 2) / size.x);
        float percentOnY = Mathf.Clamp01((worldPoint.y + size.y / 2) / size.y);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentOnX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentOnY);
        return grid[x, y];
    }

    public Node[,] getGrid() { return grid; }

    public List<Node> getNodeNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                //Check if it's itself
                if (x == 0 && y == 0)
                    continue;
                else
                {
                    int lookAtX = x + node.getX();
                    int lookAtY = y + node.getY();
                    if (isNodeInGrid(lookAtX, lookAtY))
                        neighbours.Add(grid[lookAtX, lookAtY]);
                }
            }
        }
        return neighbours;
    }

    bool isNodeInGrid(int x, int y)
    {
        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
        {
            return true;
        }
        else
            return false;
    }


}
