using UnityEngine;
using XNode;

public abstract class ExecutableNode : Node {
    public abstract void Execute(GameObject context);
}