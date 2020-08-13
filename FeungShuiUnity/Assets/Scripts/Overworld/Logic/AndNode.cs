using UnityEngine;
using XNode;

[CreateNodeMenu("Logic/And")]
public class AndNode : LogicNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool input;
    [Output] public bool output;

    public override bool GetValue(GameObject context) {
        NodePort inputPort = GetInputPort("input");
        for (int i = 0; i < inputPort.ConnectionCount; i++) {
            if (!((LogicNode)inputPort.GetConnection(i).node).GetValue(context)) {
                output = false;
                return output;
            }
        }
        output = true;
        return output;
    }
}