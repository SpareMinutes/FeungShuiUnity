using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("NPC AI/Is Defeated")]
public class IsDefeatedNode : LogicNode {
    [Output] public bool output;

    public override bool GetValue(GameObject context) {
        output = context.GetComponent<Battle>().defeated;
        return output;
    }
}