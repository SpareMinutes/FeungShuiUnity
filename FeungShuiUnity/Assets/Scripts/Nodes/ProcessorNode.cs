using UnityEngine;
using XNode;

public abstract class ProcessorNode : Node {
    public abstract object GetValue(GameObject context);
}