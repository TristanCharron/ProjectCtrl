using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public GameObject go1, go2;
    public float frequency = 0.5f;
    levelGrid lvlGrid;
    Node[,] grid;
    Node startNode, targetNode;
    Heap<Node> openSet;
    HashSet<Node> closeSet;
    LinkedList<Node> finalPath;
    public LinkedList<Node> returnPath() { return finalPath; }

    // Use this for initialization
    void Start()
    {
        lvlGrid = GetComponent<levelGrid>();
        grid = lvlGrid.getGrid();
        onResetPath();
        InvokeRepeating("onFindPath", 0f, 0.1f);

    }

    // Update is called once per frame
    void Update()
    {
     

    }

    void onResetPath(Vector3 startPosition, Vector3 targetPosition)
    {
        startNode = lvlGrid.getNodeFromWorldPoint(startPosition);
        targetNode = lvlGrid.getNodeFromWorldPoint(targetPosition);
        openSet = new Heap<Node>(lvlGrid.gridHeapSize);
        closeSet = new HashSet<Node>();
        finalPath = new LinkedList<Node>();
        openSet.Add(startNode);


    }


    void onResetPath()
    {
        startNode = null;
        targetNode = null;
        openSet = new Heap<Node>(lvlGrid.gridHeapSize);
        closeSet = new HashSet<Node>();
        finalPath = new LinkedList<Node>();
    }

   public void onFindPath() { 
    
            if (lvlGrid.isCharacterInGrid(go1) && lvlGrid.isCharacterInGrid(go2))
                onFindPath(go1.transform.position, go2.transform.position);
            else
                onResetPath();
        
       

    }

   

    public void onFindPath(Vector3 startPosition, Vector3 targetPosition)
    {


        onResetPath(startPosition, targetPosition);


        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closeSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                setFinalPath(startNode, targetNode);
                break;
            }

            foreach (Node neighbour in lvlGrid.getNodeNeighbours(currentNode))
            {

                if (isNodeValidInPath(neighbour))
                {
                    int distanceToNeighbour = neighbour.getGCost() + getDistance(currentNode, neighbour);
                    if (distanceToNeighbour < neighbour.getGCost() || !openSet.Contains(neighbour))
                    {
                        neighbour.setGCost(distanceToNeighbour);
                        neighbour.setHCost(getDistance(neighbour, targetNode));
                        neighbour.setParent(currentNode);

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.updateItem(neighbour);

                    }
                }
            }


        }
    }

    void setFinalPath(Node startNode, Node endingNode)

    {
        Node currentNode = endingNode;
        finalPath = new LinkedList<Node>();
        while (currentNode != null)
        {
            finalPath.AddFirst(currentNode);
            currentNode = currentNode.GetParent();
        }


    }

    bool isNodeCostSmaller(Node n1, Node n2)
    {
        return (n1.FCost < n2.FCost || n1.FCost == n2.FCost && n1.getHCost() < n2.getHCost());
    }

    bool isNodeValidInPath(Node node)
    {
        if (!node.isWalkable() || closeSet.Contains(node))
            return false;
        else
            return true;
    }

    int getDistance(Node a, Node b)

    {
        int distX = Mathf.Abs(a.getX() - b.getX());
        int distY = Mathf.Abs(a.getY() - b.getY());
        if (distX > distY)
            return (14 * distY) + (10 * (distX - distY));
        else
            return (14 * distX) + (10 * (distY - distX));
    }

}
