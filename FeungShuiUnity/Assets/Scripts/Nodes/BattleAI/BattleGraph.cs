using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class BattleGraph : NodeGraph {
    public StateNode startState;
    private StateNode state;
    private BattleMemory memory;

    public void Begin() {
        state = startState;
    }

    public bool isWild() {
        return memory.GetIsWild();
    }

    public void ChooseAction(GameObject context) {
        state.Execute(context);
    }

    public void selectAttack(int index) {

    }

    public void selectDefend() {
    }

    public void selectSwitch(int index) {

    }
}