using System;
using UnityEngine;

[CreateNodeMenu("Interactions/Flow Control/Start"), NodeTint(100, 200, 100)]
public class StartNode : InteractionNode {
    [Output] public bool next;
    public bool PauseMovement = true;
    public bool FacePlayer = true;

    public override void Execute(GameObject context) {
        try {
            context.GetComponent<WanderAI>().enabled = false;
            Vector3 dir = GameObject.Find("WalkableCharacter").transform.position - context.transform.position;
            context.GetComponent<Animator>().SetBool("isWalking", false);
            context.GetComponent<Animator>().SetInteger("angle", 135 - (int)(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));
        } catch (NullReferenceException e) {

        }
        ExecuteNext(GetOutputPort("next"), context);
    }
}