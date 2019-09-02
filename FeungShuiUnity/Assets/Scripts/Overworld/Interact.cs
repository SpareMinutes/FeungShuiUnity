﻿using System.Collections.Generic;
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
        area.transform.rotation = Quaternion.Euler(0, 0, animator.GetInteger("angle") + 45);

        //press enter
        if (Input.GetButtonDown("Submit")){
            Debug.Log("attempted to interact");

            List<Collider2D> lookedAt = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;
            area.GetComponent<PolygonCollider2D>().OverlapCollider(filter, lookedAt);
            Debug.Log(lookedAt.Count);
            foreach(Collider2D coll in lookedAt){
                Debug.Log(coll.gameObject.name);
                EventTrigger e = coll.GetComponent<EventTrigger>();
                if (e != null && coll.gameObject.tag.Equals("Interactable")) {
                    e.onInteract.Invoke();
                    break;
                }
            }
            Debug.Log("\n");
        }

    }


}
