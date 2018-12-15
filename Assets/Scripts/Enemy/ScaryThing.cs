using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaryThing : MonoBehaviour
{
    public float FearRadius;
    public float FearLevel;
    public float ScareRadius;
    public float ScareLevel;
    private GameObject Player;


    // Use this for initialization
    void Start()
    {
        Player = FindObjectOfType<PlayerAI>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if ((gameObject.transform.position - Player.transform.position).magnitude < FearRadius)
        {
            Player.GetComponent<PlayerAI>().AddFear(FearLevel);
        }
        if ((gameObject.transform.position - Player.transform.position).magnitude < ScareRadius)
        {
            Player.GetComponent<PlayerAI>().Scare(gameObject, ScareLevel);
        }
    }

    public void SetPlayer(GameObject player)
    {
        Player = player;
    }
}
