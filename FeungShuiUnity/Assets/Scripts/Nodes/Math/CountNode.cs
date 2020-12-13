using UnityEngine;
using XNode;

[CreateNodeMenu("Math/Count"), NodeTint(100, 150, 180)]
public class CountNode : ProcessorNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool input;
    [Output] public int output;
    public bool CountTrues;

    public override object GetValue(GameObject context) {
        NodePort inputPort = GetInputPort("input");
        int result = 0;
        for (int i = 0; i < inputPort.ConnectionCount; i++)
            if ((bool)((ProcessorNode)inputPort.GetConnection(i).node).GetValue(context) == CountTrues)
                result++;
        return result;
    }
}