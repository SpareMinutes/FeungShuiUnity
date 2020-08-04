using UnityEngine;

public class SetStartNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool next;
    public StartNode newStart;

    public override void Execute() {
        ((InteractionGraph)graph).Start = newStart;
        ExecuteNext(GetOutputPort("next"));
    }
}