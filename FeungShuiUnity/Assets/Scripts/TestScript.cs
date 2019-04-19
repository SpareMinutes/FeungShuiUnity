using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public float timeBetweemSpawns =2;
    public GameObject toSpawn;


    void Update () {
        
        if (Input.GetKeyDown(KeyCode.Z)) {
            float newX = Random.Range((float)-0.55,0);
            float newY = Random.Range((float)-0.3,(float)0.3);
            Instantiate(toSpawn,new Vector3(newX,newY,0),this.transform.localRotation);
        }
    }
}
