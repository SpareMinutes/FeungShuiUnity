using System.Collections.Generic;
using UnityEngine;

//Handles Creature status for reference in parties
[System.Serializable]
public class Creature {
    #region Info and trait variables
    System.Guid uid;
    public bool playerOwned;
    [SerializeField]
    private string name;
    public Species species;
    Nature nature;
    //The amount this spicific creature differs from others of its level and species (todo: unused)
    //0-health, 1-mana, 2-attack, 3-defense, 4-intelligence, 5-resistance, 6-speed, 7-exp
    int[] scaleFactors;
    public List<Move> Moves;
    int totalExp = -1;
    //Used for editor
    public int startLevel;
    public Dictionary<int, Move> LevelupMoves;
    #endregion

    #region Stat variables
    //dont need to edit these in the editor, but they are needed by other scripts so theyll stay public
    public float currentCriticalHealth, currentActiveHealth, currentMana;

    private List<float> statModifiers;

    private static Dictionary<Creature, float> friendshipDict = new Dictionary<Creature, float>();
    private float trainerFriendship; // because the player isnt of type Creature
    #endregion
    
    //Create a new creature from code
    public Creature(Species spIn, int startLevelIn) {
        species = spIn;
        startLevel = startLevelIn;
        init();
        int testLevel = getLevel();
        while(Moves.Count < 4 && testLevel > 0) {
            Move result = species.GetLevelupMove(testLevel);
            if (result != null)
                Moves.Add(result);
            testLevel--;
        }
    }

    //Create a new creature from editor
    public void init() {
        //If the guid is all 0 then we know this Creature has never been initialized.
        if (uid.ToString().Equals("00000000-0000-0000-0000-000000000000")) {
            //Generate permanent details
            uid = System.Guid.NewGuid();
            nature = null; //Todo
            scaleFactors = null; //Todo

            //Initialize stats
            totalExp = (int)((species.getStats()[7] / 100f) * Mathf.Pow(startLevel, 3));
            currentCriticalHealth = getMaxCriticalHealth();
            currentActiveHealth = getMaxActiveHealth();
            currentMana = getMaxMana();
        }
    }

    public string GetName() {
        return (name== null || name.Equals("")) ? species.name : name;
    }

    //Do something similar to this for stats?
    public int GetLevel() {
        return (int)Mathf.Floor(Mathf.Pow(totalExp / (species.getStats()[7]/100), 1 / 3));
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
        return (int)Mathf.Floor((species.getStats()[0] * getLevel() / 25.0f) + 10);
    }

    public float getMaxCriticalHealth() {
        return (int)Mathf.Floor(getMaxActiveHealth()/4);
    }

    public float getMaxMana() {
        return (int)Mathf.Floor((species.getStats()[1] * getLevel() / 50.0f) + 5);
    }

    public float getStat(int stat) {
        return Mathf.Floor((species.getStats()[stat] * getLevel() / 50.0f) + 5);
    }
}
