using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BattleMenu : MonoBehaviour {
    public EventSystem ES;
    public GameObject Message, Moves;
    private GameObject Selected;
    private bool IsSelectingAttack;
    private Move SelectedMove;
    //Used to force only one selection to be cancelled per cancel button press
    private bool CancelHeld;
    private Creature Attacker;
    private List<Creature> Defenders; //just to support having several targets
    private string moveUsed;
    private List<Creature> toExculdeFromSelection;

    void Start(){
        ES.GetComponent<TurnManager>().Init();
        Selected = ES.firstSelectedGameObject;
        IsSelectingAttack = false;
        Moves.SetActive(false);
        SelectedMove = null;
        Defenders = new List<Creature>{};
        AskForAction();
    }

    void Update(){
        //Ensures clicking doesn't deselect buttons
        if (ES.currentSelectedGameObject != Selected){
            if (ES.currentSelectedGameObject == null)
                ES.SetSelectedGameObject(Selected);
            else
                Selected = ES.currentSelectedGameObject;
        }
        //Called when the the cancel button is pressed (esc)
        if (Input.GetAxis("Cancel") != 0){
            if (SelectedMove != null){ //Cancel target selection and return to move selection
                GameObject spirit = GameObject.Find("Spirit3Status");
                if(spirit!=null)
                    spirit.GetComponent<Button>().interactable = false;
                spirit = GameObject.Find("Spirit4Status");
                if (spirit != null)
                    spirit.GetComponent<Button>().interactable = false;
                SelectedMove = null;
                SelectAttack();
            }else if (IsSelectingAttack && !CancelHeld){ //Cancel move selection and return to action selection
                AskForAction();
            }
            CancelHeld = true;
        }
        //Cancel button has been released, reenable canceling
        else
            CancelHeld = false;
    }

    //Select action type (Attack/Defend/Item/Run)
    public void AskForAction(){
        Creature Defender;
        if (ES.GetComponent<TurnManager>().whoWins().Equals("Player")) {
            Debug.Log("Player Wins");
            //TODO: Make this load in the correct scene with correct player position
            PersistentStats.PlayerHasMoved = true;
            SceneManager.LoadScene("TestEnvironment", LoadSceneMode.Single);
        } else if (ES.GetComponent<TurnManager>().whoWins().Equals("Computer")) {
            Debug.Log("Player Looses");
            //TODO: Make this load in the correct scene with correct player position
            PersistentStats.PlayerHasMoved = true;
            SceneManager.LoadScene("TestEnvironment", LoadSceneMode.Single);
        } else if (ES.GetComponent<TurnManager>().whoWins().Equals("No-one")) {
            if (!IsSelectingAttack) {
                Attacker = ES.GetComponent<TurnManager>().getNextSpirit();
                Attacker.relieveDefenseMove(); //get rid of the defense move on their next turn 
            }
        
            if (Attacker.isPlayerOwned()) {
                ShowMessage("What will " + Attacker.displayName + " do?");
                //Enable action type buttons
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
                Defenders.Add(Defender);
                DoAttack();
            }
        }
        
    }

    //Called when the Attack option is pressed
    public void SelectAttack(){
        Message.SetActive(false); //Text in right most text box -> disable it
        Moves.SetActive(true); //Let the moves become selectable (shown)
        IsSelectingAttack = true; 
        //Disables the attack, defend, item, run buttons as buttons (text is still there)
        GameObject.Find("Attack").GetComponent<Button>().interactable = false;
        GameObject.Find("Defend").GetComponent<Button>().interactable = false;
        GameObject.Find("Item").GetComponent<Button>().interactable = false;
        GameObject.Find("Run").GetComponent<Button>().interactable = false;
        ES.SetSelectedGameObject(GameObject.Find("Attack0")); //Set the first (upper left) attack as the currently highlighed button
        //Goes through the list of moves of a creature and displays them in the rightmost text as selectable buttons
        int i = 0;
        while (i < Attacker.moveNames.Count){
            GameObject.Find("Attack" + i).GetComponentInChildren<Text>().text = Attacker.moveNames[i];
            GameObject.Find("Attack" + i).GetComponent<Button>().interactable = true;
            i++;
        }
        //Fills out the remainders with empty text and makes them not selectable
        while (i < 4){
            GameObject.Find("Attack" + i).GetComponentInChildren<Text>().text = "";
            GameObject.Find("Attack" + i).GetComponent<Button>().interactable = false;
            i++;
        }
    }

    //Called when a move is selected
    public void LoadAttack(){
        moveUsed = ES.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        SelectedMove = MovesMaster.Find(moveUsed); //gets the move out of database
        //if statement dealing with targeting type
        if (SelectedMove.AttackTarget == Move.Target.All) {
            targetAll();

        } else if (SelectedMove.AttackTarget == Move.Target.Ally) {
            targetAlly();

        } else if (SelectedMove.AttackTarget == Move.Target.Double) {
            targetDouble();

        } else if (SelectedMove.AttackTarget == Move.Target.Others) {
            targetOthers();

        } else if (SelectedMove.AttackTarget == Move.Target.Self) {
            targetSelf();

        } else if(SelectedMove.AttackTarget == Move.Target.Single){
            targetSingle();
        }
        disableMoves();
    }

    private void targetAll () {
        //needs to make the targetAll button interactable
        toExculdeFromSelection = new List<Creature>(){}; //blank list
        GameObject button = GameObject.Find("SelectMultipleButton");
        button.GetComponent<Button>().interactable = true;
        ES.SetSelectedGameObject(button);
    }

    private void targetAlly () {
        if (ES.GetComponent<TurnManager>().getActivePlayerControlled().Count == 2) /*to handle if there isnt 2 player controlled alive*/{
                GameObject selectableSpirit = null;
                GameObject spirit = GameObject.Find("Spirit1Status");
                if (spirit.GetComponent<CreatureBattleStatusController>().Target == Attacker) {
                    //change it to be the other player controlled
                    selectableSpirit = GameObject.Find("Spirit2Status");
                } else {
                    selectableSpirit = spirit;
                }
                if (selectableSpirit != null) {
                    selectableSpirit.GetComponent<Button>().interactable = true;
                }
                ES.SetSelectedGameObject(selectableSpirit);
            } else {
                Debug.Log("no allies alive");
                //maybe give the message that the move cannot be used? and then go back to teh select attack screen?
            }
    }

    private void targetDouble () {
        //this function needs to handle the filtering of the button
            //this means changing the layers so that it looks like the 
    }

    private void targetOthers () {
        //spirits other than the Attacker
            
            //get all spirits in play and remove the attacker
                //these are the selectable spirits
    }
    
    private void targetSelf () {
        GameObject selectableSpirit = null;
            for (int i = 1; i <= 4; i++) {
                GameObject spirit = GameObject.Find("Spirit" + i + "Status");
                if (spirit != null && spirit.GetComponent<CreatureBattleStatusController>().Target == Attacker) {
                    selectableSpirit = spirit;
                }
            }
            if (selectableSpirit != null) {
                selectableSpirit.GetComponent<Button>().interactable = true;
            }
            ES.SetSelectedGameObject(selectableSpirit);
            
    }

    private void targetSingle () {
        //Makes the opponents selectable for the move to target
            GameObject spirit1 = GameObject.Find("Spirit3Status");
            GameObject spirit2 = GameObject.Find("Spirit4Status");

            if(spirit1!=null) {
                spirit1.GetComponent<Button>().interactable = true;
            }
            if (spirit2 != null){
                spirit2.GetComponent<Button>().interactable = true;
                ES.SetSelectedGameObject(spirit2);
            }else {
                ES.SetSelectedGameObject(spirit1);
            }
            
    }


    /*just to save on a couple lines for something thats needed for every target selecting type */
    private void disableMoves () {
        //Makes moves non interactable
        for (int i=0; i<4; i++) {
            GameObject.Find("Attack" + i).GetComponent<Button>().interactable = false;
        }
    }

    //Called by defender when they're selected telling script to target them
    public void LoadDefender(){
        Defenders.Add(Selected.GetComponent<CreatureBattleStatusController>().Target);
        //disable the spirit buttons
        for (int i = 1; i<=4; i++) {
            GameObject spirit = GameObject.Find("Spirit" + i + "Status");
            if (spirit != null) {
                spirit.GetComponent<Button>().interactable = false;
            }
        }
    }

    //Called for having multiple enemies
    public void LoadDefenders () {
        List<Creature> activeCreatures = ES.GetComponent<TurnManager>().getAllActive();
        Defenders = activeCreatures.Except(toExculdeFromSelection).ToList();
        //disable the button
        GameObject.Find("SelectMultipleButton").GetComponent<Button>().interactable = false;
    }

    //Called when the oppoenent is selected
    public void DoAttack(){
        foreach (Creature Defender in Defenders) {
            ShowMessage(Attacker.displayName + " used " + moveUsed + " on " + Defender.displayName + ".");
            SelectedMove.execute(Attacker, Defender);
        }
        //clear the defenders list
        Defenders.Clear();
        IsSelectingAttack = false;
        //possibly want to make the player press the enter key to progress
        //so they dont miss something they might want to see (the result of their moves)
        //also gives weight to the enemy's turn
        Invoke("AskForAction", 1);
        
    }

    //Called when a message is to be displayed in the bottom right box
    private void ShowMessage(string msg){
        //Turn on the message box and set the text
        Message.SetActive(true);
        Message.GetComponent<Text>().text = msg;
        //Disable attack buttons
        Moves.SetActive(false);
        IsSelectingAttack = false;
    }
    
    public void LoadDefend () {
        Attacker.doDefenseMove();
        //since it has no targets it doesnt need a target selection
        //show/remove appropriate buttons
        //show message that the spririt used defend
        ShowMessage(Attacker.displayName + " used Defend!");
        //progress the turn cycle
        Invoke("AskForAction", 1);
    }

    public void Run(){
        //telling the script to use the new player coordinates saved when the battle was engaged
        PersistentStats.PlayerHasMoved = true;
        //load the world again (probably also want to save the scene view for the route the player was last in)
        SceneManager.LoadScene("TestEnvironment", LoadSceneMode.Single);
    }
}
