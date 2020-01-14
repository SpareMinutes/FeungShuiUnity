using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuAndWorldUI : MonoBehaviour{
    public EventSystem ES;
    [SerializeField]
    private GameObject Player, Canvas, Message, Text, Menu, Bag;
    private bool isMenuOpen, isBagOpen = false;
    private GameObject SelectedMenu, SelectedMessage, SelectedBag;

    private List<string> bagTabs;
    private int currentTabIndex = 0;
    private List<Item> currentItems;
    private int offset = -2;

    public void Start(){
        SelectedMenu = Menu.transform.GetChild(0).gameObject;
        SelectedMessage = Message;
        SelectedBag = Bag.transform.GetChild(1).GetChild(2).gameObject; //should select the item button if hte layering is correct
    }

    public void Update (){
        //Ensures clicking doesn't deselect buttons
        GameObject selected = ES.currentSelectedGameObject;
        
        if (selected != SelectedMenu || selected != SelectedMessage || selected != SelectedBag) {
            
            if (selected == null) {
                if (isMenuOpen) {
                    //menu is open 
                    ES.SetSelectedGameObject(SelectedMenu);
                } else if (Message.activeSelf) {
                    //message is open
                    ES.SetSelectedGameObject(SelectedMessage);
                } else if (Bag.activeSelf) {
                    //the bag is open
                    ES.SetSelectedGameObject(SelectedBag);
                } 
                //else nothing is open
            }
        }

        if (Input.GetButtonDown("Cancel")) {
            if (isBagOpen) {
                CloseBag();
                //put other sub menu things here, check the escape menu last
            } else if (isMenuOpen) {
                //menu is open so close it
                CloseMenu();
                Player.GetComponent<Walk>().canWalk = true; //this is so that you can open different menus from the escape menu
            } else if(!Message.activeSelf){
                OpenMenu();
            }
        } 

        if (isBagOpen) {
            //this handles the left/right arrows for bag tab changes
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                //then we want change the tab to the left
                currentTabIndex--;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                //then we want to change tab to the right
                currentTabIndex++;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                Debug.Log(offset);
                //offset = Mathf.Clamp(offset++, -2, currentItems.Count + 2);
                offset --;
                if (offset < -2) {
                    offset = -2;
                } else if (offset > currentItems.Count -2) {
                    offset = currentItems.Count - 2;
                }
                //else the offset is fine
                Debug.Log(offset);
                UpdateItemList();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                Debug.Log(offset);
                //offset = Mathf.Clamp(offset--, -2, currentItems.Count + 2);
                offset ++;
                if (offset < -2) {
                    offset = -2;
                } else if (offset > currentItems.Count -2) {
                    offset = currentItems.Count - 2;
                }
                //else the offset is fine
                Debug.Log(offset);
                UpdateItemList();
            }
        }
    }

    public void disableButton(){
        Message.GetComponent<Button>().interactable = false;
        Message.SetActive(false);
        Player.GetComponent<Walk>().canWalk = true;
        Invoke("reActivateInteract", 0.0167f);
    }

    private void reActivateInteract () {
        Player.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ShowMessage(string msg){
        Debug.Log("A message (for now)");
        Player.transform.GetChild(0).gameObject.SetActive(false); //gets the first object that is the players child (in this case the interact area)
        Player.GetComponent<Walk>().canWalk = false;
        Message.SetActive(true);
        Message.GetComponent<Button>().interactable = true;
        ES.SetSelectedGameObject(Message);
        Text.GetComponent<Text>().text = msg;
    }

    public void OpenMenu () {
        //doesnt do anything but this is where ill put the code to open the in game menu
        Menu.SetActive(true);
        isMenuOpen = true;
        ES.SetSelectedGameObject(null);
        ES.SetSelectedGameObject(SelectedMenu);
        Player.GetComponent<Walk>().canWalk = false;
    }

    public void CloseMenu () {
        //this is mainly for if theres anything else we want to put here
        Menu.SetActive(false);
        isMenuOpen = false;
        //Player.GetComponent<Walk>().canWalk = true;
    }

    public void OpenSummary () {
        //will be called when the summary button is pressed from the menu
        ShowMessage("This will show the summary of the adventure so far.");

    }

    public void OpenParty () {
        //will be called when the party option is selected fromt the menu
        ShowMessage("This will show the spirit party.");
    }

    public void OpenBag () {
        //opens the players bag when selected form the menu
        //ShowMessage("This will show the player inventory.");
        isBagOpen = true;
        Bag.SetActive(true);
        ES.SetSelectedGameObject(null);
        ES.SetSelectedGameObject(SelectedBag);
        Player.GetComponent<Walk>().canWalk = false;

        //set up bag here:
        Inventory inventory = GameObject.Find("WalkableCharacter").GetComponent<Inventory>();
        //bag tab
        bagTabs = new List<string>(inventory.tabs.Keys);
        int tabTitleIndex = 2;
        Bag.transform.GetChild(tabTitleIndex).GetComponent<Text>().text = bagTabs[currentTabIndex%bagTabs.Count]; 
            //this shouldn't come up with an IndexOutOfBounds Exception
        
        //item lineup
        currentItems = inventory.GetList(bagTabs[currentTabIndex%bagTabs.Count]);
        UpdateItemList();
        
    }

    private void UpdateItemList () {
        int ItemIndex = 1; //for clarity in the the GetChild

        for (int i = 0; i < 5; i++) {
            string item;
            if (i+offset < 0 || i+offset >= currentItems.Count) {
                //this is to account for the 2 above and below the actual item button
                item = " ";
            } else {
                item = currentItems[i + offset].displayName;
            }
            //then set the item text to the item
            Bag.transform.GetChild(ItemIndex).GetChild(i).GetComponentInChildren<Text>().text = item;
        }
    }

    public void useItem () {
        //function for the item button
    }

    public void CloseBag () {
        Bag.SetActive(false);
        isBagOpen = false;
        Player.GetComponent<Walk>().canWalk = true;
    }

    public void Save () {
        //will be called when the player selects save from the menu
        //for right now doesnt do anything
        ShowMessage("This will save the game.");
    }

    public void OpenOptions () {
        //opens the ingame options menu from the menu
        ShowMessage("This will show the options.");
    }
}
