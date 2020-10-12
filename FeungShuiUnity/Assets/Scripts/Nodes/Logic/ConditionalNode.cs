using UnityEngine;

[CreateNodeMenu("Logic/Conditional")]
public class ConditionalNode : ExecutableNode {
    [Input(backingValue = ShowBackingValue.Never), Tooltip("Uses the value of the logic node connected to this portw hen running.")] public bool value;
    [Input(backingValue = ShowBackingValue.Never), Tooltip("Will run when a node connected to this port finishes.")] public bool trigger;
    [Output, Tooltip("This branch will run when Test is true.")] public bool WhenTrue;
    [Output, Tooltip("This branch will run when Test is false.")] public bool WhenFalse;

    public override void Execute(GameObject context) {
        value = (bool)((ProcessorNode)GetInputPort("value").GetConnection(0).node).GetValue(context);
        ExecuteNext(GetOutputPort(value ? "WhenTrue" : "WhenFalse"), context);
    }
}