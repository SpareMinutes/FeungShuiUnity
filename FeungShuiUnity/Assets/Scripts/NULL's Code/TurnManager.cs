using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TurnManager : MonoBehaviour
{
    //for the sake of testing, i'll add the gameObjects manually but in the real thing it should be automatic
    public List<Creature> battlingSpirits;
    public Creature turnSpirit;

    void Start () {
        //starts the turn cycle
        //takeTurn();
        foreach (Creature creature in sortBySpeed()) {
            Debug.Log(creature.displayName);
        }
    }

    public void takeTurn () {
        foreach (Creature creature in sortBySpeed()) {
            if (creature.getActiveHealth() <= 0) {
                //the current creatures active health is below 0 and therefore they must faint and not take their turn
                //float excessDamage = [damage taken] - [previous current health for that creature]
                Debug.Log("they dead");
            }
            creature.takeTurn();
        }


    }

    private List<Creature> sortBySpeed () {
        //this function will just reorder the spirits for the battling order in terms of speed
        List<Creature> returnList = new List<Creature>();
        Dictionary<Creature, float> speedMapping = new Dictionary<Creature, float>();

        foreach (Creature creature in battlingSpirits) {
            speedMapping.Add(creature, creature.getSpeed());
        }

        var items = from pair in speedMapping orderby pair.Value ascending select pair;

        foreach (KeyValuePair<Creature, float> pair in items) {
            //Debug.Log(pair.Key.getSpeed());
            returnList.Add(pair.Key);
        }

        return returnList;
    }

    //Hard coded for testing
    public Creature GetTurnCreature(){
        return turnSpirit;
    }
}
