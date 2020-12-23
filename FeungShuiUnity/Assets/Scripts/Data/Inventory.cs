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
    public Dictionary<BagTab, List<Item>> tabs = new Dictionary<BagTab, List<Item>>();
    public Dictionary<Item, int> itemDict = new Dictionary<Item, int>();     //amount of each item the inventory has

    //these lists are just for inputting items into the Inventory with the Unity inspector
    [SerializeField]
    private List<Item> ItemsList = new List<Item>();
    [SerializeField]
    private List<int> ItemAmountsList = new List<int>();

    public void Start() {
        //init the dictionary for look ups later on
        tabs = new Dictionary<BagTab, List<Item>>() {
            {BagTab.BattleItems, new List<Item>()},
            {BagTab.HeldItems, new List<Item>()},
            {BagTab.KeyItems, new List<Item>()},
            {BagTab.Other, new List<Item>()}
        };

        for (int i = 0; i < ItemsList.Count; i++) {
            //assume that ItemsList and ItemAmountsList are the same length
            //and ItemsList contains only 1 of each distinct item
            itemDict.Add(ItemsList[i], ItemAmountsList[i]);

            //update bag
            Item item = ItemsList[i];
            tabs[item.tab].Add(item);
        }
    }

    //Keeps inspector in-sync with internal data.
    //Should have no bearing on actual gameplay.
    private void UpdateItemsList() {
        ItemsList.Clear();
        ItemAmountsList.Clear();
        foreach (Item item in itemDict.Keys) {
            ItemsList.Add(item);
            ItemAmountsList.Add(itemDict[item]);
        }
    }

    public void AddItems(Item item, int num) {
        if (itemDict.ContainsKey(item)) {
            //then the item is already in the inventory so we want to just increase its amount
            itemDict[item] += num;
        } else {
            //need to add it 
            itemDict.Add(item, num);
            tabs[item.tab].Add(item);
        }
        UpdateItemsList();
    }

    public bool RemoveItems(Item item, int num) {
        if (!itemDict.ContainsKey(item) || itemDict[item] < num) {
            //item not in inventory or subtracting num will result in a negative amount
            return false;
        }

        itemDict[item] -= num;

        if (itemDict[item] == 0) {
            //then you dont own any of the item anymore
            //ItemsList.Remove(item);
            itemDict.Remove(item);
            tabs[item.tab].Remove(item);
        }

        UpdateItemsList();
        return true;
    }

    public List<Item> GetList(BagTab tab) {
        //returns the list of items in the tab given
        //should be called by the 
        return tabs[tab];
    }
}
