
using XNode;

[CreateNodeMenu("NPC AI")]
public abstract class AINode : ExecutableNode {
    public abstract override object GetValue(NodePort port);
}