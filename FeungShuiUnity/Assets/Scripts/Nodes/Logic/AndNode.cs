using UnityEngine;
using XNode;

[CreateNodeMenu("Logic/And"), NodeTint(80, 80, 120)]
public class AndNode : ProcessorNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool input;
    [Output] public bool output;

    public override object GetValue(GameObject context) {
        NodePort inputPort = GetInputPort("input");
        for (int i = 0; i < inputPort.ConnectionCount; i++) {
            if (!(bool)((ProcessorNode)inputPort.GetConnection(i).node).GetValue(context)) {
                output = false;
                return output;
            }
        }
        output = true;
        return output;
    }
}