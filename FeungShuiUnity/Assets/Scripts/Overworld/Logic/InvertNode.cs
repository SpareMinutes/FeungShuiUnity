using UnityEngine;

[CreateNodeMenu("Logic/Invert")]
public class InvertNode : LogicNode {
    public override void Execute(GameObject context) {
        output = !(bool)GetInputPort("input").GetConnection(0).GetOutputValue();
        ExecuteNext(GetOutputPort("output"), context);
    }
}