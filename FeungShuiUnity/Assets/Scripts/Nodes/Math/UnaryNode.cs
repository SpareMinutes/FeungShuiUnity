using UnityEngine;
using XNode;

[CreateNodeMenu("Math/Unary"), NodeTint(100, 150, 180)]
public class UnaryNode : ProcessorNode {
    [Input(backingValue = ShowBackingValue.Unconnected)] public float value;
    public Unary operation;
    [Output] public float output;

    public override object GetValue(GameObject context) {
        NodePort firstPort = GetInputPort("value");
        float val = firstPort.ConnectionCount == 0 ? value : float.Parse(((ProcessorNode)firstPort.GetConnection(0).node).GetValue(context).ToString());
        switch (operation) {
            case Unary.AbsoluteValue:
                Debug.Log("Abs: " + val);
                return Mathf.Abs(val);
            default:
                return false;
        }
    }

    public enum Unary {
        AbsoluteValue,
    }
}