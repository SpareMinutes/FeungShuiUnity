using System.Collections;
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
        area.transform.rotation = Quaternion.Euler(0, 0, animator.GetInteger("angle") + 45);
        if (Input.GetButtonUp("Submit")){
            List<Collider2D> lookedAt = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;
            area.GetComponent<PolygonCollider2D>().OverlapCollider(filter, lookedAt);
            foreach(Collider2D coll in lookedAt){
                EventTrigger e = coll.GetComponent<EventTrigger>();
                if (e != null) {
                    e.onInteract.Invoke();
                    break;
                }
            }
        }

    }


}
