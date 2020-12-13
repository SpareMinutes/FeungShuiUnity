using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

[CreateNodeMenu("Math/Function"), NodeTint(100, 150, 180)]
public class FunctionNode : ProcessorNode {
    [Input(backingValue = ShowBackingValue.Unconnected)] public float value;
    public Operation operation;
    [Output] public float output;

    public override object GetValue(GameObject context) {
        NodePort firstPort = GetInputPort("value");
        if (operation == Operation.AbsoluteValue) {
            float val = firstPort.ConnectionCount == 0 ? value : float.Parse(((ProcessorNode)firstPort.GetConnection(0).node).GetValue(context).ToString());
            return Mathf.Abs(val);
        } else {
            List<NodePort> ports = firstPort.GetConnections();
            List<float> vals = new List<float>();
            foreach(NodePort port in ports) {
                vals.Add(float.Parse(((ProcessorNode)port.node).GetValue(context).ToString()));
            }
            switch (operation) {
                case Operation.Min:
                    return vals.Min();
                case Operation.Max:
                    return vals.Max();
                case Operation.Sum:
                    return vals.Sum();
                default:
                    return false;
            }
        }
    }

    public enum Operation {
        AbsoluteValue,
        Min,
        Max,
        Sum
    }
}