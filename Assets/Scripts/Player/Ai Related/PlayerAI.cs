using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    private GameTarget CurrentTarget;
    //private Vector3 CurrentTargetPosition;
    private List<GameTarget> TargetList = new List<GameTarget>();//List of all possible targets from witch to choose
    private GameObject GameManager;
    public LayerMask hitMask;
    public float SightDistance;

    private float FearLevel;//Current fear level, decreases over time to a certain amount.
    private float BaseFearLevel;//Amount of Fear that does not recover over time, increases to a smaller amount then fear.
    public float BaseFearMultiplier = 0.5f;//Base multiplier of the Base fear level, indicates how much of the added fear pecomes base fear, should always be btween 0 and 1;
    public float MaxFearLevel = 100.0f;//How much fear before game is over
    public float FearDecreaseRate = 1.0f;//Fear per second decrease when not scared, will stop when Fear = Base Fear;
    private bool IsScared;//Checks if Drabby is Scared or not;
    private bool IsBrave;//Checks if Drabby can get Scared or not;
    private bool IsDistracted;//Checks if Drabby is awere of his surroundings or not;
    private List<GameObject> ScaryList = new List<GameObject>();//Lisf of Things that Drabby is running from
    public float BaseFleeDistance;//How Far should Drabby try to run
    public float InScareFearMultiplier;//Mesures how much faster Drabby's fear level increases if he is currently scared.
    public float ScaredTimer;//How long before Drabby stops being scared
    private float CurrentScaredTimer;//Times for how long Drabby has been scared

    private void Awake()
    {
        Mathf.Clamp(BaseFearMultiplier, 0, 1);
        CurrentScaredTimer = ScaredTimer;
    }

	// Use this for initialization
	void Start ()
    {
        FearLevel = 0;
        BaseFearLevel = 0;
        IsScared = false;
        IsBrave = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Reset all conditions, except for IsScared,  to default values.
        ResetConditions();

        //Check for Equipments and rund their updates.
        if(GetComponent<Equipment>()) foreach(Equipment equipment in GetComponents<Equipment>())
        {
            equipment.EquipmentUpdate();
        }

        //If Distracted, do distraction update only (will be called by distraticon cause)
        //If notDistracted
        if(!IsDistracted)
        {
            //Check if IsScared
            if (!IsScared)//If not Scared
            {
                //Make sure the List of ScaryThings is empty
                ScaryList.Clear();
                //Reduce Fear until it reaches BaseFearLevel
                CalmDown();
                //Set a path for Drabby to follow
                SetPlayerPath();
            }
            else//If Scared
            {
                //Set a path for Drabby to run in
                Flee();
                //Check if enougth time has passed since scared, if so, stop fleeing.
                StopPanicking();
            }

            //Move according to path set.
            gameObject.GetComponent<PlayerMovement>().MovePlayer();
        }
    }

    //Basic Setup

    public void SetGameManager(GameObject _GameManager)
    {
        GameManager = _GameManager;
        SetPlayerPath();
    }

    // Wanted Target

    public void FindNewTarget()
    {
        if (TargetList.Count <= 0) return;

        CurrentTarget = TargetList[0];
        SetTargetValues();

        foreach(GameTarget target in TargetList)
        {
            if (CurrentTarget.GetValue() < target.GetValue()) CurrentTarget = target;
        }
    }

    private void FillTargetList()
    {
        if (TargetList != null) TargetList.Clear();

        if (FindObjectsOfType<GameTarget>().Length <= 0) return;

        RaycastHit hitResult;

        foreach (GameTarget target in FindObjectsOfType<GameTarget>())
        {
            if (target.HasSmell && target.AttractionRange >= (target.transform.position - gameObject.transform.position).magnitude)
            {
                TargetList.Add(target);
            }
            else
            {
                Debug.DrawRay(gameObject.transform.position,(target.transform.position - gameObject.transform.position), Color.red);
                if (Physics.Raycast(gameObject.transform.position, (target.transform.position - gameObject.transform.position), out hitResult, SightDistance, hitMask))
                {
                    if (hitResult.transform.gameObject == target.gameObject)
                    {
                        if (target.AttractionRange <= 0)
                        {
                            TargetList.Add(target);
                        }
                        else if (target.AttractionRange >= (target.transform.position - gameObject.transform.position).magnitude)
                        {
                            TargetList.Add(target);
                        }
                    }
                }
            }
            
        }
        if (TargetList.Count <= 0) return;

        //CurrentTarget = TargetList[0];
    }

    public void RemoveFromTargetList(GameTarget target)
    {
        if(TargetList.Contains(target))
        {
            TargetList.Remove(target);
        }
    }

    private void SetTargetValues()
    {
        int Distance;

        foreach(GameTarget target in TargetList)
        {
            Distance = GameManager.GetComponent<Pathfinding>().FindPath(gameObject.transform.position, target.transform.position).Count;
            target.SetValue(target.GetAttraction()/Distance);
        }
        
    }

    public GameTarget GetTarget()
    {
        return CurrentTarget;
    }

    // Fear
  
    public float GetFearLevel()
    {
        return FearLevel;
    }

    public float GetMaxFear()
    {
        return MaxFearLevel;
    }

    public void AddFear(float FearValue)
    {
        FearValue *= Time.deltaTime;

        if(IsScared)
        {
            FearLevel += FearValue * InScareFearMultiplier;
            BaseFearLevel += FearValue * BaseFearMultiplier * InScareFearMultiplier;
        }
        else
        {
            FearLevel += FearValue;
            BaseFearLevel += FearValue * BaseFearMultiplier;
        }
    }

    private void CalmDown()
    {
        if(FearLevel > BaseFearLevel)
        {
            FearLevel -= FearDecreaseRate * Time.deltaTime;
        }
    }

    private void StopPanicking()
    {
        if(CurrentScaredTimer > 0)
        {
            CurrentScaredTimer -= Time.deltaTime;
        }
        else
        {
            IsScared = false;
            ScaryList.Clear();
            CurrentScaredTimer = ScaredTimer;
        }
    }

    public void Scare(GameObject ScaryThing, float ScareLevel)
    {
        if(!IsDistracted)
        {
            if (!ScaryList.Contains(ScaryThing))
            {
                ScaryList.Add(ScaryThing);
                FearLevel += ScareLevel;
            }
            if(!IsBrave)
            {
                CurrentScaredTimer = ScaredTimer;
                IsScared = true;
            }
        }
    }

    public bool GetIsScared()
    {
        return IsScared;
    }

    // Movement

    private void SetPlayerPath()
    {
        if(!IsScared)
        {
            FillTargetList();
            FindNewTarget();
            if (!CurrentTarget) return;
            Chase(CurrentTarget.gameObject);
        }
        else
        {
            Flee();
        }
    }

    public void Chase(GameObject target)
    {
        GetComponent<PlayerMovement>().GetNewPath(GameManager.GetComponent<Pathfinding>().FindPath(gameObject.transform.position, target.transform.position));
    }

    private void Flee()
    {
        Vector3 RunDirection = new Vector3(0, 0, 0);

        foreach (GameObject ScaryThing in ScaryList)
        {
            RunDirection += (gameObject.transform.position - ScaryThing.transform.position).normalized;
        }

        RunDirection *= BaseFleeDistance;

        GetComponent<PlayerMovement>().GetNewPath(GameManager.GetComponent<Pathfinding>().FindPath(gameObject.transform.position, (gameObject.transform.position + RunDirection)));
    }

    public void BeBrave(bool bravery)
    {
        IsBrave = bravery;
    }

    public void GetDistracted(bool distracted)
    {
        IsDistracted = distracted;
    }

    private void ResetConditions()
    {
        IsBrave = false;
        IsDistracted = false;
    }

    public void RemoveFromScaryList(GameObject scaryThing)
    {
        if (ScaryList.Contains(scaryThing)) ScaryList.Remove(scaryThing);
    }

    public bool GetIsDistracted()
    {
        return IsDistracted;
    }
}
