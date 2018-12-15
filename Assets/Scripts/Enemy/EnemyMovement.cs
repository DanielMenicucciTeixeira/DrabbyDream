using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMovementType
{
    Forward,
    Line,
    Circle,
    Follow,
    Patrol
}

[RequireComponent (typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{
    public eMovementType MovementType;
    public float Speed;

    //Used for LinearMovementDistance and Circular movement radius
    public float Radius;
    private Vector3 StartPosition;

    List<Node> Path;

	// Use this for initialization
	void Start ()
    {
        TimeCounter = 0;
        StartPosition = gameObject.transform.position;
        Direction = gameObject.transform.forward.normalized;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Move(MovementType);
	}

    void Move(eMovementType _MovementType)
    {
        switch(_MovementType)
        {
            case eMovementType.Forward: MoveForward(); break;
            case eMovementType.Line: MoveLine(); break;
            case eMovementType.Circle: MoveCircle(); break;
            case eMovementType.Follow: MoveFollow(); break;
            case eMovementType.Patrol: MovePatrol(); break;
            default: break;
        }
    }

    void MoveForward()
    {
        GetComponent<Rigidbody>().velocity = gameObject.transform.forward.normalized * Speed;
    }



    private bool GoingForward = true;//Flag Used to determin direction it is moving
    private Vector3 Direction;//Used to the the return angle in MoveLine Function
    void MoveLine()
    {
        if (GoingForward)
        {
            
            if (GoingForward && (StartPosition - gameObject.transform.position).magnitude <= Radius)
            {
               
                GetComponent<Rigidbody>().velocity = Direction * Speed;
            }
            else
            {
                GoingForward = false;
                GetComponent<Rigidbody>().velocity = -Direction * Speed;
            }
        }
        else
        {
           
            if (!GoingForward && (StartPosition - gameObject.transform.position).x >= 0)
            {
                GetComponent<Rigidbody>().velocity = -Direction * Speed;
            }
            else
            {
                GoingForward = true;
                GetComponent<Rigidbody>().velocity = Direction * Speed;
            }
        }
        
    }

    //Circular movement center
    public Transform CircleCenter;
    private float TimeCounter;
    void MoveCircle()
    {
        //Set the radius to negative value to use the distance between the object and the CircleCenter as radius
        if (CircleCenter)
        {
            if (Radius < 0) Radius = (CircleCenter.position - gameObject.transform.position).magnitude;
            gameObject.transform.position = CircleCenter.position + new Vector3(Mathf.Cos(TimeCounter * Speed / Radius) * Radius, 0, Mathf.Sin(TimeCounter * Speed / Radius) * Radius);
        }
        //If no CircleCenter is given, the objects initial position will be used as the CircleCenter.
        else
        {
            gameObject.transform.position = StartPosition + new Vector3(Mathf.Cos(TimeCounter * Speed / Radius) * Radius, 0, Mathf.Sin(TimeCounter * Speed / Radius) * Radius);
        }
        TimeCounter += Time.deltaTime;
    }

    //Target to be followed
    private GameObject Target;
    void MoveFollow()
    {

    }

    //List of points of patrol in order
    public List<Transform> PatrolPath;
    int PatrolPointIndex = 0;
    void MovePatrol()//Requires Pathfiding
    {
        if(!FindObjectOfType<MyGameManager>())
        {
            Debug.Log("Missing GameManger to run Patrol movement!");
            return;
        }
        else
        {
            if(PatrolPath.Count <= 0)
            {
                Debug.Log("There is no PatrolPath for patrol movement!");
                return;
            }
            else
            {
                
                Path = FindObjectOfType<MyGameManager>().GetComponent<Pathfinding>().FindPath(gameObject.transform.position, PatrolPath[PatrolPointIndex].position);

                if ((PatrolPath[PatrolPointIndex].transform.position - gameObject.transform.position).magnitude <= Radius)
                {
                    if (PatrolPointIndex < PatrolPath.Count - 1) PatrolPointIndex++;
                    else PatrolPointIndex = 0;

                    Debug.Log(PatrolPointIndex);
                }
                else if(GetComponent<Rigidbody>())
                {
                    GetComponent<Rigidbody>().velocity = (Path[0].Position - gameObject.transform.position).normalized * Speed;
                }
                else
                {
                    gameObject.transform.position += (Path[0].Position - gameObject.transform.position).normalized * Speed * Time.deltaTime;
                }
            }
        }
    }
}
