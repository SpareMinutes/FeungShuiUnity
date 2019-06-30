﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleMenu : MonoBehaviour {
    public EventSystem ES;
    public GameObject Message, Moves;
    private GameObject Selected;
    private bool IsSelectingAttack;
    private Move SelectedMove;
    private bool CancelHeld;
    private Creature Attacker, Defender;
    private string moveUsed;

    void Start(){
        ES.GetComponent<TurnManager>().sortBySpeed();
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
        //called when the the cancel button is pressed (esc)
        if (Input.GetAxis("Cancel") != 0){
            if (SelectedMove != null){ //for chaning descision on the move you want to make
                GameObject.Find("Spirit3Status").GetComponent<Button>().interactable = false;
                GameObject.Find("Spirit4Status").GetComponent<Button>().interactable = false;
                SelectedMove = null;
                SelectAttack();
            }else if (IsSelectingAttack && !CancelHeld){ //for when 
                AskForAction();
            }
            CancelHeld = true;
        }
        else
            CancelHeld = false;
    }

    public void AskForAction(){
        if (!IsSelectingAttack) {
            ES.GetComponent<TurnManager>().checker();
            Attacker = ES.GetComponent<TurnManager>().getNextSpirit();
        }
        
        if (Attacker.isPlayerOwned()) {
            Message.SetActive(true);
            Message.GetComponent<Text>().text = "What will " + Attacker.displayName + " do?";
            Moves.SetActive(false);
            IsSelectingAttack = true;
            GameObject.Find("Attack").GetComponent<Button>().interactable = true;
            GameObject.Find("Defend").GetComponent<Button>().interactable = true;
            GameObject.Find("Item").GetComponent<Button>().interactable = true;
            GameObject.Find("Run").GetComponent<Button>().interactable = true;
            ES.SetSelectedGameObject(GameObject.Find("Attack"));
        } else {
            // AI part //
            /* AI needs to:
                - decide whether to attack or defend or use an item (for wild spirits they shouldnt be able to use items)
                - choose an attack
                - choose a target (should be the opposite target choices as the player)
             */
            //ignoring the first descision because they have no functionality yet. 
            //so assuming the ai chooses attack

            //this should choose a random move from the enemy's move set (even if they have less than 4 moves)
            int randNum = Random.Range(0,Attacker.moveNames.Count);
            moveUsed = Attacker.moveNames[randNum];
            SelectedMove = MovesMaster.Find(moveUsed);

            //need to choose a "enemy" to attack
            List<Creature> toChoose = ES.GetComponent<TurnManager>().getActivePlayerControlled();
            int randNum2 = Random.Range(0,toChoose.Count);
            Defender = toChoose[randNum2];
            IsSelectingAttack = true;
            DoAttack();
        }
    }
    
    public void SelectAttack(){
        //called when the Attack option is pressed

        Message.SetActive(false); //text in right most text box -> disable it
        Moves.SetActive(true); //let the moves become selectable (shown)
        IsSelectingAttack = true; 
        //disables the attack, defend, item, run buttons as buttons (text is still there)
        GameObject.Find("Attack").GetComponent<Button>().interactable = false;
        GameObject.Find("Defend").GetComponent<Button>().interactable = false;
        GameObject.Find("Item").GetComponent<Button>().interactable = false;
        GameObject.Find("Run").GetComponent<Button>().interactable = false;
        //goes through the list of moves of a creature and displays them in the rightmost text as selectable buttons
        ES.SetSelectedGameObject(GameObject.Find("Attack0"));
        int i = 0;
        while (i < Attacker.moveNames.Count){
            GameObject.Find("Attack" + i).GetComponentInChildren<Text>().text = Attacker.moveNames[i];
            GameObject.Find("Attack" + i).GetComponent<Button>().interactable = true;
            i++;
        }
        //just fills out the remainders with empty text and makes them not selectable
        while (i < 4){
            GameObject.Find("Attack" + i).GetComponentInChildren<Text>().text = "";
            GameObject.Find("Attack" + i).GetComponent<Button>().interactable = false;
            i++;
        }
    }

    public void LoadAttack(){
        //called when a move is selected

        moveUsed = ES.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        SelectedMove = MovesMaster.Find(moveUsed); //gets the move out of database
        //if statement dealing with targeting type
        if(SelectedMove.AttackTarget == Move.Target.Single){
            //makes the opponents selectable for hte move to target
            //*******probably want to go through the turn cycle and get all non player controlled creatures and make them targetable*********
            GameObject.Find("Spirit3Status").GetComponent<Button>().interactable = true;
            GameObject.Find("Spirit4Status").GetComponent<Button>().interactable = true;
            ES.SetSelectedGameObject(GameObject.Find("Spirit4Status"));
            //makes moves non interactable
            for(int i=0; i<4; i++)
                GameObject.Find("Attack" + i).GetComponent<Button>().interactable = false;
        }
        //To do: attacks with different targeting types and cases with only one opponent
        
    }

    public void LoadDefender(){
        //called by defender whent they're selected telling script to target them
        Defender = Selected.GetComponent<CreatureBattleStatusController>().Target;
    }

    public void DoAttack(){
        //called when the oppoenent is selected
        Debug.Log(Attacker.displayName + " is attacking " + Defender.displayName + " with " + moveUsed);
        SelectedMove.execute(Attacker, Defender);
        IsSelectingAttack = false;
        AskForAction();
    }
}
