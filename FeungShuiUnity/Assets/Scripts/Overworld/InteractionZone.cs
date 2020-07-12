using UnityEngine;

public class InteractionZone : Interaction {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "WalkableCharacter") {
            RunStep();
        }
    }
}