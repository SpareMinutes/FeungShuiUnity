﻿
using UnityEngine;

public class BattleNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool onVictory, onDefeat;
    public StartNode onRematch, afterDefeat;
    public string Npc;

    public override void Execute() {
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().disableButton();
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().SetActiveNode(this);
        GameObject.Find(Npc).GetComponent<Battle>().StartTrainerBattle();
    }

    public void Finish(bool result) {
        ((InteractionGraph)graph).Start = result ? afterDefeat : onRematch;
        ExecuteNext(GetOutputPort(result ? "onDefeat" : "onVictory"));
    }
}