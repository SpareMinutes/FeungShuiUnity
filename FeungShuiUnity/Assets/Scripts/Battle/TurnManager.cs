using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour{   
    [SerializeField]
    private List<CreatureBattleStatusController> takeTurns;
    private int turnIndex = 0;
    private Queue<CreatureBattleStatusController> Upcoming;

    public void Init(){
        sortBySpeed();
        Upcoming = new Queue<CreatureBattleStatusController>();
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
            //Debug.Log(pair.Key.getSpeed());
            returnList.Add(pair.Key);
            //Debug.Log(pair.Key.displayName);
        }
        takeTurns =  returnList;
    }

    public void CalcUpcoming(){
        while (Upcoming.Count() < 20){
            turnIndex++;
            foreach(CreatureBattleStatusController CreatureBattleStatusController in takeTurns){
                //To do: get a better formula
                if (turnIndex % (1000 - CreatureBattleStatusController.getSpeed()) == 0){
                    Upcoming.Enqueue(CreatureBattleStatusController);
                }
            }
        }
    }

    public List<CreatureBattleStatusController> getActivePlayerControlled () {
        List<CreatureBattleStatusController> dummyList = new List<CreatureBattleStatusController>();
        foreach (CreatureBattleStatusController CreatureBattleStatusController in takeTurns) {
            if (CreatureBattleStatusController.GetCreature().isPlayerOwned()) {
                dummyList.Add(CreatureBattleStatusController);
            }
        }
        return dummyList;
    }

    public List<CreatureBattleStatusController> getActiveEnemies () {
        List<CreatureBattleStatusController> dummyList = new List<CreatureBattleStatusController>();
        foreach (CreatureBattleStatusController CreatureBattleStatusController in takeTurns) {
            if (!CreatureBattleStatusController.GetCreature().isPlayerOwned()) {
                dummyList.Add(CreatureBattleStatusController);
            }
        }
        return dummyList;
    }

    public List<CreatureBattleStatusController> getAllActive () {
        return takeTurns;
    }

    public CreatureBattleStatusController getNextSpirit () {
        CreatureBattleStatusController next = Upcoming.Dequeue();
        //Debug.Log(next.name);
        CalcUpcoming();
        return next;
    }

    public void removeFromPlay (CreatureBattleStatusController CreatureBattleStatusController) {
        takeTurns.Remove(CreatureBattleStatusController);
        Upcoming = new Queue<CreatureBattleStatusController>();
        sortBySpeed();
        CalcUpcoming();
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
