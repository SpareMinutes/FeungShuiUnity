using UnityEngine;
using XNode;

public abstract class ExecutableNode : Node {
    public override object GetValue(NodePort port) {
        return null;
    }

    public abstract void Execute(GameObject context);

    public void ExecuteNext(NodePort nextPort, GameObject context) {
        for (int i = 0; i < nextPort.ConnectionCount; i++) {
            NodePort connection = nextPort.GetConnection(i);
            ExecutableNode next = (ExecutableNode)connection.node;
            next.Execute(context);
        }
    }
}