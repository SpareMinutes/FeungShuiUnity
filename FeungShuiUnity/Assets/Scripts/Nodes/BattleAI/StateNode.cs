using System;
using UnityEngine;

[CreateNodeMenu("Battle/State"), NodeTint(100, 200, 100)]
public class StateNode : ExecutableNode {
    [Output] public bool next;

    public override void Execute(GameObject context) {
        ExecuteNext(GetOutputPort("next"), context);
    }
}