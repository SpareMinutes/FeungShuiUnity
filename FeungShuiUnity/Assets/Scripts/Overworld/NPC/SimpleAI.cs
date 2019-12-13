using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class SimpleAI : MonoBehaviour{
    int currentWaypoint = 0;
    public float nextWaypointDistance;
    protected Path path;
    protected Seeker seeker;
    protected Rigidbody2D rb;
    Animator anim;

    protected void Start(){
        Debug.Log("Start");
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    protected void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update(){
        anim.SetBool("isWalking", rb.velocity.magnitude > 0);
        if (path == null)
            return;
        if(currentWaypoint >= path.vectorPath.Count) {
            NextPath();
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position);
        if(direction.magnitude > 30) {
            Debug.Log("Recalculating");
            path = seeker.StartPath(rb.position, path.vectorPath[path.vectorPath.Count - 1], OnPathComplete);
            return;
        }    
        direction = direction.normalized;
        int angle = (int)(Mathf.Rad2Deg * Mathf.Atan2(direction.x, direction.y)) + 45;
        if (angle < -90)
            angle += 360;
        anim.SetInteger("angle", angle);
        Vector2 force = direction * 50 ;
        rb.velocity = force;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
            currentWaypoint++;
    }

    public abstract void NextPath();
}
