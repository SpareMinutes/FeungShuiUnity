using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("NPC AI/Is Defeated")]
public class IsDefeatedNode : ProcessorNode {
    [Output] public bool output;

    public override object GetValue(GameObject context) {
        output = context.GetComponent<Battle>().defeated;
        return output;
    }
}