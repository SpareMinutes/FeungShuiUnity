using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BagTabs {
        //these names are up for change (and probably should be)
        BattleItems, //things like potions and paralyze heals etc...
        HeldItems, //these are for held items like leftovers, or megastones
        KeyItems, //these are the key items from pokemon, basically story items that are needed for the plot or quests or something
        Other //these are items that dont fit into the other categories, like evolutions items, pokeballs, that kindna stuff
}

public class Inventory : MonoBehaviour{

    public Dictionary<BagTabs, List<Item>> tabs = new Dictionary<BagTabs, List<Item>>();

    public void Start () {
        //becuase Inventory is instanceble anything can have an inventory and can store any items the player can
        /* tabs = new Dictionary<BagTabs, List<Item>>() 
            {
                {BattleItems, new List<Item>()},
                {HeldItems, new List<Item>()},
                {KeyItems, new List<Item>()},
                {Other, new List<Item>()}
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
    
    public List<Item> GetList (BagTabs tab) {
        //returns the list of items in the tab given
        //should be called by the 
        return tabs[tab];
    }
}
