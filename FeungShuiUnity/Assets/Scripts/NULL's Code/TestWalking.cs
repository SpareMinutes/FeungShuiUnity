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
        //comment out whichever one you dont want to use (testing purposes)
        //OmnidirectionalMovement();
        StiffMovement();
    }

    private void OmnidirectionalMovement() {
        //get inputs (controller or keyboard)
        HorizontalDirection = Input.GetAxis("Horizontal")*Mathf.Abs(Input.GetAxisRaw("Horizontal")); //extra bit for less floatiness
        VerticalDirection = Input.GetAxis("Vertical")*Mathf.Abs(Input.GetAxisRaw("Vertical")); //extra bit for less floatiness

        // seting up the animator settings
        animator.SetBool("isWalking",HorizontalDirection != 0 || VerticalDirection != 0); 
        animator.SetFloat("HorizontalSpeed", HorizontalDirection);
        animator.SetFloat("VerticalSpeed", VerticalDirection);

        //angle is to account for speed difference in the angles of movement not parallel to the XY plane
        float angle;
        if (HorizontalDirection == 0){
            angle = 90;
        } else {
            angle = Mathf.Abs(Mathf.Atan(VerticalDirection/HorizontalDirection));
        }

        float newX = HorizontalDirection*speed*Time.deltaTime*Mathf.Cos(angle);
        float newY = VerticalDirection *speed *Time.deltaTime*Mathf.Sin(angle);

        //moving the player character around
        gameObject.transform.position += new Vector3(newX, newY, gameObject.transform.position.z);
    }

    private void StiffMovement () {
        // will only have 4 directions of movement ==> Up, Down, Left, Right

        //if you press down Up then the character moves UP, etc...
        // if you press Horizontal while already holding down Vertical, then it should go HORIZONTAL
        // if you press Vertical while already holding down Horizontal, then it should go VERTICAL

        //only one input should be accepted at a time

        //either 1 or 0 or -1 in a given direction
        HorizontalDirection = Input.GetAxisRaw("Horizontal");
        VerticalDirection = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(HorizontalDirection) == 1) {
            //means that Left/Right keys are applying input
            //so we want to disable the vertical keys from applying input
            VerticalDirection = 0.0f;
        } else if (Mathf.Abs(VerticalDirection) == 1) {
            //means that Up/Down keys are applying input
            //so the horizontal should be disabled from applying input
            HorizontalDirection = 0.0f;
        }
        //^doesnt quite work becuase it has bias to the horizontal movement (ie if holding down vertical, when you press horizontal it will go that direction)
        // and if holding down horizontal and press vertical it will still go horizontal

        // seting up the animator settings
        animator.SetBool("isWalking",HorizontalDirection != 0 || VerticalDirection != 0); 
        animator.SetFloat("HorizontalSpeed", HorizontalDirection);
        animator.SetFloat("VerticalSpeed", VerticalDirection);
        
        //no angle buisness so dont have to worry about that
        float newX = HorizontalDirection*speed*Time.deltaTime;
        float newY = VerticalDirection *speed *Time.deltaTime;

        //moving the player character around
        gameObject.transform.position += new Vector3(newX, newY, gameObject.transform.position.z);
    }
}
