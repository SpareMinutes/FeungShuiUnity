using UnityEngine;
using XNode;

[CreateNodeMenu("Logic/And")]
public class AndNode : LogicNode {
    public override void Execute(GameObject context) {
        NodePort inputPort = GetInputPort("input");
        for (int i = 0; i < inputPort.ConnectionCount; i++) {
            if (!(bool)inputPort.GetConnection(i).GetOutputValue()) {
                output = false;
                ExecuteNext(GetOutputPort("output"), context);
                return;
            }
        }
        output = true;
        ExecuteNext(GetOutputPort("output"), context);
    }
}