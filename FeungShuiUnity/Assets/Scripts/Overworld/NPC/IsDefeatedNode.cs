using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("NPC AI/Is Defeated")]
public class IsDefeatedNode : AINode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool next;

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port) {
        return next;
	}

    public override void Execute(GameObject context) {
        next = context.GetComponent<Battle>().defeated;
        ExecuteNext(GetOutputPort("next"), context);
    }
}