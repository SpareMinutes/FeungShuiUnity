using UnityEngine;

[CreateNodeMenu("Logic/Invert")]
public class InvertNode : LogicNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool input;
    [Output] public bool output;

    public override bool GetValue(GameObject context) {
        output = !((LogicNode)GetInputPort("input").GetConnection(0).node).GetValue(context);
        return output;
    }
}