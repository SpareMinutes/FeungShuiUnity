using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OverworldMenu : Menu {
    private EventSystem ES;

    private GameObject Selected;

    public GameObject menu;

    public void Start() {
        ES = gameObject.GetComponent<EventSystem>();;
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
    }

    public void OpenBag() {
        OpenNewMenu("BagScreen");
    }


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
