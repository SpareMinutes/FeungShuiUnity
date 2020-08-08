using System;
using UnityEngine;

[CreateNodeMenu("Interactions/End")]
public class EndNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;

    public override void Execute() {
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().disableButton();
        ((InteractionGraph)graph).activeNodes.Clear();
        try {
            GameObject obj = ((InteractionGraph)graph).GetConnectedObject();
            obj.GetComponent<WanderAI>().enabled = true;
        } catch (NullReferenceException e) {

        }
    }
}