using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopMenu : ItemMenu {
    public GameObject AmountSelection, SellText, BuyText;
    public int menuType;
    [HideInInspector]
    public Inventory npcInv, playerInv;

    private GameObject menu;
    private List<Item> itemList = new List<Item>();
    private Inventory currInv, otherInv;
    private int itemIndex, amount, maxAmount;
    private EventSystem ES;
    private bool itemScrolling;
    private Item selectedItem;

    void Start(){
        menu = GameObject.Find("Menu");
        menuType = 0;
        itemIndex = 0;
        itemScrolling = true;
        ES = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        ES.SetSelectedGameObject(ItemList.GetComponentInChildren<Button>().gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            if (itemScrolling) {
                //exit out of the shop altogether
                ReturnToLast();
            } else {
                //exit back to buy/sell menus
                AmountSelection.SetActive(false);
                Button button = ItemList.GetComponentInChildren<Button>();
                button.interactable = true;
                ES.SetSelectedGameObject(button.gameObject);
                selectedItem = null;
                itemScrolling = true;
            }
        }
        
        int lastType = menuType;
        if (Input.GetButtonDown("Horizontal")) {
            if (itemScrolling) {
                //just so that you can change between buy/sell once an item has been selected
                if (Input.GetAxisRaw("Horizontal") > 0)//right
                    menuType = Mathf.Min(1, menuType + 1);
                else//left
                    menuType = Mathf.Max(-1, menuType - 1);
            }

            if (lastType != menuType) {
                switch (menuType) {
                    case -1:
                        //sell
                        menu.transform.localPosition = new Vector3(-180, 0, 0);
                        AmountSelection.transform.localPosition = new Vector3(95, -55, 0);
                        BuyText.SetActive(false);

                        itemList = new List<Item>(playerInv.itemDict.Keys);
                        currInv = playerInv;
                        otherInv = npcInv;
                        UpdateItemList(itemList, currInv, 0);

                        StartCoroutine("MoveRight");
                        break;
                    case 1:
                        //buy
                        menu.transform.localPosition = new Vector3(180, 0, 0);
                        AmountSelection.transform.localPosition = new Vector3(-95, -55, 0);
                        SellText.SetActive(false);

                        itemList = new List<Item>(npcInv.itemDict.Keys);
                        currInv = npcInv;
                        otherInv = playerInv;
                        UpdateItemList(itemList, currInv, 0);

                        StartCoroutine("MoveLeft");
                        break;
                    case 0:
                        //choose
                        BuyText.SetActive(true);
                        SellText.SetActive(true);
                        switch (lastType) {
                            case -1:
                                StartCoroutine("MoveLeft");
                                break;
                            case 1:
                                StartCoroutine("MoveRight");
                                break;
                        }
                        break;
                }
            }
        }

        if (Input.GetButtonDown("Vertical")) {
            if (itemScrolling) {
                if (Input.GetAxisRaw("Vertical") > 0) {
                    //scroll items up
                    itemIndex --;
                } else {
                    //scroll items down
                    itemIndex ++;
                }
                itemIndex += itemList.Count;
                itemIndex %= itemList.Count;
                UpdateItemList(itemList, currInv, itemIndex);
            } else { 
                //amount select
                if (Input.GetAxisRaw("Vertical") > 0) {
                    //increase amount
                    changeItemAmount(1);
                } else {
                    //decrease amount
                    changeItemAmount(-1);
                }
            }
            
        }
    }

    IEnumerator MoveLeft() {
        for(int i=0; i<60; i++) {
            menu.transform.localPosition = new Vector3(menu.transform.localPosition.x - 2, 0, 0);
            yield return null; // new WaitForSecondsRealtime(0.001f);
        }
    }

    IEnumerator MoveRight() {
        for (int i = 0; i < 60; i++) {
            menu.transform.localPosition = new Vector3(menu.transform.localPosition.x + 2, 0, 0);
            yield return null; //new WaitForSecondsRealtime(0.001f);
        }
    }

    public void selectItem () {
        ItemList.GetComponentInChildren<Button>().interactable = false;
        AmountSelection.SetActive(true);
        ES.SetSelectedGameObject(AmountSelection.GetComponentInChildren<Button>().gameObject);
        itemScrolling = false;
        selectedItem = itemList[itemIndex];

        //calculate the max amount for the selected item
        maxAmount = (otherInv.money - otherInv.money % selectedItem.cost) / selectedItem.cost;
        maxAmount = Mathf.Min(currInv.itemDict[selectedItem], maxAmount);
        if (maxAmount == 0) {
            amount = 0; //cant buy any of that item
            //probably want a popup message informing the player they cant buy/sell any of that item
        } else {
            amount = 1; //can buy at least 1 of that item
        }
        //refresh button text
        AmountSelection.GetComponentInChildren<Text>().text = amount.ToString();
    }

    private void changeItemAmount (int num) {
        if (amount + num > maxAmount) {
            //cant buy more than the money or stock will allow
            amount = 1;
        } else if (amount + num <= 0) {
            //cant buy a negative amount
            amount = maxAmount;
        } else {
            // otherwise its fine to change the amount
            amount += num;
        }
        //update visual representation
        AmountSelection.GetComponentInChildren<Text>().text = amount.ToString();
    }

    public void TransferItems () {
        Debug.Log("went through");
        //money transfer
        currInv.money += amount * selectedItem.cost;
        otherInv.money -= amount * selectedItem.cost;

        //item transfer
        currInv.RemoveItems(selectedItem, amount); //should never return false
        otherInv.AddItems(selectedItem, amount);

        //exit back to item scrolling
        AmountSelection.SetActive(false);
        Button button = ItemList.GetComponentInChildren<Button>();
        button.interactable = true;
        ES.SetSelectedGameObject(button.gameObject);
        selectedItem = null;
        itemScrolling = true;
    }


}
