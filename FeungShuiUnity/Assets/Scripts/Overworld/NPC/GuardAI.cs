using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAI : MonoBehaviour{
    [Tooltip("The zone which starts aggro when entered by the player. May or may not be the same as Relax Zone.")]
    public Collider2D AggroZone;
    [Tooltip("The zone which ends aggro when exited by the player. Must completely contain Aggro Zone.")]
    public Collider2D RelaxZone;
    private bool Aggroed;

    public void Update() {
        if (AggroZone.IsTouching(GameObject.Find("WalkableCharacter").GetComponent<CapsuleCollider2D>()))
            Aggroed = true;
        if (!RelaxZone.IsTouching(GameObject.Find("WalkableCharacter").GetComponent<CapsuleCollider2D>()))
            Aggroed = false;
    }

    public bool isAggroed() {
        return Aggroed;
    }
}
