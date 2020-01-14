using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

public class Item {

    public string tab; //will be what tab the item should go in (made manually in the editor)
    public int amount = 0; //will keep track of how many of this item hte plaer has in their inventory
    public string displayName;

    private int function;
    private float power;

    //need to have a similar thing to the move to determine what their effect is
    public Item (string name, string bagTab, int function, float power) {
        displayName = name;
        tab = bagTab;
        this.function = function; //defined the type of item it is
        this.power = power; //determines how potent the item is
            //like how much an item heals for healing items
                //or steps for a repel to wear off
                //for items like held items either this value doesnt matter
    }

    public void use () {

        switch (function) {
            //each case is a different type of item, each item only has one function
            case 0 : {
                
                break;
            }
            case 1 : {

                break;
            }
            case 2 : {

                break;
            }
        }
    }
    
}
