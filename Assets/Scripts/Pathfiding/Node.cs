using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int GridX;//The X position in  the Node array.
    public int GridY;//The Y position in  the Node array.

    public bool IsWall;//Tells if this Node is being obstructed.
    public Vector3 Position;//The world position of the Node.

    public Node Parent;//For the A* algorithm, will store the Node it came from so it can trace the shortest path.

    public int GCost;//The cost of moving to the next Node.
    public int HCost;//The distance between this Node and the goal.

    public int FCost { get { return GCost + HCost; } }//Quick function to add G and H costs, since it will never be edited, there is no need to set a function.

    //Constructor initializing all base values, there is no default constructor since a default Node makes no sense, every Node must have its unique and especific position in the array and in the game world.
    public Node(bool _IsWall, Vector3 _Position, int _GridX, int _GridY)
    {
        IsWall = _IsWall;
        Position = _Position;
        GridX = _GridX;
        GridY = _GridY;
    }

}
