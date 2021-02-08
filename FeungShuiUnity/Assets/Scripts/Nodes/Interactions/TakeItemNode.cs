using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Interactions/Actions/Take Items")]
public class TakeItemNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool success, failure;
    public Item item;
    public int amount;

    public override void Execute(GameObject context) {
        //when this node runs, it needs to take items from the player's inventory
        bool itemsTaken = GameObject.Find("WalkableCharacter").GetComponent<Inventory>().RemoveItems(item, amount);

        ExecuteNext(GetOutputPort(itemsTaken ? "success" : "failure"), context);
    }
}