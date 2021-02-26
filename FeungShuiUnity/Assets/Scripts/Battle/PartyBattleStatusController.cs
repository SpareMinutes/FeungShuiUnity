using System;
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
        //Init indices
        ActiveIndices = new int[2];
        ActiveIndices[0] = -1;
        ActiveIndices[1] = -1;
        //Find healthy party members
        int checkIndex = 0;
        int successes = 0;
        while (checkIndex < Party.Count && successes < 2) {
            if(Party[checkIndex].currentActiveHealth > 0) {
                ActiveIndices[successes] = checkIndex;
                successes++;
            }
            checkIndex++;
        }
        //Fill in the active party
        CreatureStatus1.SetTarget(ActiveIndices[0] >= 0 ? Party[ActiveIndices[0]] : null);
        CreatureStatus2.SetTarget(ActiveIndices[1] >= 0 ? Party[ActiveIndices[1]] : null);
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
            int healthyCount = 0;
            foreach (Creature member in Party)
                if (member.currentActiveHealth > 0)
                    healthyCount++;
            if (healthyCount > 1)
                GameObject.Find("EventSystem").GetComponent<BattleMenu>().SelectSpirits(CreatureStatus);
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

    public bool IsCreatureActive(Creature targetCreature) {
        return (CreatureStatus1.Target!=null && CreatureStatus1.Target.Equals(targetCreature)) || (CreatureStatus2.Target != null && CreatureStatus2.Target.Equals(targetCreature));
    }
}
