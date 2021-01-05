using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Logic/Selector"), NodeTint(80, 80, 120)]
public class SelectorNode : ProcessorNode {
    [Input(backingValue = ShowBackingValue.Unconnected)] public string trueValue, falseValue;
    [Input(backingValue = ShowBackingValue.Never)] public bool selector;
    [Output] public string outValue;

    public override object GetValue(GameObject context) {
        return (bool)((ProcessorNode)GetPort("selector").GetConnection(0).node).GetValue(context) ?
            ((ProcessorNode)GetPort("trueValue").GetConnection(0).node).GetValue(context) : 
            ((ProcessorNode)GetPort("falseValue").GetConnection(0).node).GetValue(context);
    }
}