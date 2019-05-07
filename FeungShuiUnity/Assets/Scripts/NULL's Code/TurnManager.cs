using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    //for the sake of testing, i'll add the gameObjects manually but in the real thing it should be automatic
    public List<GameObject> battlingSpirits;
    
    private int turnNum = 0;

    void Start () {
        //starts the turn cycle
        Turn();
    }

    void Turn () {
        
        while (!Input.GetKeyDown(KeyCode.Space)) {
            //in this case player pressing the space bar will choose one move at random and play that move
        }

        Turn();
    }
}
