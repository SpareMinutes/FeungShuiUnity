using UnityEngine;
using XNode;

[CreateNodeMenu("Math/Compare")]
public class CompareNode : ProcessorNode {
    [Input(backingValue = ShowBackingValue.Unconnected)] public float first;
    public Comparison operation;
    [Input(backingValue = ShowBackingValue.Unconnected)] public float second;
    [Output] public bool output;

    public override object GetValue(GameObject context) {
        NodePort firstPort = GetInputPort("first");
        float firstValue = firstPort.ConnectionCount==0 ? first : float.Parse(((ProcessorNode)firstPort.GetConnection(0).node).GetValue(context).ToString());
        NodePort secondPort = GetInputPort("second");
        float secondValue = secondPort.ConnectionCount == 0 ? second : float.Parse(((ProcessorNode)secondPort.GetConnection(0).node).GetValue(context).ToString());
        switch (operation) {
            case Comparison.Equal:
                return firstValue == secondValue;
            case Comparison.GreaterThan:
                return firstValue > secondValue;
            case Comparison.GreaterThanOrEqual:
                return firstValue >= secondValue;
            case Comparison.LessThan:
                return firstValue < secondValue;
            case Comparison.LessThanOrEqual:
                return firstValue <= secondValue;
            case Comparison.NotEqual:
                return firstValue != secondValue;
            default:
                return false;   
        }
    }

    public enum Comparison {
        LessThan,
        LessThanOrEqual,
        Equal,
        GreaterThanOrEqual,
        GreaterThan,
        NotEqual    
    }
}