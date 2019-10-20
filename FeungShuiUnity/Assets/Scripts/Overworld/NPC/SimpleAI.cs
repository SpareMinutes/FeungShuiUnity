using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class SimpleAI : MonoBehaviour{
    int currentWaypoint = 0;
    public float nextWaypointDistance;
    Path path;
    Seeker seeker;
    Rigidbody2D rb;
    Animator anim;

    void Start(){
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update(){
        if (path == null)
            return;
        if(currentWaypoint >= path.vectorPath.Count) {
            NextPath();
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        anim.SetInteger("angle", (int)(Mathf.Rad2Deg*Mathf.Atan2(direction.x, direction.y)));
        Vector2 force = direction * Time.deltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
            currentWaypoint++;
    }

    public abstract void NextPath();
}
