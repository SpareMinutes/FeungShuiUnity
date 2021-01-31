using UnityEngine;

public class Interaction : MonoBehaviour {
    public InteractionGraph graph;

    public void Begin() {
        graph.Execute(gameObject);
    }
}
