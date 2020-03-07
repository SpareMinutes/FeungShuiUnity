using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BattleMenu : MonoBehaviour{
    public EventSystem ES;
    public GameObject Message, Moves;
    private GameObject Selected, OES;
    private bool IsSelectingAttack;
    private Move SelectedMove;
    private CreatureBattleStatusController Attacker;
    private List<CreatureBattleStatusController> Defenders; //just to support having several targets
    private MoveName moveUsed;
    private List<CreatureBattleStatusController> toExculdeFromSelection;
    public GameObject[] spiritStatuses, attackButtons, toggles;
    public GameObject ProgressButton;
    public MovesTable movesTable;

    /*	---------------------------
	*	General functions
	*	---------------------------	*/

    void Start(){
        movesTable.Init(); //initialise the movesTable dict
        OES = GameObject.Find("EventSystem");
        OES.SetActive(false);
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().enabled = false;
        ES.GetComponent<TurnManager>().Init();
        Selected = ES.firstSelectedGameObject;
        IsSelectingAttack = false;
        Moves.SetActive(false);
        SelectedMove = null;
        Defenders = new List<CreatureBattleStatusController> { };
        toExculdeFromSelection = new List<CreatureBattleStatusController>() { }; //blank list
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
        if (Input.GetButtonDown("Cancel")){
            if (SelectedMove != null){ //Cancel target selection and return to move selection
                if (spiritStatuses[2].activeSelf)
                    spiritStatuses[2].GetComponent<Button>().interactable = false;
                if (spiritStatuses[3].activeSelf)
                    spiritStatuses[3].GetComponent<Button>().interactable = false;
                SelectedMove = null;
                SelectAttack();
                resetSelection();
                IsSelectingAttack = true;
                GameObject.Find("SelectMultipleButton").GetComponent<Button>().interactable = false;
            }else if (IsSelectingAttack){ //Cancel move selection and return to action selection
                AskForAction();
            }
        }
    }

    /*	---------------------------
	*	Main menu functions
	*	---------------------------	*/

    //Called when a message is to be displayed in the bottom right box
    private void ShowMessage(string msg){
        //Turn on the message box and set the text
        Message.SetActive(true);
        Message.GetComponent<Text>().text = msg;
        //Disable attack buttons
        Moves.SetActive(false);
        IsSelectingAttack = false;
    }

    //Select action type (Attack/Defend/Item/Run)
    public void AskForAction(){
        if (ES.GetComponent<TurnManager>().whoWins().Equals("Player")){
            Debug.Log("Player Wins");
            EndBattle();
        } else if (ES.GetComponent<TurnManager>().whoWins().Equals("Computer")){
            Debug.Log("Player Looses");
            EndBattle();
        } else if (ES.GetComponent<TurnManager>().whoWins().Equals("No-one")){
            if (!IsSelectingAttack){
                Attacker = ES.GetComponent<TurnManager>().getNextSpirit();
                Attacker.relieveDefenseMove(); //get rid of the defense move on their next turn 
            }
            if (Attacker.GetCreature().isPlayerOwned()){
                ShowMessage("What will " + Attacker.GetCreature().displayName + " do?");
                //Enable action type buttons
                GameObject.Find("Attack").GetComponent<Button>().interactable = true;
                GameObject.Find("Defend").GetComponent<Button>().interactable = true;
                GameObject.Find("Item").GetComponent<Button>().interactable = true;
                GameObject.Find("Spirits").GetComponent<Button>().interactable = true;
                GameObject.Find("Run").GetComponent<Button>().interactable = true;
                ES.SetSelectedGameObject(GameObject.Find("Attack"));
            }else{
                EnemyAI();
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
        GameObject.Find("Spirits").GetComponent<Button>().interactable = false;
        GameObject.Find("Run").GetComponent<Button>().interactable = false;
        ES.SetSelectedGameObject(attackButtons[0]); //Set the first (upper left) attack as the currently highlighed button
        //Goes through the list of moves of a creature and displays them in the rightmost text as selectable buttons
        int i = 0;
        while (i < Attacker.GetCreature().moveNames.Count){
            attackButtons[i].GetComponentInChildren<Text>().text = Attacker.GetCreature().moveNames[i].ToString();
            attackButtons[i].GetComponent<Button>().interactable = true;
            i++;
        }
        //Fills out the remainders with empty text and makes them not selectable
        while (i < 4){
            attackButtons[i].GetComponentInChildren<Text>().text = "";
            attackButtons[i].GetComponent<Button>().interactable = false;
            i++;
        }
    }

    public void LoadDefend(){
        Attacker.doDefenseMove();
        //since it has no targets it doesnt need a target selection
        //show/remove appropriate buttons
        //show message that the spririt used defend
        ShowMessage(Attacker.GetCreature().displayName + " used Defend!");
        //progress the turn cycle
        LoadProgress();
    }

    public void SelectItem(){
        ShowMessage("" + Attacker.GetCreature().displayName + " used an [ITEM] (WIP)");
        LoadProgress();
    }

    public void Run(){
        //TODO: Add code to decide whether running is possible
        EndBattle();
    }

    private void LoadProgress () {
        ProgressButton.GetComponent<Button>().interactable = true;
        ES.SetSelectedGameObject(ProgressButton);
    }

    public void Progress () {
        ProgressButton.GetComponent<Button>().interactable = false;
        AskForAction();
    }

    /*	---------------------------
	*	Attacking functions
	*	---------------------------	*/

    //Called when the oppoenent is selected
    public void DoAttack(){
        foreach (CreatureBattleStatusController Defender in Defenders){
            ShowMessage(Attacker.GetCreature().displayName + " used " + moveUsed + " on " + Defender.GetCreature().displayName + ".");
            SelectedMove.execute(Attacker, Defender);
        }
        resetSelection();

        //possibly want to make the player press the enter key to progress
        //so they dont miss something they might want to see (the result of their moves)
        //also gives weight to the enemy's turn
        LoadProgress();
    }

    private int findAttacker(){
        for (int i = 1; i <= 2; i++){
            GameObject spirit = spiritStatuses[i - 1];
            if (spirit.activeSelf && spirit.GetComponent<CreatureBattleStatusController>().Target == Attacker.GetCreature()) {
                return (i-1);
            }
        }
        return -1;
    }

    //Called by defender when they're selected telling script to target them
    public void LoadDefender(){
        Defenders.Add(Selected.GetComponent<CreatureBattleStatusController>());
        //disable the spirit buttons
        for (int i = 1; i <= 4; i++){
            GameObject spirit = spiritStatuses[i - 1];
            if (spirit.activeSelf){
                spirit.GetComponent<Button>().interactable = false;
            }
        }
    }

    //Called for having multiple enemies
    public void LoadDefenders(){
        List<CreatureBattleStatusController> activeCreatures = ES.GetComponent<TurnManager>().getAllActive();
        Defenders = activeCreatures.Except(toExculdeFromSelection).ToList();
        //disable the button
        GameObject.Find("SelectMultipleButton").GetComponent<Button>().interactable = false;
    }

    //just to divide up functions 
    private void resetSelection(){
        Defenders.Clear();
        toExculdeFromSelection.Clear();
        SelectedMove = null;    
        IsSelectingAttack = false;

        for (int i = 0; i < 4; i++){
            toggles[i].GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        }
    }

    //Called when a move is selected
    public void LoadAttack(){
        //moveUsed = ES.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        SelectedMove = MovesTable.Find(moveUsed); //gets the move out of database
        //switch/case statement dealing with targeting type
        switch (SelectedMove.AttackTarget) {
            case Move.Target.Single:
                targetSingle();
                break;
            case Move.Target.Self:
                targetSelf();
                break;
            case Move.Target.Ally:
                targetAlly();
                break;
            case Move.Target.Double:
                targetDouble();
                break;
            case Move.Target.Team:
                targetTeam();
                break;
            case Move.Target.Others:
                targetOthers();
                break;
            case Move.Target.All:
                targetAll();
                break;
        }
        disableMoves();
    }

    /*just to save on a couple lines for something thats needed for every target selecting type */
    private void disableMoves(){
        //Makes moves non interactable
        for (int i = 0; i < 4; i++){
            attackButtons[i].GetComponent<Button>().interactable = false;
        }
    }

    /*	---------------------------
	*	Targeting functions
	*	---------------------------	*/

    private void targetSingle(){
        //Makes the opponents selectable for the move to target
        GameObject spirit1 = spiritStatuses[2];
        GameObject spirit2 = spiritStatuses[3];

        if (spirit1.activeSelf){
            spirit1.GetComponent<Button>().interactable = true;
        }
        if (spirit2.activeSelf){
            spirit2.GetComponent<Button>().interactable = true;
            ES.SetSelectedGameObject(spirit2);
        }else{
            ES.SetSelectedGameObject(spirit1);
        }
    }

    private void targetSelf(){
        GameObject selectableSpirit = null;
        selectableSpirit = spiritStatuses[findAttacker()];
        if (selectableSpirit != null){
            selectableSpirit.GetComponent<Button>().interactable = true;
        }
        ES.SetSelectedGameObject(selectableSpirit);
    }

    private void targetAlly(){
        if (ES.GetComponent<TurnManager>().getActivePlayerControlled().Count == 2){ /*to handle if there isnt 2 player controlled alive*/
            GameObject selectableSpirit = null;
            GameObject spirit = spiritStatuses[0];
            if (spirit.GetComponent<CreatureBattleStatusController>().Target == Attacker.GetCreature()) {
                //change it to be the other player controlled
                selectableSpirit = spiritStatuses[1];
            }else{
                selectableSpirit = spirit;
            }
            if (selectableSpirit != null){
                selectableSpirit.GetComponent<Button>().interactable = true;
            }
            ES.SetSelectedGameObject(selectableSpirit);
        }else{
            Debug.Log("no allies alive");
            //maybe give the message that the move cannot be used? and then go back to teh select attack screen?
        }
    }

    private void targetDouble(){
        toExculdeFromSelection = ES.GetComponent<TurnManager>().getActivePlayerControlled();
        toggles[2].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        toggles[3].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        GameObject button = GameObject.Find("SelectMultipleButton");
        button.GetComponent<Button>().interactable = true;
        ES.SetSelectedGameObject(button);
    }

    private void targetTeam(){
        toExculdeFromSelection = ES.GetComponent<TurnManager>().getActiveEnemies();
        toggles[0].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        toggles[1].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        GameObject button = GameObject.Find("SelectMultipleButton");
        button.GetComponent<Button>().interactable = true;
        ES.SetSelectedGameObject(button);
    }

    private void targetOthers(){
        toExculdeFromSelection.Add(Attacker);
        int exclude = findAttacker();
        if (exclude >= 0)
            for(int i = 0; i < 4; i++)
                if (i != exclude)
                    toggles[i].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        GameObject button = GameObject.Find("SelectMultipleButton");
        button.GetComponent<Button>().interactable = true;
        ES.SetSelectedGameObject(button);
    }

    private void targetAll(){
        //the exlcude list should already be empty (just in case)
        toExculdeFromSelection.Clear();
        for (int i = 0; i < 4; i++)
            toggles[i].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        GameObject button = GameObject.Find("SelectMultipleButton");
        button.GetComponent<Button>().interactable = true;
        ES.SetSelectedGameObject(button);
    }

    /*	---------------------------
	*	Utility functions
	*	---------------------------	*/

    private void EnemyAI () {
        CreatureBattleStatusController Defender;
        // AI part //
        /* AI needs to:
            - decide whether to attack or defend or use an item (for wild spirits they shouldnt be able to use items)
            - choose an attack
            - choose a target (should be the opposite target choices as the player)
        */
        int todo = Random.Range(0,3); 
        //0=attack, 1=defend, 2=item, 3=switch(though probably onto go through with this if certain conditions are met)

        if (todo == 0) {
            //this should choose a random move from the enemy's move set (even if they have less than 4 moves)
            int randNum = Random.Range(0, Attacker.GetCreature().moveNames.Count);
            moveUsed = Attacker.GetCreature().moveNames[randNum];
            SelectedMove = MovesTable.Find(moveUsed);

            switch (SelectedMove.AttackTarget) {
                case Move.Target.All:
                    Defenders = ES.GetComponent<TurnManager>().getAllActive();
                    DoAttack();
                    break;
                case Move.Target.Ally:
                    Defenders = ES.GetComponent<TurnManager>().getActiveEnemies();
                    Defenders.Remove(Attacker);
                    DoAttack();
                    break;
                case Move.Target.Double:
                    Defenders = ES.GetComponent<TurnManager>().getActivePlayerControlled();
                    DoAttack();
                    break;
                case Move.Target.Others:
                    Defenders = ES.GetComponent<TurnManager>().getAllActive();
                    Defenders.Remove(Attacker);
                    DoAttack();
                    break;
                case Move.Target.Self:
                    Defenders.Add(Attacker);
                    DoAttack();
                    break;
                case Move.Target.Single:
                    List<CreatureBattleStatusController> toChoose = ES.GetComponent<TurnManager>().getActivePlayerControlled();
                    int randNum2 = Random.Range(0, toChoose.Count);
                    Defender = toChoose[randNum2];
                    Defenders.Add(Defender);
                    DoAttack();
                    break;
                case Move.Target.Team:
                    Defenders = ES.GetComponent<TurnManager>().getActivePlayerControlled();
                    DoAttack();
                    break;
            }
        
        } else if (todo == 1) {
            LoadDefend();
        } else if (todo == 2) {
            ShowMessage("" + Attacker.GetCreature().displayName + " used an [ITEM] (WIP)");
            LoadProgress();
        } else if (todo == 3) {
            ShowMessage("" + Attacker.GetCreature().displayName + " switched (WIP)");
            LoadProgress();
        }
    }

    private void EndBattle() {
        //load the world again (probably also want to save the scene view for the route the player was last in)
        SceneManager.UnloadSceneAsync("Battle_GUI");
        SceneManager.sceneUnloaded += ReenableOES;
    }

    private void ReenableOES(Scene scene) {
        OES.SetActive(true);
        Time.timeScale = 1;
        GameObject.Find("WalkableCharacter").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().enabled = false;
        SceneManager.sceneUnloaded -= ReenableOES;
    }
}
