using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour{
    [SerializeField]
    private GameObject area;
    [SerializeField]
    private Animator animator;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        //controls where the interaction area is compared to the player (NESW)
        area.transform.rotation = Quaternion.Euler(0, 0, 225-animator.GetInteger("angle"));

        //press enter
        if (Input.GetButtonDown("Submit")){
            List<Collider2D> lookedAt = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;
            area.GetComponent<PolygonCollider2D>().OverlapCollider(filter, lookedAt);
            
            foreach(Collider2D coll in lookedAt){
                Interaction i = coll.GetComponent<Interaction>();
                //checking the object exists in-game and has the Interactable tag
                if (i != null && coll.gameObject.tag.Equals("Interactable")) {
                    i.Begin();
                    break;
                }
            }
        }

    }


}
