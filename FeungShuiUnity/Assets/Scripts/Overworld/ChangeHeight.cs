using UnityEngine;

public class ChangeHeight : MonoBehaviour {
    public int height;
    public GameObject player;

    void OnTriggerEnter2D(Collider2D other) {
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -0.1f - height);
    }
}
