using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum eInventorySelection
{
    INVENTORY_NONE,
    INVENTORY_SWORD,
    INVENTORY_MEAT,
    INVENTORY_CHOCOLAT,
    INVENTORY_BUTTERFLY,
    INVENTORY_TOTAL
}

public class PlayerController : MonoBehaviour
{
    //Determines what Inventory item is selected
    private eInventorySelection SelectedItem = eInventorySelection.INVENTORY_NONE;

    //List of Items associated with Inventory Selections
    public ItemStruct ItemList;

    //LayerMask for the SpawnItem Raycasts
    public LayerMask hitLayers;

    //Each item's stock value text display
    public List<Text> ItemStocksDisplay;

    //Used to diferentiate between UI and game when clicking in the editor
    private int fingerID = -1;

    private void Awake()
    {
#if !UNITY_EDITOR
         finger ID = 0;
#endif
    }

    // Use this for initialization
    void Start()
    {
        ItemList.Sword.GetComponent<Item_Sword>().Stock = 3;
        ItemList.Meat.GetComponent<Item>().Stock = 3;
        ItemList.Chocolat.GetComponent<Item>().Stock = 3;
        ItemList.Butterfly.GetComponent<Item_Butterfly>().Stock = 3;
    }

    // Update is called once per frame
    void Update()
    {
        for(eInventorySelection item = eInventorySelection.INVENTORY_SWORD; item.GetHashCode() < ItemStocksDisplay.Count; item++)
        {
            ItemStocksDisplay[item.GetHashCode()].text = "x " + GetItemStock(item).ToString(); 
        }
        //If mouse right clicked
        if(Input.GetMouseButtonDown(1))
        {
            //Deselect any selected items
            SelectedItem = eInventorySelection.INVENTORY_NONE;
            ButterflyClickFlag = false;//Make sure this flag is set to false, in case Butterfly was deselected mid click
        }
        //if left clicked, use selecated item, if any
        else if(Input.GetMouseButtonDown(0) || (ButterflyClickFlag && SelectedItem == eInventorySelection.INVENTORY_BUTTERFLY))
        {
            switch (SelectedItem)
            {
                case eInventorySelection.INVENTORY_NONE:
                    break;
                case eInventorySelection.INVENTORY_SWORD:
                    SpawnSword();
                    break;
                case eInventorySelection.INVENTORY_MEAT:
                    SpawnMeat();
                    break;
                case eInventorySelection.INVENTORY_CHOCOLAT:
                    SpawnChocolat();
                    break;
                case eInventorySelection.INVENTORY_BUTTERFLY:
                    SpawnButterfly();
                    break;
                default:
                    break;
            }
        }
    }

