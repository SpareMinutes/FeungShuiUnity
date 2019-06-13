using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleMenu : MonoBehaviour{
    public EventSystem ES;
    public GameObject Message, Moves;
    private GameObject Selected;
    private bool IsSelectingAttack;

    void Start(){
        Selected = ES.firstSelectedGameObject;
        IsSelectingAttack = false;
        Moves.SetActive(false);
    }

    void Update(){
        if(ES.currentSelectedGameObject != Selected){
            if (ES.currentSelectedGameObject == null)
                ES.SetSelectedGameObject(Selected);
            else
                Selected = ES.currentSelectedGameObject;
        }
        if (IsSelectingAttack && Input.GetAxis("Cancel")!=0){
            Message.SetActive(true);
            Moves.SetActive(false);
            IsSelectingAttack = true;
            GameObject.Find("Attack").GetComponent<Button>().interactable = true;
            GameObject.Find("Defend").GetComponent<Button>().interactable = true;
            GameObject.Find("Item").GetComponent<Button>().interactable = true;
            GameObject.Find("Run").GetComponent<Button>().interactable = true;
            ES.SetSelectedGameObject(GameObject.Find("Attack"));
        }
    }

    public void Attack(){
        Message.SetActive(false);
        Moves.SetActive(true);
        IsSelectingAttack = true;
        GameObject.Find("Attack").GetComponent<Button>().interactable = false;
        GameObject.Find("Defend").GetComponent<Button>().interactable = false;
        GameObject.Find("Item").GetComponent<Button>().interactable = false;
        GameObject.Find("Run").GetComponent<Button>().interactable = false;
        ES.SetSelectedGameObject(GameObject.Find("Attack1"));
    }

}
