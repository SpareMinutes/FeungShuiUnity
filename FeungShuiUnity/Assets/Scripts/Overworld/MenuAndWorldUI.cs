using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuAndWorldUI : MonoBehaviour{
    public EventSystem ES;
    [SerializeField]
    private GameObject Player, Canvas, Message, Text, Menu;
    private bool isMenuOpen = false;
    private GameObject SelectedMenu, SelectedMessage;

    public void Start(){
        SelectedMenu = Menu.transform.GetChild(0).gameObject;
        SelectedMessage = Message;
    }

    public void Update (){
        //Ensures clicking doesn't deselect buttons
        
        if (ES.currentSelectedGameObject != SelectedMenu || ES.currentSelectedGameObject != SelectedMessage) {
            
            if (ES.currentSelectedGameObject == null) {
                if (isMenuOpen) {
                    //menu is open 
                    ES.SetSelectedGameObject(SelectedMenu);
                } else if (Message.activeSelf) {
                    //message is open
                    ES.SetSelectedGameObject(SelectedMessage);
                } else {
                    //the UI isnt open
                }
            }
        }

        if (Input.GetButtonDown("Cancel")) {
            if (isMenuOpen) {
                //menu is open so close it
                CloseMenu();
            } else if(!Message.activeSelf){
                OpenMenu();
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
        Player.GetComponent<Walk>().canWalk = true;
    }
}
