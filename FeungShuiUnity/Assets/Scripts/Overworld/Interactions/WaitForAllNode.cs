using UnityEngine;
using XNode;

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