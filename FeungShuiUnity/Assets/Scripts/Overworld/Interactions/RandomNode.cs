using System.Collections.Generic;
using UnityEngine;

public class RandomNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output(dynamicPortList = true)] public List<int> outcomes;

    public override void Execute() {
        int result = (int)Random.Range(0, outcomes.ToArray().Length - 0.0000001f);
        ExecuteNext(GetOutputPort("outcomes " + result));
    }
}