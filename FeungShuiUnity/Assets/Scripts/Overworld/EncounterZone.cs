﻿using System;
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
        float roll = UnityEngine.Random.Range(0f, 1f);

        if (roll < rate) {
            Creature[] opposition = new Creature[2];
            for (int i = 0; i < 2; i++) {
                roll = UnityEngine.Random.Range(0f, 1f);
                for (int j = cumulativeProbs.Length - 1; j >= 0; j--)
                    if (cumulativeProbs[j] <= roll) {
                        opposition[i] = PossibleEncounters[j].Generate();
                        break;
                    }
            }
            return opposition;
        }

        return null;
    }

    [Serializable]
    public class Encounter {
        public Species species;
        public int MeanLevel, LevelRange;

        public Creature Generate() {
            float level = 0;
            for (int i = 0; i < 3; i++)
                level += UnityEngine.Random.Range(0, LevelRange / 3f);
            level += MeanLevel - (LevelRange / 2);

            List<Move> moves = GenerateMoves((int)level);

            Debug.Log(species.name + " level " + level);
            Creature creature = new Creature(species, (int)level);
            creature.Moves = moves; 
            return creature;
        }

        private List<Move> GenerateMoves (int level) {
            //get a list of possible moves that the creature can know at its current level
            List<Move> returnMoves = new List<Move>();
            List<Move> possibleMoves = new List<Move>();
            foreach (int i in species.moveSet.Keys)  {
                if (i <= level) {
                    //then the creature could have learnt the move
                    possibleMoves.Add(species.moveSet[i]);
                }
            }

            //choose 4 moves at random and return them
            for (int i = 0; i < 4; i++) {
                Move selectedMove = possibleMoves[UnityEngine.Random.Range(0, possibleMoves.Count - 1)];
                returnMoves.Add(selectedMove);
                possibleMoves.Remove(selectedMove);
            }
            return returnMoves;
        }
    }
}
