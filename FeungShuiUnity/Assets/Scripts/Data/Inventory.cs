using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BagTab {
    BattleItems,
    HeldItems,
    KeyItems,
    Other
}

public class Inventory : MonoBehaviour{

    public int money;

    //dicts
    public Dictionary<BagTab, List<Item>> tabs = new Dictionary<BagTab, List<Item>>();      //purely used to display items in the bag
    public Dictionary<Item, int> itemDict = new Dictionary<Item, int>();                    //amount of each item the inventory has

    //these lists are just for inputting items into the Inventory with the Unity inspector
    [SerializeField]
    private List<Item> ItemsList = new List<Item>();
    [SerializeField]
    private List<int> ItemAmountsList = new List<int>();

    private bool InvHasChanged = true;     //just so it doesnt redefine the tabs dict every time you open, only when something has changed

    public void Start() {
        //assume that ItemsList and ItemAmountsList are the same length
        //and ItemsList contains only 1 of each distinct item
        for (int i = 0; i < ItemsList.Count; i++) itemDict.Add(ItemsList[i], ItemAmountsList[i]);
    }

    public void AddItems(Item item, int num) {
            
        if (itemDict.ContainsKey(item)){
            //then the item is already in the inventory so we want to just increase its amount
            itemDict[item] += num;
        } else {
            itemDict.Add(item, num);
        }
        InvHasChanged = true;
    }

    public bool RemoveItems(Item item, int num) {
        //item not in inventory or subtracting num will result in a negative amount
        if (!itemDict.ContainsKey(item) || itemDict[item] < num) return false;

        //else it's fine to subtract the amount
        itemDict[item] -= num;

        //remove item from itemDict
        if (itemDict[item] == 0) {
            itemDict.Remove(item);
        }

        InvHasChanged = true;
        return true;
    }

    public void OrganiseBagTabs () {
        if (InvHasChanged) {
            tabs = new Dictionary<BagTab, List<Item>>() {
                {BagTab.BattleItems, new List<Item>()},
                {BagTab.HeldItems, new List<Item>()},
                {BagTab.KeyItems, new List<Item>()},
                {BagTab.Other, new List<Item>()}};

            foreach (Item item in itemDict.Keys) tabs[item.tab].Add(item);
            InvHasChanged = false;
        }
        //else no need to change what the bag displays
    }

    public List<Item> GetList(BagTab tab) {
        //returns the list of items in the tab given
        return tabs[tab];
    }
}