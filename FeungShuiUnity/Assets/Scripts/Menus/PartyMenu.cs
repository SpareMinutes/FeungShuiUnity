using System;
using UnityEngine;
using UnityEngine.EventSystems; 
using UnityEngine.UI;

public class PartyMenu : Menu{
    private EventSystem ES;
    private GameObject Selected, LastCanvas;
    public OptionBox Actions;
    private bool inBattle;
    private int selectedIndex;
    private Creature selectedCreature;

    private bool toUse;

    #region General
    void Start(){
        ES = gameObject.GetComponent<EventSystem>();
        Selected = ES.firstSelectedGameObject;
        GameObject canvas = GameObject.Find("PartyCanvas");

        selectedIndex = -1;

        GameObject Player = GameObject.Find("WalkableCharacter");
        //Show the party's data
        for (int i = 0; i < 6; i++) {
            GameObject memberObject = canvas.transform.GetChild(i + 1).gameObject;
            try {
                Creature memberData = Player.GetComponent<Battle>().Party[i];
                memberObject.transform.GetChild(1).GetComponent<Text>().text = memberData.GetName();
                memberObject.transform.GetChild(3).GetComponent<Slider>().value = memberData.currentCriticalHealth / memberData.getMaxCriticalHealth();
                memberObject.transform.GetChild(4).GetComponent<Slider>().value = memberData.currentActiveHealth / memberData.getMaxActiveHealth();
                memberObject.transform.GetChild(5).GetComponent<Slider>().value = memberData.currentMana / memberData.getMaxMana();
                memberObject.SetActive(true);
            } catch (ArgumentOutOfRangeException e) {
                memberObject.SetActive(false);
            }
        }
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
            if (selectedIndex>=0) {
                Actions.gameObject.SetActive(false);
                selectedIndex = -1;
            } else {
                ReturnToLast();
            }
        }
    }
    #endregion

    public void Select(int id) {
        selectedIndex = id;
        selectedCreature = GameObject.Find("WalkableCharacter").GetComponent<Battle>().Party[id];

        if (LastMenu is BattleMenu) {
            string[] labels = {"Details", "Send Out"};
            Action[] actionsIn = {() => ShowDetails(), () => SwitchOut()};
            Actions.Populate(labels, actionsIn);
        } else if (LastMenu is BagMenu) {
            string[] labels = {"Confirm"};
            Action[] actionsIn = {() => NotifySelection(selectedCreature)};
            Actions.Populate(labels, actionsIn);
        } else {
            //Todo: implement Creature.HasItem()
            //if(selectedCreature.HasItem()){
            //string[] labels = {"Details", "Switch", "Use Item", "Take Item"};
            //Action[] actionsIn = {() => ShowDetails(), () => SwitchOrder(), () => SelectItem(true), () => TakeItem()};
            //Actions.Populate(labels, actionsIn);
            //}else{
            string[] labels = {"Details", "Switch", "Use Item", "Give Item"};
            Action[] actionsIn = {() => ShowDetails(), () => SwitchOrder(), () => SelectItem(true), () => SelectItem(false)};
            Actions.Populate(labels, actionsIn);
            //}
        }
    }

    #region Using/giving items
    //TODO: Decide if we really want another copy of this stuff here
    public void UseItem(Item item, Creature creature) {
        //TODO: Implement this
    }

    public void GiveItem(Item item, Creature creature) {
        //TODO: Implement this
    }

    public void TakeItem(Creature creature) {
        //TODO: Implement this
    }
    #endregion

    #region Menu functions
    public void ShowDetails() {

    }
    
    //Bool: are you going to use the item? If not, you're going to give it.
    public void SelectItem(bool toUseIn) {
        OpenNewMenu("BagScreen");
    }

    public void SwitchOut() {

    }

    public void SwitchOrder() {

    }
    #endregion

    #region Menu interactions
    //Used to signal to the BagMenu that opened this menu which creature was selected
    public void NotifySelection(Creature result) {
        ((BagMenu)LastMenu).ReceiveNotify(result);
    }

    //Used to receive signal from the BagMenu which item was selected
    public void ReceiveNotify(Item result) {
        switch (toUse) {
            case true:
                //TODO: Code for what param to enter here instead of null
                UseItem(result, null);      break;
            case false:
                //TODO: Code for what param to enter here instead of null
                GiveItem(result, null);     break;
        }
    }
    #endregion
}
