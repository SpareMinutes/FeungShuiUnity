using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class MenuAndWorldUI : MonoBehaviour{
    public EventSystem ES;

    [SerializeField]
    private GameObject Player, Canvas, Message, Text, Menu, Bag, Arrow, AnswerBox, BuyInv, SellInv;

    private InteractionNode activeNode;
    public bool isMenuOpen, isBagOpen, isPartyOpen, isShopOpen, isBuying, selectAmount, amountCap = false;
    private GameObject SelectedMenu, SelectedMessage, SelectedBag, dialogueContext, SelectedShop, ShopConfirmation, context;

    private List<BagTab> bagTabs;
    private int currentTabIndex = 0;
    private List<Item> currentItems;
    private int offset = -2;

    private int itemNum = 1;

    public void Start(){
        SelectedMenu = Menu.transform.GetChild(0).gameObject;
        SelectedMessage = Message;
        SelectedBag = Bag.transform.GetChild(1).GetChild(2).gameObject; //should select the item button if hte layering is correct
    }

    public void Update (){
        //Ensures clicking doesn't deselect buttons
        GameObject selected = ES.currentSelectedGameObject;
        
        if (selected == null) {
            if (isMenuOpen) {
                //menu is open 
                ES.SetSelectedGameObject(SelectedMenu);
            } else if (AnswerBox.activeSelf) {
                //Do nothing, but exit the if/else anyway to avoid the next option
            } else if (Message.activeSelf) {
                //message is open
                ES.SetSelectedGameObject(SelectedMessage);
            } else if (Bag.activeSelf) {
                //the bag is open
                ES.SetSelectedGameObject(SelectedBag);
            }
            //else nothing is open
        }

        if (Input.GetButtonDown("Cancel")) {
            if (isBagOpen) {
                CloseBag();
                OpenMenu();
            } else if (isPartyOpen) {
                CloseParty();
                OpenMenu();
            } else if (activeNode is BattleNode) {
                //do nothing
            } else if (isMenuOpen) {
                //menu is open so close it
                CloseMenu(true);
                Player.GetComponent<Walk>().canWalk = true; //this is so that you can open different menus from the escape menu
            } else if (isShopOpen) {
                if (selectAmount) {
                    //exit back to shop UI
                    if (isBuying) {
                        BuyInv.transform.GetChild(3).gameObject.SetActive(false); //close amount select
                    } else {
                        SellInv.transform.GetChild(3).gameObject.SetActive(false);
                    }
                    selectAmount = false;
                    ES.SetSelectedGameObject(null);
                    ES.SetSelectedGameObject(SelectedShop);
                } else {
                    CloseShop();
                }
            } else if (!Message.activeSelf) {
                OpenMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            //reset the offset to go back to the top of the item list
            offset = -2;
        }

        if (isBagOpen) {
            //this handles the left/right arrows for bag tab changes
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                //then we want change the tab to the left
                if (currentTabIndex - 1 < 0) {
                    currentTabIndex = bagTabs.Count - 1;
                } else {
                    currentTabIndex--;
                }
                
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
        }   //end of Bag if statement

        else if (isShopOpen) {
            //shop controls go here
            if (Input.GetKeyDown(KeyCode.UpArrow) && !selectAmount) {
                offset --;
                if (offset < -2) {
                    offset = -2;
                } else if (offset > currentItems.Count -2) {
                    offset = currentItems.Count - 2;
                }
                //else the offset is fine

                UpdateShopItemList();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && !selectAmount) {
                offset ++;
                if (offset < -2) {
                    offset = -2;
                } else if (offset > currentItems.Count - 3) {
                    offset = currentItems.Count - 3;
                }
                //else the offset is fine

                UpdateShopItemList();
            }

            if (selectAmount) {
                //up/down will increase/decrease the amount of items
                Inventory buyer;
                Inventory seller;
                if (isBuying) {
                    buyer = Player.GetComponent<Inventory>();
                    seller = context.GetComponent<Inventory>();
                } else {
                    buyer = context.GetComponent<Inventory>();
                    seller = Player.GetComponent<Inventory>();
                }
                Item item = currentItems[offset + 2];
                int buyerMoney = buyer.money;
                int maxAmountToBuy = (buyerMoney - buyerMoney % item.cost) / item.cost;
                int maxAmount = Mathf.Min(seller.itemDict[item], maxAmountToBuy);
                if (maxAmount == 0) {
                    //cant buy any of the item
                    itemNum = 0;
                }

                if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    if (itemNum + 1 <= maxAmount) {
                        itemNum ++;
                    } else if (itemNum == maxAmount) {
                        if (maxAmount != 0) {
                            itemNum = 1; //loop around
                        }
                        //else itemNum should remain at 0
                    }
                    
                } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                    if (itemNum - 1 == 0) {
                        itemNum = maxAmount; //loop around
                    } else if (itemNum > 0){
                        itemNum --;
                    }
                }

                if (isBuying) {
                    BuyInv.transform.GetChild(3).GetChild(1).GetComponentInChildren<Text>().text = itemNum.ToString();
                } else {
                    SellInv.transform.GetChild(3).GetChild(1).GetComponentInChildren<Text>().text = itemNum.ToString();
                }
            }
        } //end of shop if statement
        

    }

    public void SetContext(GameObject target) {
        context = target;
    }

    #region Interactions
    public void disableButton(){
        Message.GetComponent<Button>().interactable = false;
        Message.SetActive(false);
        Player.GetComponent<Walk>().canWalk = true;
        activeNode = null;
    }

    public void disableInteract() {
        Player.transform.GetChild(0).gameObject.SetActive(false); //gets the first object that is the players child (in this case the interact area)
    }

    public void sceduleReenable() {
        Invoke("enableInteract", 0.0167f);
    }

    private void enableInteract () {
        Player.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ShowMessage(string msg, bool useArrow){
        disableInteract();
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
        AnswerBox.SetActive(true);
    }

    public void RunAnswer() {
        activeNode.ExecuteNext(activeNode.GetOutputPort("answers " + AnswerBox.GetComponent<OptionBox>().chosen), dialogueContext);
    }
    #endregion

    #region Menu
    public void OpenMenu () {
        disableInteract();
        Menu.SetActive(true);
        isMenuOpen = true;
        ES.SetSelectedGameObject(null);
        ES.SetSelectedGameObject(SelectedMenu);
        Player.GetComponent<Walk>().canWalk = false;
        GameObject.Find("WalkableCharacter").transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    //Bool determines whether to reenable movement and interactions
    //false when called by opening a submenu
    //true when called by fully closing the menu
    public void CloseMenu (bool fullyClosed) {
        Menu.SetActive(false);
        isMenuOpen = false;
        if (fullyClosed) {
            Player.GetComponent<Walk>().canWalk = true;
            sceduleReenable();
            Time.timeScale = 1;
        }
    }


    public void OpenSummary () {
        //will be called when the summary button is pressed from the menu
        //ShowMessage("This will show the summary of the adventure so far.", true);
        Debug.Log("This will show the summary of the adventure so far.");
    }


    public void OpenParty () {
        //will be called when the party option is selected fromt the menu
        isPartyOpen = true;
        ES.SetSelectedGameObject(null);
        //ES.SetSelectedGameObject(SelectedParty);
        OpenSubscene("PartyScreen");
    }

    public void CloseParty () {
        isPartyOpen = false;
        SceneManager.UnloadSceneAsync("PartyScreen");
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
        inventory.OrganiseBagTabs();
        bagTabs = new List<BagTab>(inventory.tabs.Keys);
        
        //item lineup
        UpdateItemList();
        
    }

    private void UpdateItemList () {

        Inventory inventory = Player.GetComponent<Inventory>();
        currentItems = inventory.GetList(bagTabs[currentTabIndex%bagTabs.Count]);

        int tabTitleIndex = 2;
        Bag.transform.GetChild(tabTitleIndex).GetComponent<Text>().text = bagTabs[currentTabIndex%bagTabs.Count].ToString(); 

        for (int i = 0; i < 5; i++) {
            string itemName;
            string itemNum;
            if (i+offset < 0 || i+offset >= currentItems.Count) {
                //this is to account for the 2 above and below the actual item button
                itemName = "--";
                itemNum = "-";
            } else {
                itemName = currentItems[i + offset].name;
                itemNum = inventory.itemDict[currentItems[i + offset]].ToString();
            }
            //then set the item text to the item
            Bag.transform.GetChild(1).GetChild(i).GetComponentInChildren<Text>().text = itemName;
            Bag.transform.GetChild(5).GetChild(i).GetComponentInChildren<Text>().text = itemNum;
        }
        //reset incase the tab has no items in it
        Bag.transform.GetChild(6).GetComponent<Text>().text = "";
        Bag.transform.GetChild(4).GetComponent<Image>().sprite = null;

        //to stop throwing exceptions
        if (currentItems.Count > 0) {
            //item description in bag
            Bag.transform.GetChild(6).GetComponent<Text>().text = currentItems[offset + 2].description;
            //item image
            Bag.transform.GetChild(4).GetComponent<Image>().sprite = currentItems[offset + 2].itemImage;
        }
        

    }

    public void UseItem () {
        //function for the item button
        currentItems[offset + 2].use();
        Debug.Log(currentItems[offset + 2].name);
    }

    public void CloseBag () {
        Bag.SetActive(false);
        isBagOpen = false;
        Player.GetComponent<Walk>().canWalk = true;
    }


    public void Save () {
        //will be called when the player selects save from the menu
        //for right now doesnt do anything
        Debug.Log("This will save the game.");
    }
    

    public void OpenOptions () {
        //opens the ingame options menu from the menu
        Debug.Log("This will show the options.");
    }


    public void OpenSubscene(string name) {
        Player.GetComponent<Walk>().canWalk = false;
        GameObject.Find("WalkableCharacter").transform.GetChild(0).gameObject.SetActive(false);
        SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
    }
    #endregion



    #region Shops
    private void OpenShop () {
        //opens the shop UI
        isShopOpen = true;
        ES.SetSelectedGameObject(null);
        ES.SetSelectedGameObject(SelectedShop);
        Player.GetComponent<Walk>().canWalk = false;

        
        UpdateShopItemList();
        
    }

    public void OpenSellInv (Inventory shopInventory) {
        //for the player to sell their items to a shop keeper
        isBuying = false;
        SellInv.SetActive(true);
        SelectedShop = SellInv.transform.GetChild(1).GetChild(2).gameObject; //Sell Shop shop UI button
        //hide the previous open text
        GameObject.Find("InGameUI").transform.GetChild(0).gameObject.SetActive(false);
        currentItems = new List<Item>(Player.GetComponent<Inventory>().itemDict.Keys);

        OpenShop();
    }

    public void OpenBuyInv (Inventory shopInventory) {
        //for the player to buy items from the shop keeper
        isBuying = true;
        BuyInv.SetActive(true);
        SelectedShop = BuyInv.transform.GetChild(1).GetChild(2).gameObject; //Buy Shop shop UI button
        //hide the previous open text
        GameObject.Find("InGameUI").transform.GetChild(0).gameObject.SetActive(false);
        currentItems = new List<Item>(shopInventory.itemDict.Keys);

        OpenShop();
    }

    private void UpdateShopItemList () {
        //item description
        Item item = currentItems[offset+2];
        string itemDesciption = item.description + "\ncost: " + item.cost.ToString();
        if (isBuying) {
            BuyInv.transform.GetChild(2).GetComponent<Text>().text = itemDesciption;
        } else {
            SellInv.transform.GetChild(2).GetComponent<Text>().text = itemDesciption;
        }
        
        //the list of items
        for (int i = 0; i < 5; i++) {
            string itemName;
            string itemNum;

            if (i+offset < 0 || i+offset >= currentItems.Count) { //outside the bounds
                //this is to account for the 2 above and below the actual item button
                itemName = "--";
                itemNum = "-";
            } else {
                Inventory currentInv;
                if (isBuying) {
                    currentInv = context.GetComponent<Inventory>();
                } else {
                    currentInv = Player.GetComponent<Inventory>();
                }
                itemNum = currentInv.itemDict[currentItems[offset + i]].ToString();
                itemName = currentItems[i + offset].name;
            }

            if (isBuying) {
                BuyInv.transform.GetChild(1).GetChild(i).GetComponentInChildren<Text>().text = itemName;
                BuyInv.transform.GetChild(4).GetChild(i).GetComponentInChildren<Text>().text = itemNum;
            } else {
                SellInv.transform.GetChild(1).GetChild(i).GetComponentInChildren<Text>().text = itemName;
                SellInv.transform.GetChild(4).GetChild(i).GetComponentInChildren<Text>().text = itemNum;
            }
        }
    }

    public void OpenSelectAmount () {
        selectAmount = true;
        ES.SetSelectedGameObject(null);
        if (isBuying) {
            BuyInv.transform.GetChild(3).gameObject.SetActive(true);
            ES.SetSelectedGameObject(BuyInv.transform.GetChild(3).GetChild(1).gameObject);
        } else {
            SellInv.transform.GetChild(3).gameObject.SetActive(true);
            ES.SetSelectedGameObject(SellInv.transform.GetChild(3).GetChild(1).gameObject);
        }
    }

    public void TransferItems (Inventory fromInv, Inventory toInv) {
        //handles money and item amounts
        int num = itemNum;
        Item item = currentItems[offset + 2];
        bool moneyBool = toInv.money >= item.cost * num;

        if (moneyBool && fromInv.RemoveItems(item, num)) {
            toInv.AddItems(item, num);
            //money stuff
            toInv.money -= item.cost * num;
            fromInv.money += item.cost * num;
        }
    }

    public void CompleteTransaction () {
        if (isBuying) {
            Debug.Log("test");
            TransferItems (context.GetComponent<Inventory>(), Player.GetComponent<Inventory>());
        } else {
            TransferItems (Player.GetComponent<Inventory>(), context.GetComponent<Inventory>());
        }
        offset = -2;    //reset offset to avoid out of bounds errors later
        itemNum = 1;    //reset for next time

        CloseShop();
    }

    public void CloseShop () {
        //closes the shop UI
        isShopOpen = false;
        selectAmount = false;
        BuyInv.SetActive(false);
        BuyInv.transform.GetChild(3).gameObject.SetActive(false);
        SellInv.SetActive(false);
        SellInv.transform.GetChild(3).gameObject.SetActive(false);

        //this also lets the player walk again
        activeNode.ExecuteNext(activeNode.GetOutputPort("next"), context);
    }
    #endregion
}
