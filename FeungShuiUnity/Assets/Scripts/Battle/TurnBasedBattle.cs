using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public enum GameState {
    SelectAction, //choose attack, defend, item, spirits, or run
    SelectMove, //player selected attack
    SelectTarget, //after selecting a move
    SelectItem, //player selected item
    SelectSpirits, //player chose spirits
    EnemyTurn //enemy AI
}

public class TurnBasedBattle : MonoBehaviour {

    public EventSystem eventSystem;
    public GameState gameState;
    public List<CreatureBattleStatusController> inBattleCreatures = new List<CreatureBattleStatusController>(); //this will be populated by the two characters in the battle
    private CreatureBattleStatusController activeCreature;
    private bool UIHasChanged = false;

    //UI elements to be toggled//
    public Text Message;
    public GameObject SelectActionUI, SelectItemUI, SelectMoveUI, SelectSpiritsUI, SelectTargetUI, EnemyTurnUI;


    public void Start () {
        // setup //

        //decide on the order by creature speed
        sortCreatures();
        activeCreature = inBattleCreatures[0];

        //choose who starts
        if (activeCreature.Target.isPlayerOwned()) {
            //then its the players turn
            gameState = GameState.SelectAction;
        } else {
            //enemy goes first
            gameState = GameState.EnemyTurn;
        }
    }
    /*sorts the currently active creatures in order from fastest to slowest for choosing which one is next in turn progression*/
    private void sortCreatures () {
        //just arranges the creatures so that they are in order from quickest to slowest
        List<CreatureBattleStatusController> sortedCreatures = new List<CreatureBattleStatusController>();
        List<CreatureBattleStatusController> dummyList = new List<CreatureBattleStatusController>(inBattleCreatures); //copy of all creatures in battle

        //find the quickest
        while (sortedCreatures.Count < inBattleCreatures.Count) {
            CreatureBattleStatusController toAdd = getFastest(dummyList);
            dummyList.Remove(toAdd);
            sortedCreatures.Add(toAdd);
        }

        inBattleCreatures = new List<CreatureBattleStatusController>(sortedCreatures); //replace the list
    }
    /*helper method for the sortCreatures() method to find the fastes creature out of a list*/
    private CreatureBattleStatusController getFastest (List<CreatureBattleStatusController> creatureList) {
        CreatureBattleStatusController tempFastest = creatureList[0];

        foreach (CreatureBattleStatusController creature in creatureList) {
            if (creature.getSpeed() > tempFastest.getSpeed()) {
                tempFastest = creature;
            }
        }

        return tempFastest;
    }


    public void Update () {
        //handle the UI and mechanic changes here
        switch (gameState) {
            case GameState.SelectAction : {
                SelectAction();
                break;
            } case GameState.SelectItem : {
                SelectItem();
                break;
            } case GameState.SelectMove : {
                SelectMove();
                break;
            } case GameState.SelectSpirits : {
                SelectSpirits();
                break;
            } case GameState.SelectTarget : {
                SelectTarget();
                break;
            } case GameState.EnemyTurn : {
                EnemyTurn();
                break;
            }
        }
        if (!UIHasChanged) {
            changeActiveUI();
        }
    }

    private void SelectAction () {
        //here the player will choose between attack, defend, item, spirits, and run
    }
    public void Attack () {
        //when the player selects the attack option//
        //switch to the SelectMove GameState
        gameState = GameState.SelectMove;
        UIHasChanged = false;
        //make it so that each of the 4 moves are the ones that the current active creature has
        
    }
    public void Defend () {
        //when the player selects the defend option//
        //doesnt have a game state so just execute the defend move
        activeCreature.doDefenseMove();
        
        //progress the turn cycle
        string msg = "" + activeCreature.Target.displayName + " used defend!";
        ShowMessage(msg);
    }
    public void Item () {
        //when the player selects the item option//
    }
    public void Spirits () {
        //when the player selects the spirits option//
    }
    public void Run () {
        //when the player selects the run option//
    }


    private void SelectItem () {
        //open the bag and select an item to use on the current spirit
    }

    private void SelectMove () {
        //after selecting attack this will open up the moves menu for the current spirit
    }

    private void SelectSpirits () {
        //this will open up the party menu for the player to choose another spirit to switch in
    }

    private void SelectTarget () {
        //after selecting an attack to use this will show the targetting UI so select a target for the move
    }

    private void EnemyTurn () {
        //the enemy AI will go here 
    }
   

   private void changeActiveUI () {
       //this makes sure that only the correct UI for the game state is interatable
        //should only happen once per change of the gameState
        switch (gameState) {
            case GameState.SelectAction : {

                break;
            }
        }

        //SecletAction//
        foreach (Transform child in SelectActionUI.transform) {
            if (gameState == GameState.SelectAction) {
                child.GetComponent<Button>().interactable = true;
            } else {
                child.GetComponent<Button>().interactable = false;
            }
        }
        //SelectItem//
        if (gameState == GameState.SelectItem) {
            SelectItemUI.SetActive(true);
        } else {
            SelectItemUI.SetActive(false);
        }
        //SelectMove//
        if (gameState == GameState.SelectMove) {
            SelectMoveUI.SetActive(true);
        } else {
            SelectMoveUI.SetActive(false);
        }
        //SelectSpirits//
        if (gameState == GameState.SelectSpirits) {
            SelectSpiritsUI.SetActive(true);
        } else {
            SelectSpiritsUI.SetActive(false);
        }
        //SelectTarget//
        if (gameState == GameState.SelectTarget) {
            SelectTargetUI.SetActive(true);
        } else {
            SelectTargetUI.SetActive(false);
        }
        //EnemyTurn//
        if (gameState == GameState.EnemyTurn) {
            EnemyTurnUI.SetActive(true);
        } else {
            EnemyTurnUI.SetActive(false);
        }

        UIHasChanged = true;
   }

   public void ProgressTurnCycle () {
       //this is run after an action has been taken
       activeCreature = inBattleCreatures[(inBattleCreatures.IndexOf(activeCreature) + 1)%inBattleCreatures.Count];

       if (activeCreature.isDefending) {
           activeCreature.relieveDefenseMove();
       }

       if (activeCreature.Target.isPlayerOwned()) {
           //then change the game state
           gameState = GameState.SelectAction;
       } else {
           //its an enemy
           gameState = GameState.EnemyTurn;
       }
   }

   public void CloseMessage () {
       //just closes the message
       Message.gameObject.SetActive(false);
   }

   private void ShowMessage (string msg) {
       //show a message of what just happened
       //going past this will progress the turn cycle
       Message.gameObject.SetActive(true);
       Message.text = msg;
       eventSystem.SetSelectedGameObject(Message.GetComponentInChildren<Button>().gameObject);
   }

   
}
