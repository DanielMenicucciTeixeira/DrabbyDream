using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment_Sword : Equipment
{
    //How far an enemy can be to be struck by the sword
    float Range;
    //Area in witch enemys can be struck
    SphereCollider SwordHitArea;
    //Tells if the Sword is destructable or not
    bool Indestructable;
    //How many hits the sword can give before breaking, for indistructible, set to negative value.
    int Durability;

    void Start()
    {
        OnCreate();
    }

    protected override void OnCreate()
    {
        //Set the equiment type.
        Slot = eEquipmentSlot.EQUIPMENT_HAND_ANY;

        //Set this equipments duration.
        BaseDuration = 0.0f;
        //Set the range
        Range = 3.0f;
        //Set sword to be Destructable
        Indestructable = false;
        //Set how many hits the sword can slash
        Durability = 1;
        //Create a SphereCollider Component
        SwordHitArea = gameObject.AddComponent<SphereCollider>();
        //Set it's radius to the swords range
        SwordHitArea.radius = Range;
        //Set SwordHitArea as a trigger
        SwordHitArea.isTrigger = true;

        //Run base class OnCreate().
        base.OnCreate();
    }

    //This method will be called from the AI Update, it is used instead of the typical void Update() to make sure it is called in the corredt order, between other calls of the PlayerAI class.
    public override void EquipmentUpdate()
    {
        //Make sure Playher Ai is running.
        if(GetComponent<PlayerAI>())
        {
            //Then set IsBrave value to true.
            GetComponent<PlayerAI>().BeBrave(true);
        }

        //Run base class Update.
        base.EquipmentUpdate();
    }

    //If something enters the sword's range
    private void OnTriggerEnter(Collider other)
    {
        //If Drabby is not Distracted
        if(!GetComponent<PlayerAI>().GetIsDistracted())
        {
            //Check if it is a monster, if so
            if (other.gameObject.tag == "Monster")
            {
                //Destroy the monster
                Destroy(other.gameObject);

                //If this sword is not indestructable
                if (!Indestructable)
                {
                    //Then decrease durability by one
                    Durability--;
                    //If durability is 0 or less, destroy the sword
                    if (Durability <= 0) Destroy(this);
                }
            }
        }
    }
}
