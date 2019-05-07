using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovesMaster
{
    public static Dictionary<string, Move> Master = new Dictionary<string, Move>{
        {"Scratch", new Move(40, 1, 1,"Beast", 0.01f, true)},
        {"Pound", new Move(40, 1, 1, "Beast", 0.01f, true)},
        {"Surf", new Move(90, 1, 1, "Water", 0.01f, false)},
        {"Fissure", new Move(10^12, 0.3f, 1, "Earth", 0, false)} // 1,000,000,000,000 damage should be enough for a one hit (dont think we should have OHKO in our game)
    };
}
