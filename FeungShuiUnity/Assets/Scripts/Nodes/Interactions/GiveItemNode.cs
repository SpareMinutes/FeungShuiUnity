using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("Interactions/Actions/Give Items")]
public class GiveItemNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool next;
    public Item item;
    public int amount;

    public override void Execute(GameObject context) {
        //when this node runs, it needs to give the player items that will go into their inventory
        GameObject.Find("WalkableCharacter").GetComponent<Inventory>().AddItems(item, amount);

        ExecuteNext(GetOutputPort("next"), context);
    }
}
