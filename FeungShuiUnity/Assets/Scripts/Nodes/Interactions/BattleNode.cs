using UnityEngine;

[CreateNodeMenu("Interactions/Battle")]
public class BattleNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool onVictory, onDefeat;

    private GameObject context;

    public override void Execute(GameObject contextIn) {
        context = contextIn;

        GameObject.Find("EventSystem").GetComponent<OverworldUI>().disableButton();
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().SetActiveNode(this);
        context.GetComponent<Battle>().StartTrainerBattle();
    }

    public void Finish(bool result) {
        ExecuteNext(GetOutputPort(result ? "onDefeat" : "onVictory"), context);
    }
}