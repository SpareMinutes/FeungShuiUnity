using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWalking : MonoBehaviour
{
    public float speed; // movement speed multiplier

    private float HorizontalDirection;
    private float VerticalDirection;

    void Update () {
        //checks for input

        //moves the player
        SmoothMovement();
    }

    void StiffMovement () {
        // only move in set directions (N,E,S,W,(NE,SE,SW,NW)?)

        
    }

    void SmoothMovement () {
        // can move in any direction
        HorizontalDirection = Input.GetAxisRaw("Horizontal");
        VerticalDirection = Input.GetAxisRaw("Vertical");
        gameObject.transform.position = new Vector2(gameObject.transform.position.x + HorizontalDirection*speed*Time.fixedDeltaTime,
            gameObject.transform.position.y + VerticalDirection*speed*Time.fixedDeltaTime);
    }
}
