using UnityEngine;
using XNode;

[CreateNodeMenu("Logic")]
public abstract class LogicNode : Node {
    public abstract bool GetValue(GameObject context);
}