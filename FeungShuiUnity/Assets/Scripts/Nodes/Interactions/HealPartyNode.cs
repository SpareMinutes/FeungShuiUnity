using UnityEngine;

[CreateNodeMenu("Interactions/Actions/Heal Party")]
public class HealPartyNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool next;

    public override void Execute(GameObject context) {
        Battle player = GameObject.Find("WalkableCharacter").GetComponent<Battle>();
        foreach(Creature spirit in player.Party) {
            spirit.currentActiveHealth = spirit.getMaxActiveHealth();
            spirit.currentMana = spirit.getMaxMana();
            //Todo: clear status condition
        }
        ExecuteNext(GetOutputPort("next"), context);
    }
}