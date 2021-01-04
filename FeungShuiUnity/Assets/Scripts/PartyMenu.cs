using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;

public class PartyMenu : MonoBehaviour {
    private EventSystem ES;
    private GameObject Selected, LastCanvas;
    public GameObject Actions;
    private bool inBattle, isSelecting;

    public void setInBattle(bool value) {
        inBattle = value;
    }

    // Start is called before the first frame update
    void Start(){
        ES = gameObject.GetComponent<EventSystem>();
        Selected = ES.firstSelectedGameObject;
        GameObject Canvas = GameObject.Find("PartyCanvas");
        //Disable battle screen or other, if any
        LastCanvas = GameObject.Find("Canvas");
        if (LastCanvas != null) LastCanvas.SetActive(false);

        isSelecting = false;

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
            if (isSelecting) {
                Actions.SetActive(false);
                isSelecting = false;
            } else {
                if (LastCanvas != null) LastCanvas.SetActive(true);
                SceneManager.UnloadSceneAsync("PartyScreen");
            }
        }
    }

    public void Select(int id) {
        isSelecting = true;
        Actions.transform.GetChild(1).GetComponent<Button>().interactable = LastCanvas != null;
        Actions.SetActive(true);
    }
}
