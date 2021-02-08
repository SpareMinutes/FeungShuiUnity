using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Data/GetContextConfig"), NodeTint(200, 150, 80)]
public class GetContextConfigNode : ProcessorNode {
    public string component, field;
    [Output] public string output;

    public override object GetValue(GameObject context) {
        Component comp = context.GetComponent(System.Type.GetType(component));
        return comp.GetType().GetField(field).GetValue(comp);
    }
}