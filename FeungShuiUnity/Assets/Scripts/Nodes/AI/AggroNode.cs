using UnityEngine;

[CreateNodeMenu("NPC AI/Aggro")]
public class AggroNode : ProcessorNode {
    [Output] public bool output;

    // Return the correct value of an output port when requested
    public override object GetValue(GameObject context) {
        output = context.GetComponent<GuardAI>().isAggroed();
        return output;
	}
}