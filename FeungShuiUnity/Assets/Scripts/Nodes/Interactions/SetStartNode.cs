using UnityEngine;

[CreateNodeMenu("Interactions/Flow Control/Set Start")]
public class SetStartNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool next;
    public StartNode newStart;

    public override void Execute(GameObject context) {
        ((InteractionGraph)graph).Start = newStart;
        ExecuteNext(GetOutputPort("next"), context);
    }
}