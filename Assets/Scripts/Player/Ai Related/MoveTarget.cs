using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveTarget : MonoBehaviour
{
    public LayerMask hitLayers;
    public LayerMask UILayer;
    public GameObject GameManager;
    private int fingerID = -1;

    private void Awake()
    {
        #if !UNITY_EDITOR
            finger ID = 0;
        #endif
    }

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject(fingerID))
        {
            if (Input.GetMouseButtonDown(0))//If the player has left clicked
            {
                Vector3 mouse = Input.mousePosition;//Get the mouse position
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);//Cast a ray to get where the mouse is pointing at
                RaycastHit hit;//Will store the position where the ray hit
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers) && hit.transform.gameObject.layer != UILayer)//If the Raycast does not hit a wall nor the UI
                {
                    this.transform.position = hit.point;//Move the target to the mouse position
                    //GameManager.GetComponent<MyGameManager>().SetPlayerPath();
                }
            }
        }

       
    }
	
}
