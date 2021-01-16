using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EncounterZone : MonoBehaviour{
    //Encounter rate vars
    public float DefaultRate;
    public string[] TileNames;
    public float[] AltRates;
    private Dictionary<string, float> AltTiles;

    //Encounter type vars
    public Encounter[] PossibleEncounters;
    public float[] Probabilities;
    private float[] cumulativeProbs;

    void Start(){
        //Build tile dictionary
        AltTiles = new Dictionary<string, float>();
        for (int i = 0; i < Mathf.Min(TileNames.Length, AltRates.Length); i++)
            AltTiles.Add(TileNames[i], AltRates[i]);
        //Create a lookup table for what number Range() needs to draw for each possible encounter
        cumulativeProbs = new float[Mathf.Min(PossibleEncounters.Length, Probabilities.Length)];
        float totalProbs = Probabilities.Sum();
        float runningTotal = 0;
        for (int i = 0; i < cumulativeProbs.Length; i++) {
            cumulativeProbs[i] = runningTotal;
            runningTotal += Probabilities[i] / totalProbs;
        }
    }

    public Creature[] RollEncounter(string type) {
        float rate;
        if (!AltTiles.TryGetValue(type, out rate)) {
            rate = DefaultRate;
            type += "(default)";
        }
        float roll = Random.Range(0f, 1f);

        if (roll < rate) {
            roll = Random.Range(0f, 1f);
            Creature[] opposition = new Creature[2];
            for(int i=0; i<2; i++)
                for (int j = cumulativeProbs.Length - 1; j >= 0; j--)
                    if (cumulativeProbs[j] <= roll)
                        opposition[i] = PossibleEncounters[j].Generate();
            return opposition;
        }

        return null;
    }

    public class Encounter {
        public Species species;
        public int MeanLevel, LevelRange;

        public Creature Generate() {
            float level = 0;
            for (int i = 0; i < 3; i++)
                level += Random.Range(0, LevelRange / 3f);
            level += MeanLevel - (LevelRange / 2);
            return new Creature(species, (int)level);
        }
    }
}
