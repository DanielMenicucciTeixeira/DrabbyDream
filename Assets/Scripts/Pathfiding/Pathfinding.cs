using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    GameGrid PathGrid;//For Referencing GameGrid Class

    private void Awake()//When the program starts:
    {
        PathGrid = GetComponent<GameGrid>();//Get a reference to the GameGrid in GameManager.
    }
    private void Start()
    {

    }

    private void Update()//At every frame:
    {

    }

    public List<Node> FindPath(Vector3 _StartPositoin, Vector3 _TargetPosition)
    {
        Node StartNode = PathGrid.NodeFromWorldPosition(_StartPositoin);//Get the node closest to the starting position.
        Node TargetNode = PathGrid.NodeFromWorldPosition(_TargetPosition);//Get the node closest to the target position.

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedSet = new HashSet<Node>();

        OpenList.Add(StartNode);

        while(OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];

            for (int i = 1; i < OpenList.Count; i++)
            {

                if (OpenList[i].FCost <= CurrentNode.FCost && OpenList[i].HCost < CurrentNode.HCost)
                {
                    CurrentNode = OpenList[i];
                }
            }
            OpenList.Remove(CurrentNode);
            ClosedSet.Add(CurrentNode);

            if(CurrentNode == TargetNode)
            {
                PathGrid.FinalPath = GetFinalPath(StartNode, TargetNode);//Draws the path in the editor, does not affect game screen.
                return GetFinalPath(StartNode, TargetNode);
            }
               
            foreach (Node NeighbourNode in PathGrid.GetNeighbourNodes(CurrentNode))
            {
                if (NeighbourNode.IsWall || ClosedSet.Contains(NeighbourNode)) continue;
                int MoveCost = CurrentNode.GCost + GetManhattenDistance(CurrentNode, NeighbourNode);;

                if(MoveCost < NeighbourNode.GCost || !OpenList.Contains(NeighbourNode))
                {
                    NeighbourNode.GCost = MoveCost;
                    NeighbourNode.HCost = GetManhattenDistance(NeighbourNode, TargetNode);
                    NeighbourNode.Parent = CurrentNode;

                    if (!OpenList.Contains(NeighbourNode)) OpenList.Add(NeighbourNode);
                }
            }

        }


        return null;
    }

    List<Node> GetFinalPath(Node _StartNode, Node _TargetNode)
    {
        List<Node> FinalPath = new List<Node>();
        Node CurrentNode = _TargetNode;

        while( CurrentNode != _StartNode)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }

        FinalPath.Reverse();

        return FinalPath;
    }

    int GetManhattenDistance(Node _StartNode, Node _TargetNode)
    {
        int X = Mathf.Abs(_StartNode.GridX - _TargetNode.GridX);
        int Y = Mathf.Abs(_StartNode.GridY - _TargetNode.GridY);

        return X + Y;
    }

    /*private void SetPath (Vector3 _StartPositoin, Vector3 _TargetPosition)
    {
        PathGrid.FinalPath = FindPath(_StartPositoin, _TargetPosition);
    }*/

}
