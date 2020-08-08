using UnityEngine;
using XNode;

[CreateNodeMenu("NPC AI/Begin Check")]
public class BeginCheckNode : AINode {
    [Output] public bool next;

    public override object GetValue(NodePort port) {
        return true;
    }

    public override void Execute(GameObject context) {
        ExecuteNext(GetOutputPort("next"), context);
    }
}