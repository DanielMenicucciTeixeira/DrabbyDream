using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTarget : MonoBehaviour
{
    private float Value;
    public float Attraction;
    public bool HasSmell;
    public float AttractionRange;
    
    private void Awake()
    {
    }

    public void SetValue(float NewValue)
    {
        Value = NewValue;
    }

    public float GetValue()
    {
        return Value;
    }

    public float GetAttraction()
    {
        return Attraction;
    }

    void OnCollisionEnter(Collision collision)
    {
        DoOnCollision(collision);
    }

    //This function exist to assure that every child of GameTarget can do it's OnCollisonEnter.
    protected virtual void DoOnCollision(Collision collision)
    {
    }

}
