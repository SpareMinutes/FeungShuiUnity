using UnityEngine;
using XNode;

[CreateNodeMenu("NPC AI/Aggro")]
public class AggroNode : AINode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool next;

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port) {
        return next;
	}

    public override void Execute(GameObject context) {
        next = context.GetComponent<GuardAI>().isAggroed();
        ExecuteNext(GetOutputPort("next"), context);
    }
}