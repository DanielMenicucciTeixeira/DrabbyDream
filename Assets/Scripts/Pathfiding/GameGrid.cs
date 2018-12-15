using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    private bool Initilized = false;

    public LayerMask WallMask;//Used to Identify obstructed Nodes.
    public Vector2 GridWorldSize;//Stores width and hight of the grid in real world units.
    public float NodeRadius;//Controls the size of each node.
    public float NodeDistance;//Controls how far apart each node spawns.

    int GridSizeX, GridSizeY;

    Node[,] NodeGrid;//Two dimensional array to store position of the nodes.
    public List<Node> FinalPath;//Will store the compleate path of Nodes the A* algorithm finds.


    float NodeDiameter;

    

    private void Start()
    {
        NodeDiameter = 2 * NodeRadius;

        GridSizeX = Mathf.RoundToInt(GridWorldSize.x / NodeDiameter);//Gets the number of Nodes in the Grid's X coordinate by dividing the WorldSize.x lenght by the GridDiamenter
        GridSizeY = Mathf.RoundToInt(GridWorldSize.y / NodeDiameter);//Gets the number of Nodes in the Grid's Y coordinate by dividing the WorldSize.y lenght by the GridDiamenter

        CreateGrid();
        Initilized = true;
    }

    //Function that Creates the NodeGrid Matrix and it's Nodes.
    void CreateGrid()
    {
        NodeGrid = new Node[GridSizeX, GridSizeY];//Creates the NodeGrid to be filled, it is a 2DMatrix of GridSeizeX * GridSizeY

        Vector3 BottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y / 2;//Findes the bottem left of the map

        //For each x and y position of the NodeGrid Matix:
        for(int y = 0; y < GridSizeY; y++)
        {
            for(int x = 0; x < GridSizeX; x++)
            {
                Vector3 WorldPosition = BottomLeft + Vector3.right * (x * NodeDiameter + NodeRadius) + Vector3.forward * (y * NodeDiameter + NodeRadius);//Calculates the world position of that node by moving from the bottom left.

                bool IsWall = false;//Node are not Walls by defaut.
                if(Physics.CheckSphere(WorldPosition, NodeRadius, WallMask))//If there is a wall anywhere inside this node:
                {
                    IsWall = true;//Set this node as a Wall Node.
                }
                NodeGrid[x, y] = new Node(IsWall, WorldPosition, x, y);//Create the Node and add it to the NodeGrid Matrix.
            }
        }

    }
    //Function that Draws the Wireframe.
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));
        if(NodeGrid != null)//If the Grid is not empty:
        {
            foreach(Node node in NodeGrid)//Loop through every node in the Grid.
            {
                if (node.IsWall) Gizmos.color = Color.yellow;//If the current node is a Wall Node (obstructed Node), set it's color to yellow.
                else Gizmos.color = Color.white;//If the current node is NOT a Wall Node(unobstructed Node), set it's color to white.

                if(FinalPath != null)//If the FinalPath list is NOT empty (means the path is found):
                {
                    if(FinalPath.Contains(node))
                    {
                        Gizmos.color = Color.red;//Set the Color of that Node to red.
                                                 //Note that if the destination is a wall, it will be drawn red instead of yellow, but will still be unreacheble (as long as A* is concerned)
                                                 //Walls should never be set as destinations, if you wish to do so, you may need to tweek the algorithm to find the closest possible point in case a destination is not found.
                    }
                }

                Gizmos.DrawCube(node.Position, Vector3.one * (NodeDiameter - NodeDistance));//Draw the Node at it's position.
            }
        }
    }

    public Node NodeFromWorldPosition( Vector3 _WorldPosition)
    {
        if (NodeGrid == null) return null;

        float GridPointX = (_WorldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x;
        float GridPointY = (_WorldPosition.z + GridWorldSize.y / 2) / GridWorldSize.y;

        GridPointX = Mathf.Clamp01(GridPointX);
        GridPointY = Mathf.Clamp01(GridPointY);

        int x = Mathf.RoundToInt((GridSizeX - 1) * GridPointX);
        int y = Mathf.RoundToInt((GridSizeY - 1) * GridPointY);

        return NodeGrid[x, y];
    }

    public List<Node> GetNeighbourNodes(Node node)
    {
        List<Node> NeighbourNodes = new List<Node>();
        int xCheck, yCheck;

        //Right Side
        xCheck = node.GridX + 1;
        yCheck = node.GridY;
        if(xCheck >= 0 && xCheck < GridSizeX && yCheck >=0 && yCheck < GridSizeY)
        {
            NeighbourNodes.Add(NodeGrid[xCheck, yCheck]);
        }

        //Left Side
        xCheck = node.GridX - 1;
        yCheck = node.GridY;
        if (xCheck >= 0 && xCheck < GridSizeX && yCheck >= 0 && yCheck < GridSizeY)
        {
            NeighbourNodes.Add(NodeGrid[xCheck, yCheck]);
        }

        //Above
        xCheck = node.GridX;
        yCheck = node.GridY + 1;
        if (xCheck >= 0 && xCheck < GridSizeX && yCheck >= 0 && yCheck < GridSizeY)
        {
            NeighbourNodes.Add(NodeGrid[xCheck, yCheck]);
        }

        //Bellow
        xCheck = node.GridX;
        yCheck = node.GridY - 1;
        if (xCheck >= 0 && xCheck < GridSizeX && yCheck >= 0 && yCheck < GridSizeY)
        {
            NeighbourNodes.Add(NodeGrid[xCheck, yCheck]);
        }


        return NeighbourNodes;
    }

    public bool IsInitilized()
    {
        return Initilized;
    }
}
