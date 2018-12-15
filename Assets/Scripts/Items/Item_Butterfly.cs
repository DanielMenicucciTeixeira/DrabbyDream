using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Item_Butterfly : Item
{
    //Butterfly status to be Equiped to Drabby
    Equipment_Butterfly ButterflyEquip;

    //How far can Drabby notice the butterfly
    public float DistrationRadius = 10;

    //How fast the butterfly flies
    public float MoveSpeed = 3;

	// Use this for initialization
	void Start ()
    {
        OnCreate();
	}
	
	// Update is called once per frame
	void Update ()
    {
        DoOnUpdate();
	}

    void OnTriggerEnter(Collider other)
    {
        DoOnTrigger(other);
    }

    //This function exist to assure that every child of Item_Butterfly can do its Start()
    protected override void OnCreate()
    {
        IsCollectable = false;
        GetComponent<SphereCollider>().radius = DistrationRadius;
        GetComponent<SphereCollider>().isTrigger = true;
        BaseDuration = 3;
        base.OnCreate();
    }

    //This function exist to assure that every child of Item_Butterfly can do its OnTriggerEnter
    protected override void DoOnTrigger(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerAI>())
        {
            //Check if there is any Butterfly Equipment already on Drabby, there should be no more then one at a time
            if(other.GetComponent<Equipment_Butterfly>())
            {
                //Destroy any previous Butterfly equipment to replace with the new one
                foreach(Equipment_Butterfly butterfly in GetComponents<Equipment_Butterfly>())
                {
                    Destroy(butterfly);
                }
            }
             //Add the new Butterfly Equipment
            ButterflyEquip = other.gameObject.AddComponent<Equipment_Butterfly>();
            //Set the Equipments ButterflyItem reference to this one
            ButterflyEquip.ButterflyItem = this;
        }

        //Run the base class DoOnTrigger()
        base.DoOnTrigger(other);
    }

    //This function exist to assure that every child of Item_Butterfly can do its Update()
    protected override void DoOnUpdate()
    {
        //Run the base clas DoOnUpdate()
        base.DoOnUpdate();
    }

    //This function exist to assure that every child of Item_Butterfly can do its Destroy
    protected override void OnDestroy()
    {
        //If the ButterflyEquip was created and still exists, destroy it before being destroyed
        if (ButterflyEquip)
        {
            Destroy(ButterflyEquip);
        }

        //Run the base class OnDestroy()
        base.OnDestroy();
    }

    //Sets the direction in wich the butterfly will fly
    public void SetDirection(Vector3 target)
    {
        GetComponent<Rigidbody>().velocity = (target - gameObject.transform.position).normalized * MoveSpeed;
    }
}
