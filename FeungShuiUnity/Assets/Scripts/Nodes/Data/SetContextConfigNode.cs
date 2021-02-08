using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Data/SetContextConfig"), NodeTint(200, 150, 80)]
public class SetContextConfigNode : ExecutableNode {
    public string component, field;
    [Input(backingValue = ShowBackingValue.Unconnected)] public string value;
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool next;

    public override void Execute(GameObject context) {
        Component comp = context.GetComponent(System.Type.GetType(component));
        string v = value.Equals("") ? ((ProcessorNode)GetInputPort("value").GetConnection(0).node).GetValue(context).ToString() : value;
        comp.GetType().GetField(field).SetValue(comp, v);
        ExecuteNext(GetOutputPort("next"), context);
    }
}