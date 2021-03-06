using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Data/GraphVariable"), NodeTint(200, 150, 80)]
public class GraphVariableNode : ProcessorNode {
    public string variable;
    [Output] public bool output;

    public override object GetValue(GameObject context) {
        return typeof(NodeGraph).GetProperty(variable).GetValue(graph, null);
    }
}