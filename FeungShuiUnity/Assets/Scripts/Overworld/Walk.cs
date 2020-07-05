using UnityEngine;

public class Walk : MonoBehaviour{
    [SerializeField]
    private float speed; //movement speed multiplier
    [SerializeField]
    private Animator animator;
    
    public bool canWalk = true;

    private float HorizontalDirection;
    private float VerticalDirection;
    private bool walkingHorizontally, walkingVertically;

    void Start() {
        if (PersistentStats.PlayerHasMoved) {
            gameObject.transform.position = new Vector3(PersistentStats.PlayerPosX, PersistentStats.PlayerPosY, -1);
            gameObject.GetComponent<Animator>().SetInteger("angle", PersistentStats.PlayerRotation);
            PersistentStats.PlayerHasMoved = false;
        } else if (PersistentStats.SceneChanged) {
            gameObject.transform.position = new Vector3(PersistentStats.SceneChangePosX, PersistentStats.SceneChangePosY, -1);
            gameObject.GetComponent<Animator>().SetInteger("angle", 90);
            PersistentStats.SceneChanged = false;
        }

        walkingHorizontally = false;
        walkingVertically = false;
    }

    void FixedUpdate () {
        //comment out whichever one you dont want to use (testing purposes)
        if (canWalk) {
            OmnidirectionalMovement();
            //StiffMovement();
        } else {
            //set movement to zero to stop the player moving in the menu
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3 (0,0,0);
            //set animator to idle animation instead of walking
            animator.SetBool("isWalking", false);
        }
    }

    private void OmnidirectionalMovement() {
        //get inputs (controller or keyboard)
        HorizontalDirection = Input.GetAxisRaw("Horizontal");
        VerticalDirection = Input.GetAxisRaw("Vertical");
        Vector2 Direction = new Vector2(HorizontalDirection, VerticalDirection);

        //normalize keyboard inputs
        if (Direction.magnitude > 1){
            Direction /= Direction.magnitude;
        }

        float angle = Mathf.Atan2(Direction.x, Direction.y);

        //seting up the animator settings
        animator.SetBool("isWalking", Direction.magnitude>0);
        if(Direction.magnitude > 0)
            animator.SetInteger("angle", (int)(Mathf.Rad2Deg*angle)+45);

        //moving the player character around
        gameObject.GetComponent<Rigidbody2D>().velocity = Direction * speed * Time.fixedDeltaTime * 640;
    }

    private void StiffMovement () {
        //will only have 4 directions of movement ==> Up, Down, Left, Right
        //plyayer should move in the direction of the most recent input, regardless of other inputs

        //either 1 or 0 or -1 in a given direction
        HorizontalDirection = walkingVertically ? 0 : Input.GetAxisRaw("Horizontal");
        VerticalDirection = walkingHorizontally ? 0 : Input.GetAxisRaw("Vertical");

        walkingHorizontally = HorizontalDirection != 0;
        walkingVertically = VerticalDirection != 0;

        //seting up the animator settings
        //will need to be updated if we ever switch back to this movement scheme
        animator.SetBool("isWalking",HorizontalDirection != 0 || VerticalDirection != 0); 
        animator.SetFloat("HorizontalSpeed", HorizontalDirection);
        animator.SetFloat("VerticalSpeed", VerticalDirection);

        //moving the player character around
        float newX = HorizontalDirection*speed*Time.fixedDeltaTime*640;
        float newY = VerticalDirection *speed *Time.fixedDeltaTime*640;

        //player movement
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(newX, newY, 0);

    }
}
