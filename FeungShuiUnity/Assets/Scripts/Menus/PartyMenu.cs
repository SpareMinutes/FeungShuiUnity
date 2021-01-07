using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;

public class PartyMenu : Menu{
    private EventSystem ES;
    private GameObject Selected, LastCanvas;
    public OptionBox Actions;
    private bool inBattle;
    private int selectedIndex;

    #region General
    // Start is called before the first frame update
    void Start(){
        ES = gameObject.GetComponent<EventSystem>();
        Selected = ES.firstSelectedGameObject;
        GameObject Canvas = GameObject.Find("PartyCanvas");

        selectedIndex = -1;

        GameObject Player = GameObject.Find("WalkableCharacter");
        //Show the party's data
        for (int i = 0; i < 6; i++) {
            GameObject memberObject = Canvas.transform.GetChild(i + 1).gameObject;
            try {
                Creature memberData = Player.GetComponent<Battle>().Party[i];
                memberObject.transform.GetChild(1).GetComponent<Text>().text = memberData.name;
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
        if(LastCanvas == null) {
            string[] labels = { "Details" };
            Action[] actionsIn = { () => ShowDetails() };
            Actions.Populate(labels, actionsIn);
        }
    }

    public void ShowDetails() {

    }

    public void SwitchOut() {

    }
}
