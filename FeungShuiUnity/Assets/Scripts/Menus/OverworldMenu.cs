using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OverworldMenu : Menu {
    private EventSystem ES;

    private GameObject Selected;

    public GameObject menu;

    public void Start() {
        //disableInteract();
        //Menu.SetActive(true);
        //isMenuOpen = true;
        ES = gameObject.GetComponent<EventSystem>();
        //ES.SetSelectedGameObject(null);
        //ES.SetSelectedGameObject(SelectedMenu);
        //Player.GetComponent<Walk>().canWalk = false;
        //GameObject.Find("WalkableCharacter").transform.GetChild(0).gameObject.SetActive(false);
        //Time.timeScale = 0;
    }

    private void Update() {
        //No need to update a paused scene
        if (paused) return;

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
    }

    #region Menu interactions
    public override void Pause() {
        base.Pause();
        menu.SetActive(false);
    }

    public override void Resume() {
        base.Resume();
        menu.SetActive(true);
    }
    #endregion

    #region Menu
    public void OpenSummary() {
        //will be called when the summary button is pressed from the menu
        //ShowMessage("This will show the summary of the adventure so far.", true);
        Debug.Log("This will show the summary of the adventure so far.");
    }


    public void OpenParty() {
        OpenNewMenu("PartyScreen");
    //    isPartyOpen = true;
    //    ES.SetSelectedGameObject(null);
    //    //ES.SetSelectedGameObject(SelectedParty);
    //    OpenSubscene("PartyScreen");
    //    GameObject.Find("PartyEventSystem").GetComponent<PartyMenu>().SetLastMenu(null);
    }

    //public void CloseParty() {
    //    isPartyOpen = false;
    //    SceneManager.UnloadSceneAsync("PartyScreen");
    //}


    public void OpenBag() {
    //    //opens the players bag when selected form the menu
    //    //ShowMessage("This will show the player inventory.");
    //    isBagOpen = true;
    //    Bag.SetActive(true);
    //    ES.SetSelectedGameObject(null);
    //    ES.SetSelectedGameObject(SelectedBag);
    //    Player.GetComponent<Walk>().canWalk = false;

    //    //set up bag here:
    //    Inventory inventory = Player.GetComponent<Inventory>();

    //    //bag tab
    //    inventory.OrganiseBagTabs();
    //    bagTabs = new List<BagTab>(inventory.tabs.Keys);

    //    //item lineup
    //    UpdateItemList();

    }

    //private void UpdateItemList() {

    //    Inventory inventory = Player.GetComponent<Inventory>();
    //    currentItems = inventory.GetList(bagTabs[currentTabIndex % bagTabs.Count]);

    //    int tabTitleIndex = 2;
    //    Bag.transform.GetChild(tabTitleIndex).GetComponent<Text>().text = bagTabs[currentTabIndex % bagTabs.Count].ToString();

    //    for (int i = 0; i < 5; i++) {
    //        string itemName;
    //        string itemNum;
    //        if (i + offset < 0 || i + offset >= currentItems.Count) {
    //            //this is to account for the 2 above and below the actual item button
    //            itemName = "--";
    //            itemNum = "-";
    //        } else {
    //            itemName = currentItems[i + offset].name;
    //            itemNum = inventory.itemDict[currentItems[i + offset]].ToString();
    //        }
    //        //then set the item text to the item
    //        Bag.transform.GetChild(1).GetChild(i).GetComponentInChildren<Text>().text = itemName;
    //        Bag.transform.GetChild(5).GetChild(i).GetComponentInChildren<Text>().text = itemNum;
    //    }
    //    //reset incase the tab has no items in it
    //    Bag.transform.GetChild(6).GetComponent<Text>().text = "";
    //    Bag.transform.GetChild(4).GetComponent<Image>().sprite = null;

    //    //to stop throwing exceptions
    //    if (currentItems.Count > 0) {
    //        //item description in bag
    //        Bag.transform.GetChild(6).GetComponent<Text>().text = currentItems[offset + 2].description;
    //        //item image
    //        Bag.transform.GetChild(4).GetComponent<Image>().sprite = currentItems[offset + 2].itemImage;
    //    }


    //}

    //public void UseItem() {
    //    //function for the item button
    //    currentItems[offset + 2].use();
    //    Debug.Log(currentItems[offset + 2].name);
    //}

    //public void CloseBag() {
    //    Bag.SetActive(false);
    //    isBagOpen = false;
    //    Player.GetComponent<Walk>().canWalk = true;
    //}


    public void Save() {
        //will be called when the player selects save from the menu
        //for right now doesnt do anything
        Debug.Log("This will save the game.");
    }


    public void OpenOptions() {
        //opens the ingame options menu from the menu
        Debug.Log("This will show the options.");
    }
    #endregion

}
