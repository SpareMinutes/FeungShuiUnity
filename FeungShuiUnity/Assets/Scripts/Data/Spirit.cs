using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit{
    System.Guid uid;
    string name;
    Species species;
    Nature nature;
    //0-exp, 1-health, 2-attack, 3-defense, 4-intelligence, 5-resistance, 6-speed
    float[] scaleFactors;
    int health, totalExp;

    public Spirit(Species spIn, Nature natIn, float[] factorsIn, int startLevel){
        this.species = spIn;
        this.nature = natIn;
        this.scaleFactors = factorsIn;
        this.uid = System.Guid.NewGuid();
        this.totalExp = (int)(scaleFactors[0] * Mathf.Pow(startLevel, 3));
    }

    //Do something similar to this for stats?
    public int GetLevel(){
        return (int)Mathf.Floor(Mathf.Pow((totalExp/scaleFactors[0]), (1/3)));
    }

    //Returns true if there was a level up
    public bool GainExp(int amount){
        int levelBefore = GetLevel();
        totalExp += amount;
        int levelAfter = GetLevel();
        return (levelBefore != levelAfter);
    }
}
