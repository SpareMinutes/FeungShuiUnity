using UnityEngine;
using XNode;

[CreateNodeMenu("Interactions")]
public abstract class InteractionNode : ExecutableNode {

    // Use this for initialization
    protected override void Init() {
		base.Init();
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}

    public void ExecuteNext(NodePort nextPort, GameObject context) {
        ((InteractionGraph)graph).activeNodes.Remove(this);
        for (int i = 0; i < nextPort.ConnectionCount; i++) {
            NodePort connection = nextPort.GetConnection(i);
            InteractionNode next = (InteractionNode)connection.node;
            ((InteractionGraph)graph).activeNodes.Add(next);
            next.Execute(context);
        }
    }
}