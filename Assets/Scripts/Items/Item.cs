using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : GameTarget
{
    //Flag to tell if the Item can be picked up, default is true
    protected bool IsCollectable = true;
    //How long the item lasts, set to 0 for infinite duration
    protected float BaseDuration = 0;
    //How much time before base duration is reached
    float CurrentDuration;
    //Amount of this Item available in inventory, not to be used in instances
    public int Stock = 0;

    void Start()
    {
        OnCreate();
    }

    void Update()
    {
        DoOnUpdate();
    }

    void OnEnterCollisioin(Collision collision)
    {
        DoOnCollision(collision);
    }

    void OnTriggerEnter(Collider other)
    {
        DoOnTrigger(other);
    }

    //This function exist to assure that every child of Item can do it's OnCollisonEnter.
    protected override void DoOnCollision(Collision collision)
    {
        if ((collision.gameObject.GetComponent<PlayerAI>() || collision.gameObject.GetComponent<EnemyMovement>()) && IsCollectable)
        {
            FindObjectOfType<PlayerAI>().RemoveFromTargetList(this);
            Destroy(gameObject);
        }

        base.DoOnCollision(collision);
    }

    //This function exist to assure that every child of Item can do it's Start.
    protected virtual void OnCreate()
    {
        CurrentDuration = BaseDuration;
    }

    //This function exist to assure that every child of Item can do it's Update.
    protected virtual void DoOnUpdate()
    {
        if(CurrentDuration > 0)
        {
            CurrentDuration -= Time.deltaTime;
            if(CurrentDuration <= 0)
            {
                OnDestroy();
            }
        }
    }

    //This function exist to assure that every child of Item can do it's OnTriggerEnter.
    protected virtual void DoOnTrigger(Collider other)
    {

    }

    //This function exist to assure that every child of Item can do it's Destroy.
    protected virtual void OnDestroy()
    {
        Destroy(gameObject);
    }
}
