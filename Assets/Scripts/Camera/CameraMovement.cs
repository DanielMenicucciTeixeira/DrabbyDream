using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject CameraTarget;
    Vector3 LastFrameDistance;

	// Use this for initialization
	void Start ()
    {
       LastFrameDistance = CameraTarget.transform.position - gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.transform.position += (CameraTarget.transform.position - gameObject.transform.position) - LastFrameDistance;
        LastFrameDistance = CameraTarget.transform.position - gameObject.transform.position;
    }
}
