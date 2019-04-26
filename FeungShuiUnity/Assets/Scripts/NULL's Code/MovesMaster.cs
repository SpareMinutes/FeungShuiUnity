using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesMaster
{
    public Dictionary<string, Move> Master = new Dictionary<string, Move>{
        {"Scratch", new Move(40,1,1, new List<string>{"Normal"}, true)},
        {"Pound", new Move(40,1,1, new List<string>{"Normal"}, true)},
        {"Surf", new Move(90,1,1, new List<string>{"Water"}, false)},
        {"Fissure", new Move(10^12,1,1, new List<string>{"Ground"}, false)} // 1,000,000,000,000 damage should be enough for a one hit (dont think we should have them in out game)
    };
}
