using UnityEngine;
using XNode;

[CreateNodeMenu("NPC AI/Toggle AI Mode")]
public class ToggleAIModeNode : AINode {
    [Input(backingValue = ShowBackingValue.Never), Tooltip("Uses the value of the logic node connected to this portw hen running.")] public bool value;
    [Input(backingValue = ShowBackingValue.Never), Tooltip("Will run when a node connected to this port finishes.")] public bool trigger;
    [Output] public bool next;
    public string ModeName;
    
    public override object GetValue(NodePort port) {
        return next;
    }

    public override void Execute(GameObject context) {
        value = (bool)((ProcessorNode)GetInputPort("value").GetConnection(0).node).GetValue(context);
        ((MonoBehaviour)context.GetComponent(ModeName)).enabled = value;
        next = value;
        ExecuteNext(GetOutputPort("next"), context);
    }
}