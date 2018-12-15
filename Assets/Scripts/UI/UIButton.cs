using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    public GameObject GameItem;
    private bool IsActive = false;

    private void Awake()
    {
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetIsActive(bool isAvtive)
    {
        IsActive = isAvtive;
    }

    public bool GetIsActive()
    {
        return IsActive;
    }
}
