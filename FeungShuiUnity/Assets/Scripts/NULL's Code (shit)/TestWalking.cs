using System.Collections;
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
        HorizontalDirection = Input.GetAxisRaw("Horizontal");
        VerticalDirection = Input.GetAxisRaw("Vertical");

        // seting up the animator settings
        animator.SetBool("isWalking",HorizontalDirection != 0 || VerticalDirection != 0); 
        animator.SetFloat("HorizontalSpeed", HorizontalDirection);
        animator.SetFloat("VerticalSpeed", VerticalDirection);

        //moving the player character around
        gameObject.transform.position = new Vector2(gameObject.transform.position.x + HorizontalDirection*speed*Time.fixedDeltaTime,
            gameObject.transform.position.y + VerticalDirection*speed*Time.fixedDeltaTime);
    }
}
