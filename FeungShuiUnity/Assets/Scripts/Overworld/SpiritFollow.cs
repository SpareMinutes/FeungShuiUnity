using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritFollow : MonoBehaviour{
    public GameObject player, other;
    public float followDist, separateDist, speed;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        Vector2 separation = player.transform.position - transform.position;
        if(separation.magnitude < followDist) {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        } else {
            separation.Normalize();
            GetComponent<Rigidbody2D>().velocity = separation * speed;
        }

        separation = other.transform.position - transform.position;
        if (separation.magnitude < separateDist) {
            separation.Normalize();
            GetComponent<Rigidbody2D>().velocity -= separation * (speed/4);
        }
    }
}
