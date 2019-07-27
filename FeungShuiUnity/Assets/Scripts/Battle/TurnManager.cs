using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour{   
    [SerializeField]
    private List<Creature> takeTurns;
    private int turnIndex = 0;
    private Queue<Creature> Upcoming;

    public void Init(){
        sortBySpeed();
        Upcoming = new Queue<Creature>();
        CalcUpcoming();
    }

    public void sortBySpeed () {
        //this function will just reorder the spirits for the battling order in terms of speed
        List<Creature> returnList = new List<Creature>();
        Dictionary<Creature, float> speedMapping = new Dictionary<Creature, float>();

        foreach (Creature creature in takeTurns) {
            speedMapping.Add(creature, creature.getSpeed());
        }

        var items = from pair in speedMapping orderby pair.Value descending select pair;

        foreach (KeyValuePair<Creature, float> pair in items) {
            //Debug.Log(pair.Key.getSpeed());
            returnList.Add(pair.Key);
            //Debug.Log(pair.Key.displayName);
        }
        takeTurns =  returnList;
    }

    public void CalcUpcoming(){
        while (Upcoming.Count() < 20){
            turnIndex++;
            foreach(Creature creature in takeTurns){
                //To do: get a better formula
                if (turnIndex % (1000 - creature.getSpeed()) == 0){
                    Upcoming.Enqueue(creature);
                }
            }
        }
    }

    public List<Creature> getActivePlayerControlled () {
        List<Creature> dummyList = new List<Creature>();
        foreach (Creature creature in takeTurns) {
            if (creature.isPlayerOwned()) {
                dummyList.Add(creature);
            }
        }
        return dummyList;
    }

    public List<Creature> getActiveEnemies () {
        List<Creature> dummyList = new List<Creature>();
        foreach (Creature creature in takeTurns) {
            if (!creature.isPlayerOwned()) {
                dummyList.Add(creature);
            }
        }
        return dummyList;
    }

    public List<Creature> getAllActive () {
        return takeTurns;
    }

    public Creature getNextSpirit () {
        Creature next = Upcoming.Dequeue();
        //Debug.Log(next.name);
        CalcUpcoming();
        return next;
    }

    public void removeFromPlay (Creature creature) {
        takeTurns.Remove(creature);
        Upcoming = new Queue<Creature>();
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
