using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Interactions/Take Money")]
public class TakeMoneyNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool success, failure;
    public int amount;

    public override void Execute(GameObject context) {
        //removes the amount of money specified to the players inventory
        Inventory playerInv = GameObject.Find("WalkableCharacter").GetComponent<Inventory>();
        bool moneyTaken = amount <= playerInv.money;

        if (moneyTaken) {
            playerInv.money -= amount;
        }

        ExecuteNext(GetOutputPort(moneyTaken ? "success" : "failure"), context);
    }
}
