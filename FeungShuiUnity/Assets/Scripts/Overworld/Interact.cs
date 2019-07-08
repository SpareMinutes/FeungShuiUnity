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
        area.transform.rotation = Quaternion.Euler(0, 0, animator.GetFloat("angle") + 45);
    }
}
