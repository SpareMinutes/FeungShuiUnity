using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Data/HasItem"), NodeTint(200, 150, 80)]
public class HasItemNode : ProcessorNode {
    public Item item;
    public int amount;
    [Output] public bool output;

    public override object GetValue(GameObject context) {
        Inventory playerInv = GameObject.Find("WalkableCharacter").GetComponent<Inventory>();

        if (playerInv.itemDict.ContainsKey(item)) {
            //Debug.Log(playerInv.itemDict[item] + ", " + amount);
            return playerInv.itemDict[item] >= amount;
        } else {
            return false;
        }
    }
}