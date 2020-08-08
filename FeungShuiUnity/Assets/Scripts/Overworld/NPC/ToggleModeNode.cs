using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("NPC AI/Toggle AI Mode")]
public class ToggleAIModeNode : AINode {
    [Input(backingValue = ShowBackingValue.Never)] public bool Value;
    [Output] public bool next;
    public string ModeName;
    
    public override object GetValue(NodePort port) {
        return next;
    }

    public override void Execute(GameObject context) {
        Value = (bool)GetInputPort("Value").GetConnection(0).GetOutputValue();
        ((MonoBehaviour)context.GetComponent(ModeName)).enabled = Value;
        next = Value;
        ExecuteNext(GetOutputPort("next"), context);
    }
}