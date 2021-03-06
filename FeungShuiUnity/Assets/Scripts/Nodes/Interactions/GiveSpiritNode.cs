﻿using UnityEngine;

[CreateNodeMenu("Interactions/Actions/Give Spirit")]
public class GiveSpiritNode : InteractionNode {
    public Species species;
    public int level;
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool success, noRoom;

    public override void Execute(GameObject context) {
        Battle player = GameObject.Find("WalkableCharacter").GetComponent<Battle>();
        if (player.Party.Count < 6) {
            Creature spawnedCreature = new Creature(species, level);
            spawnedCreature.playerOwned = true;
            player.Party.Add(spawnedCreature);
            ExecuteNext(GetOutputPort("success"), context);
        } else {
            ExecuteNext(GetOutputPort("noRoom"), context);
        }
    }
}