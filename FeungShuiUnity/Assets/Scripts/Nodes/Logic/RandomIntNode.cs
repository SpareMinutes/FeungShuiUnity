using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Logic/RandomInt"), NodeTint(80, 80, 120)]
public class RandomIntNode : ProcessorNode {
    [Input(backingValue = ShowBackingValue.Unconnected)] public int min, max;
    [Output] public bool output;

    public override object GetValue(GameObject context) {
        min = GetOutputPort("min").ConnectionCount == 0 ? min : (int)((ProcessorNode)GetInputPort("min").GetConnection(0).node).GetValue(context);
        max = GetOutputPort("max").ConnectionCount == 0 ? min : (int)((ProcessorNode)GetInputPort("max").GetConnection(0).node).GetValue(context);
        return Random.Range(min, max);
    }
}