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
        List<Creature> dummyList = new List<Creature>(){};;
        foreach (Creature creature in takeTurns) {
            if (creature.isPlayerOwned() && creature.getActiveHealth() > 0) {
                dummyList.Add(creature);
            }
        }
        return dummyList;
    }

    public Creature getNextSpirit () {
        turnIndex += 1;
        return takeTurns[turnIndex%takeTurns.Count];
    }

    public void checker () {
        foreach (Creature creature in takeTurns) {
            if (creature.getActiveHealth() <= 0) {
                //remove that creature from the list so it can't make more moves
                takeTurns.Remove(creature);
            }
        }
    }
}
