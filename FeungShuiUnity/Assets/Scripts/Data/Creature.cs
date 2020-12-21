using System.Collections.Generic;
using UnityEngine;

//Handles Creature status for reference in parties
[System.Serializable]
public class Creature {
    #region Info and trait variables
    System.Guid uid;
    public bool playerOwned;
    public string name;
    public Species species;
    Nature nature;
    //The amount this spicific creature differs from others of its level and species
    //0-exp, 1-health, 2-attack, 3-defense, 4-intelligence, 5-resistance, 6-speed
    int[] scaleFactors;
    public List<Move> Moves;
    int totalExp;
    #endregion

    #region Stat variables
    //dont need to edit these in the editor, but they are needed by other scripts so theyll stay public
    [HideInInspector]
    public float maxActiveHealth;
    [HideInInspector]
    public float maxCriticalHealth;
    [HideInInspector]
    public float maxMana;

    [HideInInspector]
    public float currentCriticalHealth;
    [HideInInspector]
    public float currentActiveHealth;
    [HideInInspector]
    public float currentMana;

    //convenience variables of actual stats calculated from species, nature, etc
    private int attack;
    private int defense;
    private int intelligence; //Special attack
    private int resistance; //Special defense
    private int speed;

    private List<float> statModifiers;

    private static Dictionary<Creature, float> friendshipDict = new Dictionary<Creature, float>();
    private float trainerFriendship; // because the player isnt of type Creature
    #endregion

    public Creature(Species spIn, Nature natIn, int[] factorsIn, int startLevel) {
        species = spIn;
        nature = natIn;
        scaleFactors = factorsIn;
        uid = System.Guid.NewGuid();
        totalExp = (int)(scaleFactors[0] * Mathf.Pow(startLevel, 3));

        UpdateStats();

        currentActiveHealth = maxActiveHealth;
        currentCriticalHealth = maxCriticalHealth;
        currentMana = maxMana;
    }

    public void UpdateStats() {
        int level = getLevel();
        //TODO: factor in upgrade points and multipliers
        //0-health, 1-mana, 2-attack, 3-defense, 4-intelligence, 5-resistance, 6-speed
        int[] baseStats = species.getStats();
        maxActiveHealth = (int)Mathf.Floor((baseStats[0] * level / 25.0f) + 10);
        currentActiveHealth = maxActiveHealth;
        maxMana = (int)Mathf.Floor((baseStats[1] * level / 50.0f) + 5);
        attack = (int)Mathf.Floor((baseStats[2] * level / 50.0f) + 5);
        defense = (int)Mathf.Floor((baseStats[3] * level / 50.0f) + 5);
        intelligence = (int)Mathf.Floor((baseStats[4] * level / 50.0f) + 5);
        resistance = (int)Mathf.Floor((baseStats[5] * level / 50.0f) + 5);
        speed = (int)Mathf.Floor((baseStats[6] * level / 50.0f) + 5);
    }

    //Do something similar to this for stats?
    public int GetLevel() {
        return (int)Mathf.Floor(Mathf.Pow((totalExp / scaleFactors[0]), (1 / 3)));
    }

    //Returns true if there was a level up
    public bool GainExp(int amount) {
        int levelBefore = GetLevel();
        totalExp += amount;
        int levelAfter = GetLevel();
        return (levelBefore != levelAfter);
    }

    public int getLevel() {
        return (int)Mathf.Pow(totalExp, 1 / 3f);
    }

    public Type getPType() {
        return species.getPType();
    }
    public Type getSType() {
        return species.getSType();
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

    public float getCriticalHealth() {
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

    public bool isPlayerOwned() {
        return playerOwned;
    }
}
