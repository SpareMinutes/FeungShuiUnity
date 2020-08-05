using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class InteractionGraph : NodeGraph {
    public StartNode Start;
    public List<InteractionNode> activeNodes = new List<InteractionNode>();
    private GameObject ConnectedObject;
    public string ConnectedObjectName;

    public void Execute() {
        ConnectedObject = GameObject.Find(ConnectedObjectName);
        Start.Execute();
    }

    public GameObject GetConnectedObject() {
        return ConnectedObject;
    }
}