using UnityEngine;

[CreateNodeMenu("Logic/Invert"), NodeTint(80, 80, 120)]
public class InvertNode : ProcessorNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool input;
    [Output] public bool output;

    public override object GetValue(GameObject context) {
        output = !(bool)((ProcessorNode)GetInputPort("input").GetConnection(0).node).GetValue(context);
        return output;
    }
}