using UnityEngine;
using XNode;

[CreateNodeMenu("Interactions/Wait For All")]
public class WaitForAllNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool next;

    public override void Execute() {
        foreach(NodePort port in GetInputPort("previous").GetConnections()) {
            if (((InteractionGraph)graph).activeNodes.Contains((InteractionNode)port.node))
                return;
        }
        ExecuteNext(GetOutputPort("next"));
    }
}