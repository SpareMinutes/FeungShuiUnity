using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundAI : MonoBehaviour{
    public AIGraph graph;

    // Update is called once per frame
    void Update(){
        graph.Execute(gameObject);
    }
}
