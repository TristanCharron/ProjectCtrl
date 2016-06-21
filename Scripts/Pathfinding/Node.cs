using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node>
{
    bool walkable;
    int gCost, hCost, x, y;
    Node parent;
    Vector3 worldPosition;
    int heapIndex;
    public int HeapIndex
    {
        get {  return heapIndex; }
        set { heapIndex = value; }
    }

    public bool isWalkable() { return walkable; }
    public Vector3 getPosition() { return worldPosition; }

    public int FCost
    {
        get{ return gCost + hCost;}
    }

    public int getGCost() { return gCost; }
    public int getHCost() { return hCost; }
    public void setHCost(int cost) { hCost = cost; }
    public void setGCost(int cost) { gCost = cost; }
    public int getX() { return x; }
    public int getY() { return y; }
    public void setParent(Node p) { parent = p; }
    public Node GetParent() { return parent; }

    public Node(bool _walkable, Vector3 position,int _x, int _y) {
        worldPosition = position;
        walkable = _walkable;
        x = _x;
        y = _y;
        parent = null;
    }

    public int CompareTo(Node node)
    {
        int compare = FCost.CompareTo(node.FCost);
        if (compare != 0)
            return  -compare;
        else
            return (-1 * hCost.CompareTo(node.getHCost()));

            
    }
}
