using System;
using UnityEngine;

[CreateNodeMenu("Interactions/End"), NodeTint(200, 100, 100)]
public class EndNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;

    public override void Execute(GameObject context) {
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().disableButton();
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().scheduleReenable();
        ((InteractionGraph)graph).activeNodes.Clear();
        try {
            context.GetComponent<WanderAI>().enabled = true;
        } catch (NullReferenceException e) {

        }
    }
}