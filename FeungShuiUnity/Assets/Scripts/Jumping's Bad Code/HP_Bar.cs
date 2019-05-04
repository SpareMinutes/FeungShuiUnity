using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HP_Bar : MonoBehaviour
{
    // shows HP bar at 40 percent HP
    void Start()
    {
        Transform bar = transform.Find("Bar");
        bar.localScale = new Vector3(.4f, 1f);
    }






    //for when I (Jumpingbeans) know how to use unity
    /*private Transform bar;

    void Start()
    {
        bar = transform.Find("Bar");

    }

    // Update is called once per frame
    
    public void SetSize(float sizeNormalized){
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }*/
}
