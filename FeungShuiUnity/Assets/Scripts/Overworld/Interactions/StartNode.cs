using UnityEngine;
public class StartNode : InteractionNode {
    [Output] public bool next;

    public override void Execute() {
        Debug.Log("Start");
        ExecuteNext(GetOutputPort("next"));
    }
}