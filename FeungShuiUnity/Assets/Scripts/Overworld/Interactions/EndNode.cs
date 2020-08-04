using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class EndNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;

    public override void Execute() {
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().disableButton();
        ((InteractionGraph)graph).activeNodes.Clear();
    }
}