using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanToFollow : MonoBehaviour{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private Rect bounds;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        this.transform.position = new Vector3(Mathf.Max(Mathf.Min(target.transform.position.x, bounds.xMax), bounds.xMin), Mathf.Max(Mathf.Min(target.transform.position.y, bounds.yMax), bounds.yMin), -10);
    }
}
