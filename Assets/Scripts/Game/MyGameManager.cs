using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameManager : MonoBehaviour
{
    private GameObject Player;

	// Use this for initialization
	void Start ()
    {
        InitialSetUp();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!Player)
        {
            InitialSetUp();
        }
        if (Player.GetComponent<PlayerAI>().GetFearLevel() > Player.GetComponent<PlayerAI>().GetMaxFear())
        {
            //Debug.Log("Game Over");
            Application.Quit();
        }
	}

    /*public void SetPlayerPath()
    {
        GetComponent<Pathfinding>().SetPath(Player.transform.position, Player.GetComponent<PlayerAI>().GetTarget().transform.position);
        Player.GetComponent<PlayerMovement>().GetNewPath(GetComponent<Pathfinding>().GetPlayerPath());
    }*/

    public GameObject GetPlayer()
    {
        return Player;
    }

    private void InitialSetUp()
    {
        Player = FindObjectOfType<PlayerAI>().gameObject;


        if (Player)
        {
            foreach (ScaryThing scaryThing in FindObjectsOfType<ScaryThing>())
            {
                scaryThing.SetPlayer(Player);
            }
            Player.GetComponent<PlayerMovement>().SetGameManager(gameObject);
            Player.GetComponent<PlayerAI>().SetGameManager(gameObject);
            Player.GetComponent<PlayerAI>().FindNewTarget();
            // GetComponent<UserInterface>().FillButtonList();
        }

        gameObject.transform.position = new Vector3(0, 0, 0);
    }

    void OnGUI()
    {
        GUI.Label( new Rect(0, 0, 100, 100), Player.GetComponent<PlayerAI>().GetFearLevel().ToString());
    }

    public void LoadLevel(string SceneName)
    {
        SceneManager.LoadSceneAsync(SceneName);
    }
}
