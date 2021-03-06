using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Data/GraphMethod"), NodeTint(200, 150, 80)]
public class GraphMethodNode : ProcessorNode {
    public string method;
    [Output] public bool output;

    public override object GetValue(GameObject context) {
        return typeof(NodeGraph).GetMethod(method).Invoke(graph, null);
    }
}