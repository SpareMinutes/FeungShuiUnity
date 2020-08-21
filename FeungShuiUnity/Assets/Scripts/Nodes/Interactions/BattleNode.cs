using UnityEngine;

[CreateNodeMenu("Interactions/Battle")]
public class BattleNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool onVictory, onDefeat;
    public StartNode onRematch, afterDefeat;

    public override void Execute(GameObject context) {
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().disableButton();
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().SetActiveNode(this);
        context.GetComponent<Battle>().StartTrainerBattle();
    }

    public void Finish(bool result, GameObject context) {
        ((InteractionGraph)graph).Start = result ? afterDefeat : onRematch;
        ExecuteNext(GetOutputPort(result ? "onDefeat" : "onVictory"), context);
    }
}