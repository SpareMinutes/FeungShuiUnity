using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuAndWorldUI : MonoBehaviour{
    public EventSystem ES;
    [SerializeField]
    private GameObject Player, Canvas, Message, Text, Menu, Bag, Party, Arrow, AnswerBox;
    private InteractionNode activeNode;
    [SerializeField]
    private GameObject[] Answers, AnswerBG;
    private bool isMenuOpen, isBagOpen, isPartyOpen = false;
    private GameObject SelectedMenu, SelectedMessage, SelectedBag, SelectedParty, dialogueContext;

    private List<BagTab> bagTabs;
    private int currentTabIndex = 0;
    private int shownAnswers = 0;
    private List<Item> currentItems;
    private int offset = -2;

    public void Start(){
        SelectedMenu = Menu.transform.GetChild(0).gameObject;
        SelectedMessage = Message;
        SelectedBag = Bag.transform.GetChild(1).GetChild(2).gameObject; //should select the item button if hte layering is correct
        SelectedParty = Party.transform.GetChild(1).gameObject;
    }

    public void Update (){
        //Ensures clicking doesn't deselect buttons
        GameObject selected = ES.currentSelectedGameObject;
        
        if (selected == null) {
            if (isMenuOpen) {
                //menu is open 
                ES.SetSelectedGameObject(SelectedMenu);
            } else if (Message.activeSelf) {
                //message is open
                ES.SetSelectedGameObject(SelectedMessage);
            } else if (Bag.activeSelf) {
                //the bag is open
                ES.SetSelectedGameObject(SelectedBag);
            }
            //else nothing is open
        }
        if (shownAnswers > 0 && !Answers.Contains(selected)) {
            //answers are shown
            ES.SetSelectedGameObject(Answers[9]);
        }

        if (Input.GetButtonDown("Cancel")) {
            if (isBagOpen) {
                CloseBag();
            } else if (isPartyOpen) {
                CloseParty();
            } else if (isMenuOpen) {
                //menu is open so close it
                CloseMenu();
                Player.GetComponent<Walk>().canWalk = true; //this is so that you can open different menus from the escape menu
            } else if(!Message.activeSelf){
                OpenMenu();
            }
        } 

        if (isBagOpen) {
            //this handles the left/right arrows for bag tab changes
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                //then we want change the tab to the left
                currentTabIndex--;
                offset = -2;
                UpdateItemList();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                //then we want to change tab to the right
                currentTabIndex++;
                offset = -2;
                UpdateItemList();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                //offset = Mathf.Clamp(offset++, -2, currentItems.Count + 2);
                offset --;
                if (offset < -2) {
                    offset = -2;
                } else if (offset > currentItems.Count -2) {
                    offset = currentItems.Count - 2;
                }
                //else the offset is fine
                UpdateItemList();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                //offset = Mathf.Clamp(offset--, -2, currentItems.Count + 2);
                offset ++;
                if (offset < -2) {
                    offset = -2;
                } else if (offset > currentItems.Count - 3) {
                    offset = currentItems.Count - 3;
                }
                //else the offset is fine
                UpdateItemList();
            }
        }
    }

    public void disableButton(){
        Message.GetComponent<Button>().interactable = false;
        Message.SetActive(false);
        Player.GetComponent<Walk>().canWalk = true;
        activeNode = null;
        Invoke("reActivateInteract", 0.0167f);
    }

    private void reActivateInteract () {
        Player.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ShowMessage(string msg, bool useArrow){
        Player.transform.GetChild(0).gameObject.SetActive(false); //gets the first object that is the players child (in this case the interact area)
        Player.GetComponent<Walk>().canWalk = false;
        Message.SetActive(true);
        Message.GetComponent<Button>().interactable = useArrow;
        Arrow.SetActive(useArrow);
        ES.SetSelectedGameObject(Message);
        Text.GetComponent<Text>().text = msg;
    }

    public void SetActiveNode(InteractionNode target) {
        activeNode = target;
    }

    public void SetDialogueContext(GameObject target) {
        dialogueContext = target;
    }

    public void AdvanceMessage() {
        shownAnswers = 0;
        activeNode.ExecuteNext(activeNode.GetOutputPort("next"), dialogueContext);
        //if (!activeNode.RunStep())
        //    disableButton();
    }

    public void ShowAnswers(string[] options) {
        AnswerBox.SetActive(true);
        shownAnswers = options.Length;
        int offset = Answers.Length - options.Length;
        int maxLen = 0;
        //Fill in text. Also count length of longest option for use later
        for (int i=0; i<options.Length; i++) {
            Answers[offset + i].GetComponent<Text>().text = options[i];
            maxLen = Mathf.Max(maxLen, options[i].Length);
            Answers[offset + i].SetActive(true);
        }
        for(int i=0; i<offset; i++) {
            Answers[i].GetComponent<Text>().text = "";
            Answers[i].SetActive(false);
        }
        //Set box height
        for(int i=6; i<9; i++) {
            RectTransform rt = AnswerBG[i].GetComponent<RectTransform>();
            rt.localPosition = new Vector3(rt.localPosition.x, 10*options.Length-1, 0);
        }
        for (int i = 3; i < 6; i++) {
            RectTransform rt = AnswerBG[i].GetComponent<RectTransform>();
            rt.transform.localScale = new Vector3(rt.transform.localScale.x, (float)(2.5*options.Length-0.25), 1);
        }
        //Set box width
        for (int i = 2; i <= 8; i+=3) {
            RectTransform rt = AnswerBG[i].GetComponent<RectTransform>();
            rt.localPosition = new Vector3(6*maxLen, rt.localPosition.y, 0);
        }
        for (int i = 1; i <= 7; i+=3) {
            RectTransform rt = AnswerBG[i].GetComponent<RectTransform>();
            rt.transform.localScale = new Vector3((float)(1.5 * maxLen), rt.transform.localScale.y, 1);
        }
    }

    public void RunAnswer(int selection) {
        selection -= 10 - shownAnswers;
        AnswerBox.SetActive(false);
        shownAnswers = 0;
        activeNode.ExecuteNext(activeNode.GetOutputPort("answers " + selection), dialogueContext);
    }

    public void OpenMenu () {
        //doesnt do anything but this is where ill put the code to open the in game menu
        Menu.SetActive(true);
        isMenuOpen = true;
        ES.SetSelectedGameObject(null);
        ES.SetSelectedGameObject(SelectedMenu);
        Player.GetComponent<Walk>().canWalk = false;
    }

    public void CloseMenu () {
        //this is mainly for if theres anything else we want to put here
        Menu.SetActive(false);
        isMenuOpen = false;
        //Player.GetComponent<Walk>().canWalk = true;
    }

    public void OpenSummary () {
        //will be called when the summary button is pressed from the menu
        ShowMessage("This will show the summary of the adventure so far.", true);

    }

    public void OpenParty () {
        //will be called when the party option is selected fromt the menu
        //ShowMessage("This will show the spirit party.");
        isPartyOpen = true;
        Party.SetActive(true);
        ES.SetSelectedGameObject(null);
        ES.SetSelectedGameObject(SelectedParty);
        Player.GetComponent<Walk>().canWalk = false;
    }

    public void CloseParty () {
        Party.SetActive(false);
        isPartyOpen = false;
        Player.GetComponent<Walk>().canWalk = true;
    }

    public void OpenBag () {
        //opens the players bag when selected form the menu
        //ShowMessage("This will show the player inventory.");
        isBagOpen = true;
        Bag.SetActive(true);
        ES.SetSelectedGameObject(null);
        ES.SetSelectedGameObject(SelectedBag);
        Player.GetComponent<Walk>().canWalk = false;

        //set up bag here:
        Inventory inventory = Player.GetComponent<Inventory>();
        
        //bag tab
        bagTabs = new List<BagTab>(inventory.tabs.Keys);
        
        //item lineup
        UpdateItemList();
        
    }

    private void UpdateItemList () {
        int ItemIndex = 1; //for clarity in the the GetChild function
        //Debug.Log(offset);
        Inventory inventory = Player.GetComponent<Inventory>();
        currentItems = inventory.GetList(bagTabs[currentTabIndex%bagTabs.Count]);

        int tabTitleIndex = 2;
        Bag.transform.GetChild(tabTitleIndex).GetComponent<Text>().text = bagTabs[currentTabIndex%bagTabs.Count].ToString(); 

        for (int i = 0; i < 5; i++) {
            string item;
            if (i+offset < 0 || i+offset >= currentItems.Count) {
                //this is to account for the 2 above and below the actual item button
                item = "--";
            } else {
                item = currentItems[i + offset].displayName;
            }
            //then set the item text to the item
            Bag.transform.GetChild(ItemIndex).GetChild(i).GetComponentInChildren<Text>().text = item;
        }
    }

    public void UseItem () {
        //function for the item button
        currentItems[offset + 2].use();
        Debug.Log(currentItems[offset + 2].displayName);
    }

    public void CloseBag () {
        Bag.SetActive(false);
        isBagOpen = false;
        Player.GetComponent<Walk>().canWalk = true;
    }

    public void Save () {
        //will be called when the player selects save from the menu
        //for right now doesnt do anything
        ShowMessage("This will save the game.", true);
    }

    public void OpenOptions () {
        //opens the ingame options menu from the menu
        ShowMessage("This will show the options.", true);
    }
}
