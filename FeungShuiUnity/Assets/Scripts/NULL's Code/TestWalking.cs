﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWalking : MonoBehaviour
{
    public float speed; // movement speed multiplier
    public Animator animator;

    private float HorizontalDirection;
    private float VerticalDirection;


    void Update () {
        //get inputs (controller or keyboard)
        HorizontalDirection = Input.GetAxis("Horizontal");
        VerticalDirection = Input.GetAxis("Vertical");
        Debug.Log("Horizotnal");
        Debug.Log(HorizontalDirection);
        Debug.Log("Vertical");
        Debug.Log(VerticalDirection);

        // seting up the animator settings
        animator.SetBool("isWalking",HorizontalDirection != 0 || VerticalDirection != 0); 
        animator.SetFloat("HorizontalSpeed", HorizontalDirection);
        animator.SetFloat("VerticalSpeed", VerticalDirection);

        float angle;
        if (HorizontalDirection == 0){
            angle = 90;
        } else {
            angle = Mathf.Abs(Mathf.Atan(VerticalDirection/HorizontalDirection));
        }

        //moving the player character around
        gameObject.transform.position = new Vector2(gameObject.transform.position.x + HorizontalDirection*speed*Time.fixedDeltaTime*Mathf.Cos(angle),
            gameObject.transform.position.y + VerticalDirection*speed*Time.fixedDeltaTime*Mathf.Sin(angle));
    }
}
