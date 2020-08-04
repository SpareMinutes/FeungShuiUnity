using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class InteractionGraph : NodeGraph {
    public StartNode Start;
    public List<InteractionNode> activeNodes = new List<InteractionNode>();

	public void Execute() {
        Start.Execute();
    }
}