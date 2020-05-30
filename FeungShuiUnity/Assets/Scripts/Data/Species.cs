using System.Collections;
using System.Collections.Generic;

public class Species{
    Type typeP, typeS;
    Hashtable learnset;
    //0-health, 1-mana, 2-attack, 3-defense, 4-intelligence, 5-resistance, 6-speed
    int[] baseStats;

    public Species(Type typePIn, Type typeSIn, int[] baseStatsIn) {
        this.typeP = typePIn;
        this.typeS = typeSIn;
        this.baseStats = baseStatsIn;
    }

    public Spirit Spawn(int startLevel){
        Dictionary<string, float> scaleFactors = new Dictionary<string, float>();
        Nature nature = SelectNature();
        //TODO: Generate final scale factors
        return new Spirit(this, nature, baseStats, startLevel);
    }

    protected Nature SelectNature(){
        //TODO
        return null;
    }

    public int[] getStats() {
        return baseStats;
    }

    public Type getPType () {
        return typeP;
    }

    public Type getSType () {
        return typeS;
    }
}