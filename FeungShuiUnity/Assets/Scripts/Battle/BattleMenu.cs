using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class BattleMenu : MonoBehaviour{
    public EventSystem ES;
    public GameObject Message, Moves;
    private GameObject Selected, OES;
    private bool IsSelectingAttack;
    private Move SelectedMove;
    private CreatureBattleStatusController Attacker;
    private List<CreatureBattleStatusController> Defenders; //just to support having several targets
    private List<CreatureBattleStatusController> toExculdeFromSelection;
    public GameObject[] spiritStatuses, actionButtons, attackButtons, toggles;
    public GameObject ProgressButton;
    public Interaction interaction;
    private bool playerWon;
    public Queue<Action> messageBoxActions;
    Action currentAction;

    #region General

    void Start(){
        //Disable overworld's event system and ui(since the battle window is just an overlay)
        OES = GameObject.Find("EventSystem");
        OES.SetActive(false);
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().enabled = false;
        //Initialize the turn order
        ES.GetComponent<TurnManager>().Init();
        //Set up navigation control variables
        Selected = ES.firstSelectedGameObject;
        IsSelectingAttack = false;
        SelectedMove = null;
        //Initialize lists and queue
        Defenders = new List<CreatureBattleStatusController> { };
        toExculdeFromSelection = new List<CreatureBattleStatusController>() { };
        messageBoxActions = new Queue<Action>(){ };
        currentAction = null;
        //Setup done, begin the battle
        BeginTurn();
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
        //Run a pending action, if any
        advanceActions();
    }

    #endregion

    #region Cycle controls

    //Run the next action in the queue
    public void advanceActions(){
        if (currentAction == null && messageBoxActions.Count > 0){
            currentAction = messageBoxActions.Dequeue();
            currentAction();
            Debug.Log("Performed an action! " + messageBoxActions.Count + " left!");
        }
    }

    //Clear the current action to mark that the next one should run next Update()
    //Called by the advance arrow on the text box
    public void finishAction(){
        currentAction = null;
    }

    //Called when a message is to be displayed in the bottom right box
    private void ShowMessage(string msg){
        //Turn on the message box and set the text
        Message.SetActive(true);
        Message.GetComponent<Text>().text = msg;
        //Disable attack buttons
        Moves.SetActive(false);
        IsSelectingAttack = false;
        //Turn on the advance arrow
        ProgressButton.GetComponent<Button>().interactable = true;
        ES.SetSelectedGameObject(ProgressButton);
    }

    //Processing performed at the start of a turn before player/ai input
    public void BeginTurn(){
        if (ES.GetComponent<TurnManager>().whoWins().Equals("Player")){
            Debug.Log("Player Wins");
            playerWon = true;
            EndBattle();
        } else if (ES.GetComponent<TurnManager>().whoWins().Equals("Computer")){
            Debug.Log("Player Looses");
            playerWon = false;
            EndBattle();
        } else if (ES.GetComponent<TurnManager>().whoWins().Equals("No-one")){
            Attacker = ES.GetComponent<TurnManager>().getNextSpirit();
            Attacker.relieveDefenseMove(); //get rid of the defense move on their next turn 
            Debug.Log(Attacker.Target.name + "'s turn beginning.");
            //Status effects that take place at the START of the turn go here
            float randNum = UnityEngine.Random.Range(0.0f, 1.0f);
            float FrostBiteChance = 0.5f;
            float ParalysisChance = 0.5f;
            float RabidChance = 0.5f;
            bool StatusHappened = false;
            switch (Attacker.statusEffect){
                case StatusEffect.FrostBite:
                    //chance of no action
                    if (randNum <= FrostBiteChance){
                        //then no action occurs
                        StatusHappened = true;
                        messageBoxActions.Enqueue(() => ShowMessage(Attacker.Target.name + " did not take an action due to Frost Bite."));
                    }
                    break;
                case StatusEffect.Paralysis:
                    //chance of no action
                    if (randNum <= ParalysisChance){
                        //then no action occurs
                        StatusHappened = true;
                        messageBoxActions.Enqueue(() => ShowMessage(Attacker.Target.name + " did not take an action due to Paralysis."));
                        messageBoxActions.Enqueue(() => EndTurn());
                    }
                    break;
                case StatusEffect.Rabid:
                    //chance of random action
                    if (randNum <= RabidChance){
                        //then a random action occurs
                        StatusHappened = true;
                        RandomAction();
                    }
                    break;
                default:
                    if (!StatusHappened){
                        AskForAction();
                    }
                    break;
            }
        }
    }

    //Select action type (Attack/Defend/Item/Run)
    private void AskForAction(){
        //End last action
        finishAction();
        if (Attacker.GetCreature().isPlayerOwned()){
            ShowMessage("What will " + Attacker.GetCreature().name + " do?");
            //Enable action type buttons
            for (int i = 0; i < 5; i++) actionButtons[i].GetComponent<Button>().interactable = true;
            ProgressButton.GetComponent<Button>().interactable = false;
            ES.SetSelectedGameObject(GameObject.Find("Attack"));
        } else {
            EnemyAI();
        }
    }

    private void RandomAction(){
        //here the eidolon will choose a random move
        int randNum = UnityEngine.Random.Range(1, 6 + 1); //the +1 since Random.Range(int min, int max) is exclusive on max
        if (randNum <= 4){
            //then choose a move with index [randNum]
            Move move = Attacker.Target.Moves[randNum];
            TurnManager turnManager = ES.GetComponent<TurnManager>();
            //for now the targets will conform to what the moves would be normally
                /*
                * but maybe this could be simplified down becuase the ediolon is rabid and may not have cohesive thoughts 
                */
            switch (move.AttackTarget){
                case Move.Target.All:
                    Defenders = turnManager.getAllActive();
                    DoAttack();
                    break;
                case Move.Target.Ally:
                    if (Attacker.Target.isPlayerOwned()){
                        Defenders = turnManager.getActivePlayerControlled();
                    } else {
                        Defenders = turnManager.getActiveEnemies();
                    }
                    Defenders.Remove(Attacker);
                    DoAttack();
                    break;
                case Move.Target.Double:
                    if (Attacker.Target.isPlayerOwned()){
                        Defenders = turnManager.getActiveEnemies();
                    } else {
                        Defenders = turnManager.getActivePlayerControlled();
                    }
                    DoAttack();
                    break;
                case Move.Target.Others:
                    Defenders = turnManager.getAllActive();
                    Defenders.Remove(Attacker);
                    DoAttack();
                    break;
                case Move.Target.Self:
                    Defenders.Clear();
                    Defenders.Add(Attacker);
                    DoAttack();
                    break;
                case Move.Target.Single:
                    List<CreatureBattleStatusController> toChoose = turnManager.getAllActive();
                    int randNum2 = UnityEngine.Random.Range(0, toChoose.Count);
                    Defenders.Clear();
                    Defenders.Add(toChoose[randNum2]);
                    DoAttack();
                    break;
                case Move.Target.Team:
                    if (Attacker.Target.isPlayerOwned()){
                        Defenders = turnManager.getActivePlayerControlled();
                    } else {
                        Defenders = turnManager.getActiveEnemies();
                    }
                    DoAttack();
                    break;
            }
        } else if (randNum == 5){
            //defend action
            SelectDefend();
        } else if (randNum == 6){
            //hurt itself
            float damageToTake = (1 / 16) * Attacker.Target.maxActiveHealth;
            Attacker.ApplyDamage(damageToTake, Attacker);
            ShowMessage(Attacker.Target.name + " hurt itself in its rage.");
            EndTurn();
        }
    }

    //Processing performed after a turn
    private void EndTurn(){
        //END of turn status effects go here, start of turn status effects go in AskForAction
        switch (Attacker.statusEffect){
            case StatusEffect.Burn:
                //hp drain (END)
                //lowers attack
                //does not wear off
                HPDrain();
                break;
            case StatusEffect.FrostBite:
                //chance of no action (START)
                //hp drain (END)
                //does not wear off
                HPDrain();
                break;
            case StatusEffect.Paralysis:
                //chance of no action (START)
                //speed down
                //does not wear off
                break;
            case StatusEffect.Poison:
                //hp drain (END)
                //mana drain (END)
                //does not wear off
                HPDrain();
                ManaDrain();
                break;
            case StatusEffect.Rabid:
                //chance of random action (START)
                //attack up
                //does wear off
                checkWearOff();
                break;
        }

        //then progress the turn cycle as normal
        ProgressButton.GetComponent<Button>().interactable = true;
        ES.SetSelectedGameObject(ProgressButton);
        //Start next turn
        BeginTurn();
    }

    #endregion

    #region Main menu buttons

    //Called when the Attack option is pressed
    public void SelectAttack(){
        Message.SetActive(false);                       //Disable message box
        Moves.SetActive(true);                          //Put moves buttons in its place
        IsSelectingAttack = true;                       //Keep track of our location in the menu
        ES.SetSelectedGameObject(attackButtons[0]);     //Set the first (upper left) attack as the currently highlighed button
        //Goes through the list of moves of a creature and displays them in the rightmost text as selectable buttons
        int i = 0;
        while (i < Attacker.GetCreature().Moves.Count){
            attackButtons[i].GetComponentInChildren<Text>().text = Attacker.GetCreature().Moves[i].ToString();
            attackButtons[i].GetComponent<Button>().interactable = true;
            i++;
        }
        //Fills out the remainders with empty text and makes them not selectable
        while (i < 4){
            attackButtons[i].GetComponentInChildren<Text>().text = "";
            attackButtons[i].GetComponent<Button>().interactable = false;
            i++;
        }
        //Disable action type buttons
        for (i = 0; i < 5; i++) actionButtons[i].GetComponent<Button>().interactable = false;
    }

    public void SelectDefend(){
        //Notify controller that it is defending
        Attacker.doDefenseMove();
        //Show a message that the spririt defended
        messageBoxActions.Enqueue(() => ShowMessage(Attacker.GetCreature().name + " defended!"));
        //Progress the turn cycle
        messageBoxActions.Enqueue(() => EndTurn());
    }

    public void SelectItem(){
        //Show a message that this is not implemented yet
        messageBoxActions.Enqueue(() => ShowMessage(Attacker.GetCreature().name + " used an [ITEM] (WIP)"));
        //Progress the turn cycle
        messageBoxActions.Enqueue(() => EndTurn());
    }
    
    public void SelectSpirits(){
        //Show a message that this is not implemented yet
        messageBoxActions.Enqueue(() => ShowMessage(Attacker.GetCreature().name + " looked at [SPIRITS] (WIP)"));
        //Progress the turn cycle
        messageBoxActions.Enqueue(() => AskForAction());
    }

    public void Run(){
        //TODO: Add code to decide whether running is possible
        playerWon = false;
        EndBattle();
    }

    #endregion

    #region Attacking

    //Called when a move is selected
    public void LoadAttack(int index){
        SelectedMove = Attacker.Target.Moves[index];
        Debug.Log("target" + SelectedMove.AttackTarget.ToString());
        GetType().GetMethod("target" + SelectedMove.AttackTarget.ToString()).Invoke(this, null);
        for (int i = 0; i < 4; i++) attackButtons[i].GetComponent<Button>().interactable = false;
    }

    //Get the index of the CreatureBattleStatusController connected to the attacker
    private int findAttacker(){
        for (int i = 0; i <= 1; i++){
            GameObject spirit = spiritStatuses[i];
            if (spirit.activeSelf && spirit.GetComponent<CreatureBattleStatusController>().Target == Attacker.GetCreature()){
                return i;
            }
        }
        return -1;
    }

    //Called by defender when they're selected telling script to target them
    public void LoadDefender(){
        Defenders.Add(Selected.GetComponent<CreatureBattleStatusController>());
        //disable the spirit buttons
        for (int i = 0; i <= 3; i++){
            GameObject spirit = spiritStatuses[i];
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

    //Called when the oppoenent is selected
    public void DoAttack(){
        foreach (CreatureBattleStatusController Defender in Defenders){
            string message;
            if (SelectedMove.execute(Attacker, Defender)){
                //attack hit
                message = Attacker.GetCreature().name + " used " + SelectedMove.name + " on " + Defender.GetCreature().name + ".";
            } else {
                //attack missed
                message = SelectedMove.name + " missed" + Defender.GetCreature().name + ".";
            }
            messageBoxActions.Enqueue(() => ShowMessage(message));
        }
        resetSelection();

        messageBoxActions.Enqueue(() => EndTurn());
    }

    //Clear everything related to attacking
    private void resetSelection(){
        Defenders.Clear();
        toExculdeFromSelection.Clear();
        SelectedMove = null;

        for (int i = 0; i < 4; i++){
            toggles[i].GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        }
    }

    #endregion

    #region Targeting
    
    //Utility method to avoid repeating a few lines of code
    private void enableTarget(int id){
        GameObject spirit = spiritStatuses[id];
        if (spirit!=null && spirit.activeSelf){
            spirit.GetComponent<Button>().interactable = true;
            ES.SetSelectedGameObject(spirit);
        }
    }

    public void targetSingle(){
        enableTarget(2);
        enableTarget(3);
    }

    public void targetSelf(){
        enableTarget(findAttacker());
    }

    public void targetAlly(){
        if (ES.GetComponent<TurnManager>().getActivePlayerControlled().Count == 2){ //Only possible when 2 player-controlled spirits are alive
            int selectableSpirit = 0;
            GameObject spirit = spiritStatuses[0];
            if (spirit.GetComponent<CreatureBattleStatusController>().Target == Attacker.GetCreature()){
                //change it to be the other player controlled
                selectableSpirit = 1;
            }
            enableTarget(selectableSpirit);
        } else {
            //Show an error to the player
            messageBoxActions.Enqueue(() => ShowMessage("There is no ally to target."));
            //Try again
            messageBoxActions.Enqueue(() => AskForAction());
        }
    }

    public void targetDouble(){
        toExculdeFromSelection = ES.GetComponent<TurnManager>().getActivePlayerControlled();
        toggles[2].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        toggles[3].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        GameObject button = GameObject.Find("SelectMultipleButton");
        button.GetComponent<Button>().interactable = true;
        ES.SetSelectedGameObject(button);
    }

    public void targetTeam() {
        toExculdeFromSelection = ES.GetComponent<TurnManager>().getActiveEnemies();
        toggles[0].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        toggles[1].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        GameObject button = GameObject.Find("SelectMultipleButton");
        button.GetComponent<Button>().interactable = true;
        ES.SetSelectedGameObject(button);
    }

    public void targetOthers(){
        toExculdeFromSelection.Add(Attacker);
        int exclude = findAttacker();
        if (exclude >= 0)
            for (int i = 0; i < 4; i++)
                if (i != exclude)
                    toggles[i].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        GameObject button = GameObject.Find("SelectMultipleButton");
        button.GetComponent<Button>().interactable = true;
        ES.SetSelectedGameObject(button);
    }

    private void targetAll(){
        //the exlcude list should already be empty (just in case)
        toExculdeFromSelection.Clear();
        for (int i = 0; i < 4; i++) toggles[i].GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        GameObject button = GameObject.Find("SelectMultipleButton");
        button.GetComponent<Button>().interactable = true;
        ES.SetSelectedGameObject(button);
    }

    #endregion

    #region Utility

    //Randomly generate actions the opponent is going to perform
    //Todo: allow different types of AIs with different levels of difficulty
    private void EnemyAI () {
        CreatureBattleStatusController Defender;
        // AI part //
        /* AI needs to:
            - decide whether to attack or defend or use an item (for wild spirits they shouldnt be able to use items)
            - choose an attack
            - choose a target (should be the opposite target choices as the player)
        */
        int todo = UnityEngine.Random.Range(0, 3);
        //0=attack, 1=defend, 2=item, 3=switch(though probably onto go through with this if certain conditions are met)

        if (todo == 0) {
            //this should choose a random move from the enemy's move set (even if they have less than 4 moves)
            int randNum = UnityEngine.Random.Range(0, Attacker.GetCreature().Moves.Count);
            SelectedMove = Attacker.GetCreature().Moves[randNum];

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
                    int randNum2 = UnityEngine.Random.Range(0, toChoose.Count);
                    Defender = toChoose[randNum2];
                    Defenders.Add(Defender);
                    DoAttack();
                    break;
                case Move.Target.Team:
                    Defenders = ES.GetComponent<TurnManager>().getActivePlayerControlled();
                    DoAttack();
                    break;
            }
        } else if (todo == 1){
            messageBoxActions.Enqueue(() => ShowMessage(Attacker.GetCreature().name + " defended!"));
            messageBoxActions.Enqueue(() => EndTurn());
        } else if (todo == 2){
            messageBoxActions.Enqueue(() => ShowMessage("" + Attacker.GetCreature().name + " used an [ITEM] (WIP)"));
            messageBoxActions.Enqueue(() => EndTurn());
        } else if (todo == 3){
            messageBoxActions.Enqueue(() => ShowMessage("" + Attacker.GetCreature().name + " switched (WIP)"));
            messageBoxActions.Enqueue(() => EndTurn());
        }
    }

    private void EndBattle(){
        SceneManager.UnloadSceneAsync("Battle_GUI");
        //Enable overworld's event system and UI
        OES.SetActive(true);
        Time.timeScale = 1;
        GameObject.Find("WalkableCharacter").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().enabled = true;
        //Continue interaction, if any (there probably is one though).
        if (interaction != null){
            interaction.GetComponent<Battle>().defeated = playerWon;    //Update the status of the battle
            //Find the first active battle node to signal the result of the battle. Assumes only one battle will be active at once.
            foreach (InteractionNode node in interaction.graph.activeNodes){
                if (typeof(BattleNode).IsInstanceOfType(node)){
                    ((BattleNode)node).Finish(playerWon, interaction.gameObject);
                    break;
                }
            }
            interaction = null;     //There is no longer an interaction in control of the BattleMenu
        }
    }

    private void HPDrain(){
        //a helper method for the status effects
        float reducingFactor = 1 / 16; // 1/16 is what pokemon uses for burn/poison
        Attacker.ApplyDamage(reducingFactor * Attacker.Target.maxActiveHealth, Attacker);
        ShowMessage("drained hp from " + Attacker.Target.name + ".");
    }

    private void ManaDrain(){
        //helper for the status effects
        float reducingFactor = 1 / 16;
        Attacker.Target.currentMana = Mathf.Max(0, Attacker.Target.currentMana - reducingFactor * Attacker.Target.maxMana);
        ShowMessage("drained mana from " + Attacker.Target.name + ".");
    }

    private void checkWearOff(){
        //another helper method for the status effects
        float wearOffChance = 0.4f; //hardcoded 40% chance of wearing off every turn
        float randNum = UnityEngine.Random.Range(0.0f, 1.0f);
        if (randNum <= wearOffChance){
            //then the status will wear off
            ShowMessage(Attacker.statusEffect.ToString() + " wore off.");
            Attacker.statusEffect = StatusEffect.None;
            Attacker.RestoreStatFromStatus();
        }
    }

    #endregion
    
}
