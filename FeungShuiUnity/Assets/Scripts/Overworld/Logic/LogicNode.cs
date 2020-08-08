using UnityEngine;
using XNode;

[CreateNodeMenu("Logic")]
public abstract class LogicNode : ExecutableNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool input;
    [Output] public bool output;

    public override object GetValue(NodePort port) {
        return output;
    }

    public void ExecuteNext(NodePort nextPort, GameObject context) {
        for (int i = 0; i < nextPort.ConnectionCount; i++) {
            NodePort connection = nextPort.GetConnection(i);
            ExecutableNode next = (ExecutableNode)connection.node;
            next.Execute(context);
        }
    }
}