using System;
using UnityEngine;

public class StartNode : InteractionNode {
    [Output] public bool next;
    public bool PauseMovement = true;
    public bool FacePlayer = true;

    public override void Execute() {
        ExecuteNext(GetOutputPort("next"));
        try {
            GameObject obj = graph.GetConnectedObject();
            obj.GetComponent<WanderAI>().enabled = false;
            Vector3 dir = GameObject.Find("WalkableCharacter").transform.position - obj.transform.position;
            obj.GetComponent<Animator>().SetBool("isWalking", false);
            obj.GetComponent<Animator>().SetInteger("angle", 135 - (int)(Mathf.Atan2(dir.y, dir.x)* Mathf.Rad2Deg));
        } catch(NullReferenceException e) {

        }
    }
}