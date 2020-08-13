using UnityEngine;
using XNode;

[CreateNodeMenu("Logic/Or")]
public class OrNode : LogicNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool input;
    [Output] public bool output;

    public override bool GetValue(GameObject context) {
        NodePort inputPort = GetInputPort("input");
        for (int i = 0; i < inputPort.ConnectionCount; i++) {
            if (((LogicNode)inputPort.GetConnection(0).node).GetValue(context)) {
                output = true;
                return output;
            }
        }
        output = false;
        return output;
    }
}