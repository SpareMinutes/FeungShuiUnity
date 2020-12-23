using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Data/String"), NodeTint(200, 150, 80)]
public class StringNode : ProcessorNode {
    public string value;
    [Output] public string output;

    public override object GetValue(GameObject context) {
        return value;
    }
}