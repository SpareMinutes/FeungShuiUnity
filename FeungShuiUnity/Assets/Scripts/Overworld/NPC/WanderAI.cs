using Pathfinding;
using UnityEngine;

public class WanderAI : SimpleAI {
    public Collider2D bounds;
    void Start() {
        base.Start();
        NextPath();
    }

    public override void NextPath() {
        path = null;
        rb.velocity = new Vector2(0, 0);
        Invoke("Move", 1 + Random.Range(0f, 0.5f) + Random.Range(0f, 0.5f));
    }

    void Move() {
        Vector2 dest;
        do {
            Vector2 offset = Random.insideUnitCircle * 100;
            dest = new Vector2(this.transform.position.x + offset.x, this.transform.position.y + offset.y);
        } while (bounds != null && !bounds.OverlapPoint(dest));
        Debug.Log("(" + dest.x + ", " + dest.y + ")");
        seeker.StartPath(rb.position, dest, OnPathComplete);
    }

    void OnPathComplete(Path p) {
        Debug.Log("Done");
        base.OnPathComplete(p);
        //Invoke("NextPath", Random.Range(0f, 0.5f) + Random.Range(0f, 0.5f));
    }
}
