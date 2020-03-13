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

    public List<Item> BattleItems, HeldItems, KeyItems, Other = new List<Item>();

    public Dictionary<BagTab, List<Item>> tabs = new Dictionary<BagTab, List<Item>>();

    public void Start () {

        //init the dictionary for look ups later on
        tabs = new Dictionary<BagTab, List<Item>>() {
            {BagTab.BattleItems, BattleItems},
            {BagTab.HeldItems, HeldItems},
            {BagTab.KeyItems, KeyItems},
            {BagTab.Other, Other}
        };
        
        /* tabs = new Dictionary<string, List<Item>>() 
            {
                {"BattleItems", new List<Item>{new Item("itemone","BattleItems",0,0), 
                    new Item("itemtwo","BattleItems",0,0), new Item("itemthree","BattleItems",0,0), 
                    new Item("itemfour","BattleItems",0,0), new Item("itemfive","BattleItems",0,0), 
                    new Item("itemsix","BattleItems",0,0), new Item("itemseven","BattleItems",0,0)}},
                {"HeldItems", new List<Item>()},
                {"KeyItems", new List<Item>()},
                {"Other", new List<Item>()}
            }; */
    }


    public void Add (Item item) {
        //this fucntion is used for all items to add an item to the inventory
        //it will sort the item into the right list
        if (tabs[item.tab].Contains(item)) {
            //then the item is already in the inventory so we want to just increase its amount
            int index = tabs[item.tab].IndexOf(item); //just makes it a little clearer
            tabs[item.tab][index].amount ++;
        } else {
            //we want to add it to the list
            tabs[item.tab].Add(item);
        }
        
    }
    
    public List<Item> GetList (BagTab tab) {
        //returns the list of items in the tab given
        //should be called by the 
        return tabs[tab];
    }
}
