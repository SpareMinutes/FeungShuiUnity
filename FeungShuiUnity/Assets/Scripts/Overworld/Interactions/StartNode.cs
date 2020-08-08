using System;
using UnityEngine;

[CreateNodeMenu("Interactions/Start")]
public class StartNode : InteractionNode {
    [Output] public bool next;
    public bool PauseMovement = true;
    public bool FacePlayer = true;

    public override void Execute() {
        try {
            GameObject obj = ((InteractionGraph)graph).GetConnectedObject();
            obj.GetComponent<WanderAI>().enabled = false;
            Vector3 dir = GameObject.Find("WalkableCharacter").transform.position - obj.transform.position;
            obj.GetComponent<Animator>().SetBool("isWalking", false);
            obj.GetComponent<Animator>().SetInteger("angle", 135 - (int)(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));
        } catch (NullReferenceException e) {

        }
        ExecuteNext(GetOutputPort("next"));
    }
}