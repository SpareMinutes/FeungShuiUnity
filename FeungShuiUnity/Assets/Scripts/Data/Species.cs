using System.Collections;
using System.Collections.Generic;

public class Species{
    Type type;
    Hashtable learnset;
    //0-exp, 1-health, 2-attack, 3-defense, 4-intelligence, 5-resistance, 6-speed
    float[] speciesScaleFactors;

    public Spirit Spawn(int startLevel){
        Dictionary<string, float> scaleFactors = new Dictionary<string, float>();
        Nature nature = SelectNature();
        //TODO: Generate final scale factors
        return new Spirit(this, nature, speciesScaleFactors, startLevel);
    }

    protected Nature SelectNature(){
        //TODO
        return null;
    }
}
