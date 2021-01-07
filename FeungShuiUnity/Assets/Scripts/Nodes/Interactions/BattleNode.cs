using UnityEngine;

[CreateNodeMenu("Interactions/Battle")]
public class BattleNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool onVictory, onDefeat;

    public override void Execute(GameObject context) {
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().disableButton();
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().SetActiveNode(this);
        context.GetComponent<Battle>().StartTrainerBattle();
    }

    public void Finish(bool result, GameObject context) {
        ExecuteNext(GetOutputPort(result ? "onDefeat" : "onVictory"), context);
    }
}