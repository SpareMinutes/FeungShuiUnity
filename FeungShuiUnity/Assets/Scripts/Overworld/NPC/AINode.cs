using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("NPC AI")]
public abstract class AINode : ExecutableNode {
    public abstract override object GetValue(NodePort port);

    public void ExecuteNext(NodePort nextPort, GameObject context) {
        for (int i = 0; i < nextPort.ConnectionCount; i++) {
            NodePort connection = nextPort.GetConnection(i);
            ExecutableNode next = (ExecutableNode)connection.node;
            next.Execute(context);
        }
    }
}