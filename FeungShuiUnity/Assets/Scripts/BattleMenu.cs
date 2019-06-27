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
    private Move SelectedMove;
    private bool CancelHeld;
    private Creature Attacker, Defender;

    void Start(){
        Selected = ES.firstSelectedGameObject;
        IsSelectingAttack = false;
        Moves.SetActive(false);
        SelectedMove = null;
        AskForAction();
    }

    void Update(){
        if (ES.currentSelectedGameObject != Selected){
            if (ES.currentSelectedGameObject == null)
                ES.SetSelectedGameObject(Selected);
            else
                Selected = ES.currentSelectedGameObject;
        }
        if (Input.GetAxis("Cancel") != 0){
            if (SelectedMove != null){
                GameObject.Find("Spirit3Status").GetComponent<Button>().interactable = false;
                GameObject.Find("Spirit4Status").GetComponent<Button>().interactable = false;
                SelectedMove = null;
                SelectAttack();
            }else if (IsSelectingAttack && !CancelHeld){
                AskForAction();
            }
            CancelHeld = true;
        }
        else
            CancelHeld = false;
    }

    public void AskForAction(){
        Attacker = ES.GetComponent<TurnManager2>().getNextSpirit();
        Message.SetActive(true);
        Message.GetComponent<Text>().text = "What will " + Attacker.displayName + " do?";
        Moves.SetActive(false);
        IsSelectingAttack = true;
        GameObject.Find("Attack").GetComponent<Button>().interactable = true;
        GameObject.Find("Defend").GetComponent<Button>().interactable = true;
        GameObject.Find("Item").GetComponent<Button>().interactable = true;
        GameObject.Find("Run").GetComponent<Button>().interactable = true;
        ES.SetSelectedGameObject(GameObject.Find("Attack"));
    }

    public void SelectAttack(){
        Message.SetActive(false);
        Moves.SetActive(true);
        IsSelectingAttack = true;
        GameObject.Find("Attack").GetComponent<Button>().interactable = false;
        GameObject.Find("Defend").GetComponent<Button>().interactable = false;
        GameObject.Find("Item").GetComponent<Button>().interactable = false;
        GameObject.Find("Run").GetComponent<Button>().interactable = false;
        ES.SetSelectedGameObject(GameObject.Find("Attack0"));
        int i = 0;
        while (i < Attacker.moveNames.Count){
            GameObject.Find("Attack" + i).GetComponentInChildren<Text>().text = Attacker.moveNames[i];
            GameObject.Find("Attack" + i).GetComponent<Button>().interactable = true;
            i++;
        }
        while (i < 4){
            GameObject.Find("Attack" + i).GetComponentInChildren<Text>().text = "";
            GameObject.Find("Attack" + i).GetComponent<Button>().interactable = false;
            i++;
        }
    }

    public void LoadAttack(){
        SelectedMove = MovesMaster.Find(ES.currentSelectedGameObject.GetComponentInChildren<Text>().text);
        if(SelectedMove.AttackTarget == Move.Target.Single){
            GameObject.Find("Spirit3Status").GetComponent<Button>().interactable = true;
            GameObject.Find("Spirit4Status").GetComponent<Button>().interactable = true;
            ES.SetSelectedGameObject(GameObject.Find("Spirit4Status"));
            for(int i=0; i<4; i++)
                GameObject.Find("Attack" + i).GetComponent<Button>().interactable = false;
        }
        //To do: attacks with different targeting types and cases with only one opponent
    }

    public void LoadDefender(){
        Defender = Selected.GetComponent<CreatureBattleStatusController>().Target;
    }

    public void DoAttack(){
        SelectedMove.execute(Attacker, Defender);
        //move onto next opponent?
    }
}
