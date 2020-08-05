using System;
using UnityEngine;

public class EndNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;

    public override void Execute() {
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().disableButton();
        graph.activeNodes.Clear();
        try {
            GameObject obj = graph.GetConnectedObject();
            obj.GetComponent<WanderAI>().enabled = true;
        } catch (NullReferenceException e) {

        }
    }
}