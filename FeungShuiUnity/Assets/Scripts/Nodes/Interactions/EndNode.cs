using System;
using UnityEngine;

[CreateNodeMenu("Interactions/End"), NodeTint(200, 100, 100)]
public class EndNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;

    public override void Execute(GameObject context) {
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().disableButton();
        ((InteractionGraph)graph).activeNodes.Clear();
        Debug.Log(context);
        try {
            context.GetComponent<WanderAI>().enabled = true;
        } catch (NullReferenceException e) {

        }
    }
}