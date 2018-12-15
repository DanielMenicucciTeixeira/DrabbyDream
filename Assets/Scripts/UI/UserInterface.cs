using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour {

    private List<UIButton> ButtonList;
    private PlayerController playerController;

	// Use this for initialization
	void Start ()
    {
        playerController = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SelectItem(UIButton Button)
    {
      /*  foreach(UIButton button in ButtonList)//Sets all button to inactive
        {
            button.SetIsActive(false);
        }*/
        Button.SetIsActive(true);//Then sets the clicked button to active
        //playerController.SetSelectedItem(Button.GameItem);
    }

    public void FillButtonList()
    {
        ButtonList = null;

        foreach (UIButton button in FindObjectsOfType<UIButton>())
        {
            ButtonList.Add(button);
        }
    }
}
