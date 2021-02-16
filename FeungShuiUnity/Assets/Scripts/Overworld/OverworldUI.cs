using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class OverworldUI : Menu {
    [SerializeField]
    private GameObject Player, Message, MessageText, Arrow, AnswerBox;
    private GameObject dialogueContext;
    
    private InteractionNode activeNode;

    public void Update() {
        //No need to update a paused scene
        if (paused) return;

        if (Input.GetButtonDown("Cancel") && GameObject.Find("InWorldMessage")==null) {
            OpenNewMenu("OverworldMenu");
        }
    }

    #region Interactions
    public void disableButton() {
        Message.GetComponent<Button>().interactable = false;
        Message.SetActive(false);
        Player.GetComponent<Walk>().canWalk = true;
        activeNode = null;
    }

    public void disableInteract() {
        Player.transform.GetChild(0).gameObject.SetActive(false); //gets the first object that is the players child (in this case the interact area)
    }

    public void scheduleReenable() {
        Invoke("enableInteract", 0.0167f);
    }

    private void enableInteract() {
        Player.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ShowMessage(string msg, bool useArrow) {
        disableInteract();
        Player.GetComponent<Walk>().canWalk = false;
        Message.SetActive(true);
        Message.GetComponent<Button>().interactable = useArrow;
        Arrow.SetActive(useArrow);
        gameObject.GetComponent<EventSystem>().SetSelectedGameObject(Message);
        MessageText.GetComponent<Text>().text = msg;
    }

    public void SetActiveNode(InteractionNode target) {
        activeNode = target;
    }

    public void SetDialogueContext(GameObject target) {
        dialogueContext = target;
    }

    public void AdvanceMessage() {
        activeNode.ExecuteNext(activeNode.GetOutputPort("next"), dialogueContext);
        //if (!activeNode.RunStep())
        //    disableButton();
    }

    public void ShowAnswers(string[] options) {
        //Build an array of Actions
        Action[] actions = new Action[options.Length];
        for (int i = 0; i < options.Length; i++) {
            actions[i] = () => RunAnswer();
        }
        AnswerBox.GetComponent<OptionBox>().Populate(options, actions);
    }

    public void RunAnswer() {
        activeNode.ExecuteNext(activeNode.GetOutputPort("answers " + AnswerBox.GetComponent<OptionBox>().chosen), dialogueContext);
    }
    #endregion

    #region Menu interactions
    public override void Pause() {
        base.Pause();
        Message.SetActive(false);
        Player.GetComponent<Walk>().canWalk = false;
        disableInteract();
        Time.timeScale = 0;
        paused = true;
    }

    public override void Resume() {
        base.Resume();
        Time.timeScale = 1;
        if(activeNode == null) {
            Player.GetComponent<Walk>().canWalk = true;
            scheduleReenable();
        }
    }

    public void FinishBattle(bool result) {
        Resume();
        //Continue the interaction
        //There should never be a situation where this is not a valid cast
        if (activeNode != null)
            ((BattleNode)activeNode).Finish(result);
    }

    public void CloseShop() {
        Resume();
        //Continue the interaction
        //There should never be a situation where this is not a valid cast
        ((OpenShopNode)activeNode).Finish();
    }
    #endregion
}