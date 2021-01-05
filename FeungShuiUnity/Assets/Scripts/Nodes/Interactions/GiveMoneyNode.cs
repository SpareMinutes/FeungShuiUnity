using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Interactions/Give Money")]
public class GiveMoneyNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool next;

    public int amount;

    public override void Execute(GameObject context) {
        //add the amount of money specified to the players inventory
        GameObject.Find("WalkableCharacter").GetComponent<Inventory>().money += amount;
        ExecuteNext(GetOutputPort("next"), context);
    }
}


