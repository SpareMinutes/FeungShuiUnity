using System.Collections.Generic;
using UnityEngine;

//Handles Creature status for reference in parties
[System.Serializable]
public class Creature{
    //name of the creature, will default to species name if none is given
    public string displayName;
    public string speciesName;
    private Species species;

    //stats
    public float maxActiveHealth;
    public float maxCriticalHealth;
    public float maxMana;
    //dont need to edit these in the editor, but they are needed by other scripts so theyll stay public
    [HideInInspector] 
    public float currentCriticalHealth;
    [HideInInspector]
    public float currentActiveHealth;
    [HideInInspector]
    public float currentMana;

    private int attack;
    private int defense;
    private int intelligence; //Special attack
    private int resistance; //Special defense
    private int speed;
     
    private float friendship = 0; //0 should be neutral, and less <0 is bad >0 is good

    //xp stuff
    public float totalExp = 0;

    //misc stats
    public bool playerOwned;
    private Type type; //only single type for now
    private string personality;
    private List<float> statModifiers;
    public List<Move> Moves;

    private static Dictionary<Creature, float> friendshipDict = new Dictionary<Creature, float>();
    private float trainerFriendship; // because the player isnt of type Creature

    public void Init() {
        species = SpiritsTable.Find(speciesName);
        int level = getLevel();
        //TODO: factor in upgrade points and multipliers
        //0-health, 1-mana, 2-attack, 3-defense, 4-intelligence, 5-resistance, 6-speed
        int[] baseStats = species.getStats();
        maxActiveHealth = (int)Mathf.Floor((baseStats[0] * level / 50.0f) + 10);
        currentActiveHealth = maxActiveHealth;
        maxMana = (int)Mathf.Floor((baseStats[1] * level / 50.0f) + 5);
        attack = (int)Mathf.Floor((baseStats[2] * level / 50.0f) + 5);
        defense = (int)Mathf.Floor((baseStats[3] * level / 50.0f) + 5);
        intelligence = (int)Mathf.Floor((baseStats[4] * level / 50.0f) + 5);
        resistance = (int)Mathf.Floor((baseStats[5] * level / 50.0f) + 5);
        speed = (int)Mathf.Floor((baseStats[6] * level / 50.0f) + 5);
    }
    
    public int getLevel() {
        return (int)Mathf.Pow(totalExp,1/3f);
    }

    public Type getType() {
        return this.type;
    }

    public float getMaxActiveHealth() {
        return maxActiveHealth;
    }

    public float getActiveHealth() {
        return currentActiveHealth;
    }

    public float getMaxCriticalHealth() {
        return maxCriticalHealth;
    }

    public float getCriticalHealth () {
        return currentCriticalHealth;
    }

    public float getMaxMana() {
        return this.maxMana;
    }

    public float getMana() {
        return this.currentMana;
    }

    public int getAttack() {
        return this.attack;
    }

    public int getDefense() {
        return this.defense;
    }

    public int getIntelligence() {
        return this.intelligence;
    }

    public int getResistance() {
        return this.resistance;
    }

    public int getSpeed() {
        return this.speed;
    }

    public bool isPlayerOwned () {
        return playerOwned;
    }
}
