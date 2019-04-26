using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesMaster
{
    public Dictionary<string, Move> Master = new Dictionary<string, Move>{
        {"Scratch", new Move(40,1,1, new List<string>{"Normal"}, "physical")},
        {"Pound", new Move(40,1,1, new List<string>{"Normal"}, "Physical")},
        {"Surf", new Move(90,1,1, new List<string>{"Water"}, "Special")},
        {"Fissure", new Move(10^12,1,1, new List<string>{"Water"}, "Special")} // 1,000,000,000,000 damage should be enough for a one hit (dont think we should have them in out game)
    };
}
