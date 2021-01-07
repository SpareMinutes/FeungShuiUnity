using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("Interactions/Open Sell Invetory")]
public class OpenSellInventoryNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool next;

    public override void Execute(GameObject context) {

        //shop inventory
        Inventory inventory = context.GetComponent<Inventory>(); //this inventory is just needed for the transfer of money between the player and npc
        
        GameObject.Find("InGameUI").GetComponent<OverworldUI>().SetActiveNode(this);
        //GameObject.Find("InGameUI").GetComponent<OverworldUI>().SetContext(context);

        //GameObject.Find("InGameUI").GetComponent<OverworldUI>().OpenSellInv(inventory);
    }
}
