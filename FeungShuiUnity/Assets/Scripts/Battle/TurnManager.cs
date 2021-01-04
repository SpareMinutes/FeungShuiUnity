using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour{
    [SerializeField]
    private List<CreatureBattleStatusController> takeTurns;
    [SerializeField]
    private Text[] TurnLabels;
    private List<CreatureBattleStatusController> remove;
    private int turnIndex = 0;
    private Queue<CreatureBattleStatusController> Upcoming;

    public void Init(){
        sortBySpeed();
        Upcoming = new Queue<CreatureBattleStatusController>();
        remove = new List<CreatureBattleStatusController>();
        CalcUpcoming();
    }

    public void sortBySpeed () {
        //this function will just reorder the spirits for the battling order in terms of speed
        List<CreatureBattleStatusController> returnList = new List<CreatureBattleStatusController>();
        Dictionary<CreatureBattleStatusController, float> speedMapping = new Dictionary<CreatureBattleStatusController, float>();

        foreach (CreatureBattleStatusController creature in takeTurns) {
            speedMapping.Add(creature, creature.getSpeed());
        }

        var items = from pair in speedMapping orderby pair.Value descending select pair;

        foreach (KeyValuePair<CreatureBattleStatusController, float> pair in items) {
            returnList.Add(pair.Key);
        }
        takeTurns =  returnList;
    }

    public void CalcUpcoming(){
        if (remove.Count() != 0) {
            Queue<CreatureBattleStatusController> NowUpcoming = new Queue<CreatureBattleStatusController>();
            while (Upcoming.Count() > 0) {
                CreatureBattleStatusController creature = Upcoming.Dequeue();
                if (!remove.Contains(creature))
                    NowUpcoming.Enqueue(creature);
            }
            remove.Clear();
        }
        while (Upcoming.Count() < 6){
            turnIndex++;
            foreach(CreatureBattleStatusController creature in takeTurns){
                //To do: get a better formula
                if (turnIndex % (1000 - creature.getSpeed()) == 0){
                    Upcoming.Enqueue(creature);
                }
            }
        }
        for(int i=0; i<5; i++) {
            TurnLabels[i].text = Upcoming.ElementAt<CreatureBattleStatusController>(i).Target.name;
        }
    }

    public List<CreatureBattleStatusController> getActivePlayerControlled () {
        List<CreatureBattleStatusController> dummyList = new List<CreatureBattleStatusController>();
        foreach (CreatureBattleStatusController creature in takeTurns) {
            if (creature.GetCreature().playerOwned) {
                dummyList.Add(creature);
            }
        }
        return dummyList;
    }

    public List<CreatureBattleStatusController> getActiveEnemies () {
        List<CreatureBattleStatusController> dummyList = new List<CreatureBattleStatusController>();
        foreach (CreatureBattleStatusController creature in takeTurns) {
            if (!creature.GetCreature().playerOwned) {
                dummyList.Add(creature);
            }
        }
        return dummyList;
    }

    public List<CreatureBattleStatusController> getAllActive () {
        return takeTurns;
    }

    public CreatureBattleStatusController getNextSpirit () {
        CalcUpcoming();
        CreatureBattleStatusController next = Upcoming.Dequeue();
        return next;
    }

    public void removeFromPlay (CreatureBattleStatusController creature) {
        takeTurns.Remove(creature);
        remove.Add(creature);
    }

    public string whoWins () {
        if (getActivePlayerControlled().Count == 0) {
            //CPU wins
            return "Computer";
        } else if(getActiveEnemies().Count == 0) {
            // player wins
            return "Player";
        }  else {
            //no one wins
            return "No-one";
        }
    }
}
