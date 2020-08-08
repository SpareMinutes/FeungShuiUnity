using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("NPC AI/AI Selector")]
public class AISelectorNode : AINode {
    [Tooltip("The test that determines the output state.")]
    [Input(backingValue = ShowBackingValue.Never)] public bool Test;
    [Tooltip("The branch that's used when Test is true. Will send notification to following nodes if and only if changed.")]
    [Output] public bool WhenTrue;
    [Tooltip("The branch that's used when Test is false. Will send notification to following nodes if and only if changed.")]
    [Output] public bool WhenFalse;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
        return port.Equals(GetOutputPort("WhenTrue")) ? WhenTrue : WhenFalse;
    }

    public override void Execute(GameObject context) {
        bool result = (bool)GetInputPort("Test").GetConnection(0).GetOutputValue();
        if (result != Test) {
            Test = result;
            WhenTrue = result;
            WhenFalse = !result;
            ExecuteNext(GetOutputPort("WhenTrue"), context);
            ExecuteNext(GetOutputPort("WhenFalse"), context);
        }
    }
}