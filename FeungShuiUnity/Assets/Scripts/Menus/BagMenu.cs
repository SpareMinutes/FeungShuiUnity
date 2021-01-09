using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagMenu : ItemMenu {
    //GameObject stuff
    private EventSystem ES;
    private GameObject Selected, LastCanvas, Player, canvas;
    
    //Inventory stuff
    private Inventory bagInventory;
    private List<BagTab> bagTabs;
    private List<Item> currentItems;

    private int currentTabIndex;
    private int currentItemIndex;

    public OptionBox Actions;
    private bool inBattle;
    
    #region General
    void Start() {
        ES = gameObject.GetComponent<EventSystem>();
        Selected = ES.firstSelectedGameObject;
        canvas = GameObject.Find("BagCanvas");

        Player = GameObject.Find("WalkableCharacter");

        //set up bag here:
        bagInventory = Player.GetComponent<Inventory>();

        //bag tab
        bagInventory.OrganiseBagTabs();
        bagTabs = new List<BagTab>(bagInventory.tabs.Keys);

        //item lineup
        currentTabIndex = 0;
        currentItemIndex = 0;
        UpdateItemList();
    }

    void Update() {
        //Ensures clicking doesn't deselect buttons
        if (ES.currentSelectedGameObject != Selected) {
            if (ES.currentSelectedGameObject == null)
                ES.SetSelectedGameObject(Selected);
            else
                Selected = ES.currentSelectedGameObject;
        }
        if (Input.GetButtonDown("Cancel")) {
            ReturnToLast();
        }

        if (Input.GetButtonDown("Horizontal")){ 
            if(Input.GetAxisRaw("Horizontal") > 0)//right
                currentTabIndex++;
            else//left
                currentTabIndex--;
            currentTabIndex += bagTabs.Count;
            currentTabIndex %= bagTabs.Count;
            currentItemIndex = 0;
            UpdateItemList();
        }

        if (Input.GetButtonDown("Vertical")) {
            if (Input.GetAxisRaw("Vertical") > 0)//up
                currentItemIndex--;
            else//down
                currentItemIndex++;
            currentItemIndex += currentItems.Count;
            currentItemIndex %= currentItems.Count;
            UpdateItemList();
        }
    }
    #endregion

    private void UpdateItemList() {
        currentItems = bagInventory.GetList(bagTabs[currentTabIndex % bagTabs.Count]);
        canvas.transform.GetChild(2).GetComponent<Text>().text = bagTabs[currentTabIndex % bagTabs.Count].ToString();
        UpdateItemList(currentItems, bagInventory, currentItemIndex);
        canvas.transform.GetChild(3).GetComponent<Image>().sprite = null;

        //reset incase the tab has no items in it
        canvas.transform.GetChild(4).GetComponent<Text>().text = "";
    }

    public void UseItem() {
        //function for the item button
        currentItems[currentItemIndex + 2].use();
        Debug.Log(currentItems[currentItemIndex + 2].name);
    }
}
