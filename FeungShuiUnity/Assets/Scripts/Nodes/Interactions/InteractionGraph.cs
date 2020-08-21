using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class InteractionGraph : NodeGraph {
    public StartNode Start;
    public List<InteractionNode> activeNodes = new List<InteractionNode>();
    private GameObject ConnectedObject;

    public void Execute(GameObject context) {
        Start.Execute(context);
    }
}