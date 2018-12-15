using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject GameManager;
    private List<Node>  Path = new List<Node>();
    public float PlayerSpeed;
    int currentNode = 0;

	// Use this for initialization
	void Start ()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetNewPath(List<Node> NewPath)
    {
        Path = NewPath;
        currentNode = 0;
    }

    bool DidReachNode()
    {
        if ((gameObject.transform.position - Path[currentNode].Position).magnitude <= 0) return true;
        else return false;
    }

    public void MovePlayer()
    {
        if (Path != null)
        {
            if (currentNode < Path.Count)
            {
                if (DidReachNode())
                {
                    currentNode++;
                }
                if (currentNode != Path.Count)
                {
                    GetComponent<Rigidbody>().velocity = (Path[currentNode].Position - gameObject.transform.position).normalized * PlayerSpeed;
                }
                else GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            }
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }

    public void SetGameManager(GameObject _GameManager)
    {
        GameManager = _GameManager;
    }
}
