using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour{
    private float Karma;

    void Start(){
        Karma = 0;
    }

    //True is a beneficial result
    public bool nextBool(float p) {
        if (Karma > 1) {
            Debug.Log("Good karma");
            Karma--;
            return true;
        }else if(Karma < -1) {
            Debug.Log("Bad karma");
            Karma++;
            return false;
        }
        bool result = Random.Range(0, 1) >= p;
        Karma += p;
        if (result)
            Karma--;

        return result;
    }
}
