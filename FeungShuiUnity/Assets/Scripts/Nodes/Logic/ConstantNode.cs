using UnityEngine;

[CreateNodeMenu("Logic/Constant")]
public class ConstantNode : LogicNode {
    public bool value;
    [Output] public bool output;

    public override bool GetValue(GameObject context) {
        output = value;
        return output;
    }
}