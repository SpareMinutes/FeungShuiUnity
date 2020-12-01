using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("Interactions/Open Buy Invetory")]
public class OpenBuyInventoryNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool next;

    public override void Execute(GameObject context) {

        //shop inventory
        Inventory buyInventory = context.GetComponent<Inventory>();

        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().SetActiveNode(this);
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().SetContext(context);
        
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().OpenBuyInv(buyInventory);
    }

}
