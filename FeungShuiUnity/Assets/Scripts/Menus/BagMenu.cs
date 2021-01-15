using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagMenu : ItemMenu {
    //GameObject stuff
    private EventSystem ES;
    private GameObject Selected, LastCanvas, Player, canvas;
    public OptionBox Actions;

    //Inventory stuff
    private Inventory bagInventory;
    private List<BagTab> bagTabs;
    private List<Item> currentItems;

    private int currentTabIndex;
    private int currentItemIndex;

    private bool toUse;

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

    #region Selection stuff
    public void Select() {
        if(LastMenu is BattleMenu) {
            string[] labels = {"Use Item"};
            Action[] actionsIn = {() => UseOrSelectCreature()};
            Actions.Populate(labels, actionsIn);
        }else if(LastMenu is PartyMenu) {
            string[] labels = {"Confirm"};
            //TODO: Code for what param to enter here instead of null
            Action[] actionsIn = {() => NotifySelection(null)};
            Actions.Populate(labels, actionsIn);
        }else {
            string[] labels = {"Use Item", "Give Item"};
            Action[] actionsIn = {() => UseOrSelectCreature(), () => SelectCreature(false)};
            Actions.Populate(labels, actionsIn);
        }
    }

    //Bool: are you going to use the item? If not, you're going to give it.
    public void UseOrSelectCreature() {
        //TODO: Implement the methods needed for this if/else
        //if(used on a specific creature) //eg potion
        SelectCreature(true);
        //else (eg repel)
        //UseItem(item);
    }

    public void SelectCreature(bool toUseIn) {
        toUse = toUseIn;
        OpenNewMenu("PartyScreen");
    }
    #endregion

    #region Using/giving items
    public void UseItem() {
        //function for the item button
        currentItems[currentItemIndex + 2].use();
        Debug.Log(currentItems[currentItemIndex + 2].name);
    }

    public void UseItem(Item item) {
        //TODO: Implement this
    }

    public void UseItem(Item item, Creature creature) {
        //TODO: Implement this
    }

    public void GiveItem(Item item, Creature creature) {
        //TODO: Implement this
    }
    #endregion

    #region Menu interactions
    //Used to signal to the PartyMenu that opened this menu which item was selected
    public void NotifySelection(Item result) {
        ((PartyMenu)LastMenu).ReceiveNotify(result);
    }

    //Used to receive signal from the PartyMenu which creature was selected
    public void ReceiveNotify(Creature result) {
        switch (toUse) {
            case true:
                //TODO: Code for what param to enter here instead of null
                UseItem(null, result);      break;
            case false:
                //TODO: Code for what param to enter here instead of null
                GiveItem(null, result);     break;
        }
    }
    #endregion
}
