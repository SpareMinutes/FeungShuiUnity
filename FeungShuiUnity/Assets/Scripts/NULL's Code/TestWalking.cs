using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWalking : MonoBehaviour
{
    public float speed; // movement speed multiplier
    public Animator animator;

    private float HorizontalDirection;
    private float VerticalDirection;
    private bool walkingHorizontally, walkingVertically;

    void Start() {
        walkingHorizontally = false;
        walkingVertically = false;
    }

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
        animator.SetBool("isWalking", HorizontalDirection != 0 || VerticalDirection != 0); 
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
        //plyayer should move in the direction of the most recent input, regardless of other inputs

        //either 1 or 0 or -1 in a given direction
        HorizontalDirection = walkingVertically ? 0 : Input.GetAxisRaw("Horizontal");
        VerticalDirection = walkingHorizontally ? 0 : Input.GetAxisRaw("Vertical");

        walkingHorizontally = HorizontalDirection != 0;
        walkingVertically = VerticalDirection != 0;

        if (Mathf.Abs(HorizontalDirection) == 1) {
            //horizontal input
            VerticalDirection = 0.0f;
        } else if (Mathf.Abs(VerticalDirection) == 1) {
            //vertical input
            HorizontalDirection = 0.0f;
        }

        // seting up the animator settings
        animator.SetBool("isWalking",HorizontalDirection != 0 || VerticalDirection != 0); 
        animator.SetFloat("HorizontalSpeed", HorizontalDirection);
        animator.SetFloat("VerticalSpeed", VerticalDirection);

        //moving the player character around
        float newX = HorizontalDirection*speed*Time.deltaTime;
        float newY = VerticalDirection *speed *Time.deltaTime;

        //player movement
        gameObject.transform.position += new Vector3(newX, newY, 0);

    }
}
