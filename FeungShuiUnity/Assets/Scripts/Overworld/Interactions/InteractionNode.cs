using UnityEngine;
using XNode;

public abstract class InteractionNode : Node {

    // Use this for initialization
    protected override void Init() {
		base.Init();
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}

    public abstract void Execute();

    public void ExecuteNext(NodePort nextPort) {
        ((InteractionGraph)graph).activeNodes.Remove(this);
        for (int i = 0; i < nextPort.ConnectionCount; i++) {
            NodePort connection = nextPort.GetConnection(i);
            InteractionNode next = (InteractionNode)connection.node;
            ((InteractionGraph)graph).activeNodes.Add(next);
            next.Execute();
        }
    }
}