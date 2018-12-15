using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Sword : Item
{
    void OnCollisionEnter(Collision collision)
    {
        DoOnCollision(collision);
    }

    //This function exist to assure that every child of Item_Sword can do it's OnCollisonEnter.
    protected override void DoOnCollision(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerAI>())
        {
            //Check if there is already any sword equiped
            if (collision.gameObject.GetComponent<Equipment_Sword>())
            {
                //If so, remove equiped swords, there should be no more then one sword equiped at a time
                foreach (Equipment_Sword sword in collision.gameObject.GetComponents<Equipment_Sword>())
                {
                    Destroy(sword);
                }
            }

            //Add new sword to Drabbys equipments
            collision.gameObject.AddComponent<Equipment_Sword>();

            //Run base class OnCollisionEnter
            base.DoOnCollision(collision);
        }
    }
}