    private bool SpawnItem(GameObject item)
    {

        if (!EventSystem.current.IsPointerOverGameObject(fingerID))
        {
            //Debug.Log(GetSelectedItem());
            if (item)
            {
                Vector3 mouse = Input.mousePosition;//Get the mouse position
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);//Cast a ray to get where the mouse is pointing at
                RaycastHit hit;//Will store the position where the ray hit
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers))//If the Raycast does not hit a wall nor the UI
                {
                    //Instantiate(SelectedItem, hit.point, Quaternion.identity);
                    Instantiate(item, new Vector3(hit.point.x, GetComponent<MyGameManager>().GetPlayer().transform.position.y, hit.point.z), Quaternion.identity);
                    return true;
                }
            }
        }

        return false;
    }

    //Function for when inventory Sword is selected
    protected void SpawnSword()
    {
        //If there swords in the inventory
        if(ItemList.Sword.GetComponent<Item_Sword>().Stock > 0)
        {
            //Spawn a sword at clicked location
            if (SpawnItem(ItemList.Sword))
            {
                //Remove a sword from inventory stock
                ItemList.Sword.GetComponent<Item_Sword>().Stock--;
            }  
        }
    }

    //Function for when inventory Meat is selected
    protected void SpawnMeat()
    {
        //If there is meat in the inventory
        if (ItemList.Meat.GetComponent<Item>().Stock > 0)
        {
            //Spawn meat at clicked location
           if( SpawnItem(ItemList.Meat))
            {
                //Remove one meat from inventory stock
                ItemList.Meat.GetComponent<Item>().Stock--;
            }
        }
    }

    //Function for when inventory Chocolat is selected
    protected void SpawnChocolat()
    {
        //If there is meat in the inventory
        if (ItemList.Chocolat.GetComponent<Item>().Stock > 0)
        {
            //Spawn meat at clicked location
            if(SpawnItem(ItemList.Chocolat))
            {
                //Remove one meat from inventory stock
                ItemList.Chocolat.GetComponent<Item>().Stock--;
            }
        }
    }

    //Flag for the Spawnposition of SpawnButterfly 
    bool ButterflyClickFlag = false;
    //Varible to store the first click hit.point
    Vector3 ButterflySpawnLocation;
    //Function for when inventory Butterfly is selected
    protected void SpawnButterfly()
    {
        //If clicking on UI, ignore this function
        if (EventSystem.current.IsPointerOverGameObject(fingerID)) return;

            //If there is any butterfly in the inventory
            if (ItemList.Butterfly.GetComponent<Item_Butterfly>().Stock > 0)
        {
            //If the spawn location is not set, set it to mouse click location
            if (!ButterflyClickFlag)
            {
                Vector3 mouse = Input.mousePosition;//Get the mouse position
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);//Cast a ray to get where the mouse is pointing at
                RaycastHit hit;//Will store the position where the ray hit
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers))//If the Raycast does not hit a wall nor the UI
                {
                    ButterflySpawnLocation = new Vector3(hit.point.x, 3, hit.point.z);//Set the butterfly spawn position
                    ButterflyClickFlag = true;//Set the first click flag to true;
                }
            }
            //If the spawn location is already set
            else if (Input.GetMouseButtonUp(0))
            {
                Vector3 mouse = Input.mousePosition;//Get the mouse position
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);//Cast a ray to get where the mouse is pointing at
                RaycastHit hit;//Will store the position where the ray hit
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers))//If the Raycast does not hit a wall nor the UI
                {
                    Vector3 ButterflyTargetDirection = new Vector3(hit.point.x, 1.5f, hit.point.z);//Set the butterfly target direction

                    //Spawn the butterfly
                    Item_Butterfly butterfly = Instantiate(ItemList.Butterfly, ButterflySpawnLocation, Quaternion.identity).GetComponent<Item_Butterfly>();
                    //Set the butterflys flight direction
                    if (butterfly) butterfly.SetDirection(ButterflyTargetDirection);

                    //Decrease the amount of butterflies in stock by one
                    ItemList.Butterfly.GetComponent<Item_Butterfly>().Stock--;
                    //Reset the first click flag
                    ButterflyClickFlag = false;
                }
            }
        }
    }

    public void SetSelectedItem (eInventorySelection item)
    {
       SelectedItem = item;
    }

    public int GetItemStock(eInventorySelection item)
    {
        switch(item)
        {
            case eInventorySelection.INVENTORY_NONE:
                return 0;
                break;
            case eInventorySelection.INVENTORY_SWORD:
                return ItemList.Sword.GetComponent<Item_Sword>().Stock;
            case eInventorySelection.INVENTORY_MEAT:
                return ItemList.Meat.GetComponent<Item>().Stock;
            case eInventorySelection.INVENTORY_CHOCOLAT:
                return ItemList.Chocolat.GetComponent<Item>().Stock;
            case eInventorySelection.INVENTORY_BUTTERFLY:
                return ItemList.Butterfly.GetComponent<Item_Butterfly>().Stock;
            default:
                return 0;
        }
    }

    //Workaround to unitys lack of enum buttons
    public void SetSelectedItem(string itemName)
    {
        if (itemName == "Sword") SetSelectedItem(eInventorySelection.INVENTORY_SWORD);
        else if (itemName == "Meat") SetSelectedItem(eInventorySelection.INVENTORY_MEAT);
        else if (itemName == "Butterfly") SetSelectedItem(eInventorySelection.INVENTORY_BUTTERFLY);
        else if (itemName == "Chocolat") SetSelectedItem(eInventorySelection.INVENTORY_CHOCOLAT);
        else SetSelectedItem(eInventorySelection.INVENTORY_NONE);
    }
}
