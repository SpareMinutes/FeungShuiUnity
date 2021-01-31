using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyBattleStatusController : MonoBehaviour{
    public CreatureBattleStatusController CreatureStatus1, CreatureStatus2;
    public Image[] OverviewIndicators;
    public bool IsPlayer;
    public Sprite noneSprite, healthySprite, faintedSprite;

    List<Creature> Party;
    int[] ActiveIndices;

    public void Begin(List<Creature> PartyIn) {
        Party = PartyIn;
        ActiveIndices = new int[2];
        ActiveIndices[0] = 0;
        CreatureStatus1.SetTarget(Party[0]);
        if (Party.Count > 1) {
            CreatureStatus2.SetTarget(Party[1]);
            ActiveIndices[1] = 1;
        } else {
            CreatureStatus2.SetTarget(null);
            ActiveIndices[1] = -1;
        }
        UpdateIndicators();
    }

    public void UpdateIndicators() {
        for (int i = 0; i < 6; i++) {
            if (i >= Party.Count || Party[i].currentCriticalHealth <= 0) {
                OverviewIndicators[i].sprite = noneSprite;
                OverviewIndicators[i].color = Color.white;
            } else if (Party[i].currentActiveHealth > 0) {
                OverviewIndicators[i].sprite = healthySprite;
                OverviewIndicators[i].color = Color.green;
            } else {
                OverviewIndicators[i].sprite = faintedSprite;
                OverviewIndicators[i].color = Color.grey;
            }
        }
    }

    public void ChooseNew(CreatureBattleStatusController CreatureStatus) {
        if (IsPlayer) {

        } else {
            //Todo: make this be based on AI
            for(int i=0; i< Party.Count; i++) {
                if (!Party[i].Equals(Party[ActiveIndices[0]]) && !Party[i].Equals(Party[ActiveIndices[1]]) && Party[i].currentActiveHealth > 0) {
                    ActiveIndices[CreatureStatus.index] = i;
                    CreatureStatus.SetTarget(Party[i]);
                    break;
                }
            }
        }

        UpdateIndicators();
    }

    public void Remove(CreatureBattleStatusController CreatureStatus) {
        Creature dead = Party[ActiveIndices[CreatureStatus.index]];
        ChooseNew(CreatureStatus);
        Party.Remove(dead);
    }

    public List<CreatureBattleStatusController> GetActive(){
        List<CreatureBattleStatusController> Active = new List<CreatureBattleStatusController>();
        if (CreatureStatus1.Target != null) Active.Add(CreatureStatus1);
        if (CreatureStatus2.Target != null) Active.Add(CreatureStatus2);
        return Active;
    }
}
