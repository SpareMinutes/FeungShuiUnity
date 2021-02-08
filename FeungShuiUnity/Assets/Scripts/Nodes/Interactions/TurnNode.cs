using System;
using UnityEngine;

[CreateNodeMenu("Interactions/Animation/Turn")]
public class TurnNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool next;
    [Tooltip("The object to turn. Only needed if not the same as the connected object.")]
    public string ObjectName;
    [Tooltip("An integer angle in degrees or the name of another object.")]
    public string Direction;

    public override void Execute(GameObject context) {
        GameObject obj = ObjectName.Equals("") ? context : GameObject.Find(ObjectName);
        int dir;
        try {
            dir = Int32.Parse(Direction);
        }catch(FormatException e) {
            Vector3 vector = GameObject.Find(Direction).transform.position - obj.transform.position;
            dir = 135 - (int)(Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg);
        }
        obj.GetComponent<Animator>().SetInteger("angle", dir);
        ExecuteNext(GetOutputPort("next"), context);
    }
}