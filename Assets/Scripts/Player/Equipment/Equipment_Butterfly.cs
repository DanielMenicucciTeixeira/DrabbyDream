using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment_Butterfly : Equipment
{
    //The Item that spawned this component
    public Item_Butterfly ButterflyItem;

	// Use this for initialization
	void Start ()
    {
        OnCreate();
	}

    protected override void OnCreate()
    {
        Debug.Log("Hi There");

        //Set slot to none
        Slot = eEquipmentSlot.EQUIPMENT_NO_SLOT;

        //Set durantion
        BaseDuration = 0;

        //Run base class OnCreate()
        base.OnCreate();
    }

    public override void EquipmentUpdate()
    {
        //Make sure ButterflyItem was properly set and PlayerAI component is found
        if(ButterflyItem && GetComponent<PlayerAI>() && GetComponent<PlayerMovement>())
        {
            //Then make Drabby Distracted
            GetComponent<PlayerAI>().GetDistracted(true);
            //AndMake him chase the butterfly that spawned this equipment
            GetComponent<PlayerAI>().Chase(ButterflyItem.gameObject);
            GetComponent<PlayerMovement>().MovePlayer();
        }

        //Run base class EquipmentUpdate()
        base.EquipmentUpdate();
    }
}
