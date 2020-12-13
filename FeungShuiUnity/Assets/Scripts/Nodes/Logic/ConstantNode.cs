using UnityEngine;

[CreateNodeMenu("Logic/Constant"), NodeTint(80, 80, 120)]
public class ConstantNode : ProcessorNode {
    public bool value;
    [Output] public bool output;

    public override object GetValue(GameObject context) {
        output = value;
        return output;
    }
}