using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemEffect {
    Heal,
    BuffAttack,
    //etc...
}

[CreateAssetMenu()]
public class Item : ScriptableObject {

    public BagTab tab; //will be what tab the item should go in (made manually in the editor)
    public string displayName;
    public ItemEffect function;     //what it does
    public int potency;             //how well it does it

    [HideInInspector]
    public int amount = 0; //will keep track of how many of this item the plaer has in their inventory

    /* //need to have a similar thing to the move to determine what their effect is
    public Item (string name, string bagTab, int function, float power) {
        displayName = name;
        tab = bagTab;
        this.function = function; //defined the type of item it is
        this.power = power; //determines how potent the item is
            //like how much an item heals for healing items
                //or steps for a repel to wear off
                //for items like held items either this value doesnt matter
    } */

    public void use () {

        switch (function) {
            //each case is a different type of item, each item only has one function
            case ItemEffect.Heal : {
                Debug.Log("will heal a Eidolon by " + potency.ToString() + " health points");
                break;
            }
            case ItemEffect.BuffAttack : {
                Debug.Log("will buff an eidolon's attack stat by " + potency.ToString() + "x");
                break;
            }
            //etc...
        }
    }
    
}
