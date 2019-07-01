using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{   
    [SerializeField]
    private List<Creature> takeTurns;
    private int turnIndex = -1;

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
        }
        takeTurns =  returnList;
        //
        //
        // this needs to be rewritten becuase we changed the way turn order is calculated
        //
        //
    }

    public List<Creature> getActivePlayerControlled () {
        List<Creature> dummyList = new List<Creature>(){};
        foreach (Creature creature in takeTurns) {
            if (creature.isPlayerOwned()) {
                dummyList.Add(creature);
            }
        }
        return dummyList;
    }

    public List<Creature> getActiveEnemies () {
        List<Creature> dummyList = new List<Creature>(){};
        foreach (Creature creature in takeTurns) {
            if (!creature.isPlayerOwned()) {
                dummyList.Add(creature);
            }
        }
        return dummyList;
    }

    public Creature getNextSpirit () {
        turnIndex += 1;
        return takeTurns[turnIndex%takeTurns.Count];
    }

    public void removeFromPlay (Creature creature) {
        takeTurns.Remove(creature);
    }

    public string whoWins () {
        if (getActiveEnemies().Count == 0) {
            // player wins
            return "Player";
        } else if (getActivePlayerControlled().Count == 0){
            //CPU wins
            return "Computer";
        } else {
            //no one wins
            return "No-one";
        }
    }
}
