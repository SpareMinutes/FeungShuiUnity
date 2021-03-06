using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Battle/Action"), NodeTint(120, 80, 80)]
public class ActionNode : ExecutableNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    public Action action;
    [Input(backingValue = ShowBackingValue.Unconnected)] public int index;

    public override void Execute(GameObject context) {
        throw new System.NotImplementedException();
    }

    public enum Action {
        Attack,
        Defend,
        Switch,
        Item
    }
}