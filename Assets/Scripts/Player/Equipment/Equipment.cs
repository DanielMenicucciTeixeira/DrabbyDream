using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eEquipmentSlot
{
    EQUIPMENT_HEAD,
    EQUIPMENT_TORSO,
    EQUIPMENT_HAND_ANY,
    EQUIPMENT_HAND_LEFT,
    EQUIPMENT_HAND_RIGHT,
    EQUIPMENT_HAND_BOTH,
    EQUIPMENT_FEET,
    EQUIPMENT_NO_SLOT,
    EQUIPMENT_TOTAL_SLOTS
}

public class Equipment : MonoBehaviour
{
    protected eEquipmentSlot Slot = eEquipmentSlot.EQUIPMENT_NO_SLOT;
    protected float BaseDuration = 0.0f;
    private float CurrentDurantion;

    void Start()
    {
        OnCreate();
    }

    //This method will be called from the AI Update, it is used instead of the typical void Update() to make sure it is called in the corredt order, between other calls of the PlayerAI class.
    public virtual void EquipmentUpdate()
    {
        if (BaseDuration > 0)
        {
            CurrentDurantion -= Time.deltaTime;
            if (CurrentDurantion <= 0)
            {
                Destroy(this);
            }
        }
    }
    
    protected virtual void OnCreate()
    {
        CurrentDurantion = BaseDuration;
    }
}