using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Interactions/Give Spirit")]
public class GiveSpiritNode : InteractionNode {
    public Species species;
    public int level;
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool success, noRoom;

    public override void Execute(GameObject context) {
        Battle player = GameObject.Find("WalkableCharacter").GetComponent<Battle>();
        if (player.Party.Count < 6) {
            player.Party.Add(species.Spawn(level));
            ExecuteNext(GetOutputPort("success"), context);
        } else {
            ExecuteNext(GetOutputPort("noRoom"), context);
        }
    }
}