using UnityEngine;

public class BridgeCollide : MonoBehaviour {
    public int height;
    //The walls that are active when on the bridge
    public GameObject[] onWalls;
    //The walls that are active when off the bridge
    public GameObject[] offWalls;
    public GameObject player;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        bool onBridge = player.transform.position.z < (0-height);
        foreach(GameObject wall in onWalls) {
            EdgeCollider2D ec = (EdgeCollider2D)wall.GetComponent<EdgeCollider2D>();
            ec.enabled = onBridge;
        }
        foreach (GameObject wall in offWalls)
        {
            EdgeCollider2D ec = (EdgeCollider2D)wall.GetComponent<EdgeCollider2D>();
            ec.enabled = !onBridge;
        }
    }
}
