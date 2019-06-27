using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager2 : MonoBehaviour
{
    public List<Creature> takeTurns;

    private int turnIndex = 0;

    public void Start () {
        takeTurns = sortBySpeed();
    }

    private List<Creature> sortBySpeed () {
        //this function will just reorder the spirits for the battling order in terms of speed
        List<Creature> returnList = new List<Creature>();
        Dictionary<Creature, float> speedMapping = new Dictionary<Creature, float>();

        foreach (Creature creature in takeTurns) {
            speedMapping.Add(creature, creature.getSpeed());
        }

        var items = from pair in speedMapping orderby pair.Value ascending select pair;

        foreach (KeyValuePair<Creature, float> pair in items) {
            //Debug.Log(pair.Key.getSpeed());
            returnList.Add(pair.Key);
        }
        return returnList;
        //
        //
        // this needs to be rewritten becuase we changed the way turn order is calculated
        //
        //
    }

    public Creature getNextSpirit () {
        turnIndex += 1;
        return takeTurns[turnIndex%takeTurns.Count];
    }
}
