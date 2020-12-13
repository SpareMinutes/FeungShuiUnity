using UnityEngine;
using XNode;

[CreateNodeMenu("Math/Operation"), NodeTint(100, 150, 180)]
public class OperationNode : ProcessorNode {
    [Input(backingValue = ShowBackingValue.Unconnected)] public float first;
    public Operation operation;
    [Input(backingValue = ShowBackingValue.Unconnected)] public float second;
    [Output] public float output;

    public override object GetValue(GameObject context) {
        NodePort firstPort = GetInputPort("first");
        float firstValue = firstPort.ConnectionCount == 0 ? first : float.Parse(((ProcessorNode)firstPort.GetConnection(0).node).GetValue(context).ToString());
        NodePort secondPort = GetInputPort("second");
        float secondValue = secondPort.ConnectionCount == 0 ? second : float.Parse(((ProcessorNode)secondPort.GetConnection(0).node).GetValue(context).ToString());
        switch (operation) {
            case Operation.Plus:
                return firstValue + secondValue;
            case Operation.Minus:
                return firstValue - secondValue;
            case Operation.Times:
                return firstValue * secondValue;
            case Operation.DividedBy:
                return firstValue / secondValue;
            case Operation.Modulo:
                return firstValue % secondValue;
            case Operation.ToPowerOf:
                return Mathf.Pow(firstValue, secondValue);
            default:
                return false;
        }
    }

    public enum Operation {
        Plus,
        Minus,
        Times,
        DividedBy,
        Modulo,
        ToPowerOf
    }
}