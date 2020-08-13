using UnityEngine;

[CreateNodeMenu("NPC AI/Aggro")]
public class AggroNode : LogicNode {
    [Output] public bool output;

    // Return the correct value of an output port when requested
    public override bool GetValue(GameObject context) {
        output = context.GetComponent<GuardAI>().isAggroed();
        return output;
	}
}