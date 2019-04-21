using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Types_2
{
    //Type effectiveness dictionaries
    private static Dictionary<string,List<string>> strongTypeEffectiveness = new Dictionary<string,List<string>>{
        {"Fire", new List<string>{"Plant","Beast"}},
        {"Water", new List<string>{"Fire","Earth","Stone"}},
        {"Earth", new List<string>{"Fire","Electricity"}},
        {"Wind", new List<string>{"Plant"}}
        //add more
    }; //this is for types that the key type DEALS extra damage TO
    private static Dictionary<string,List<string>> weakTypeEffectiveness = new Dictionary<string,List<string>>{
        {"Fire", new List<string>{"Earth", "Water"}},
        {"Water", new List<string>{"Plant", "Ice"}},
        {"Earth", new List<string>{"Water", "Ice"}},
        {"Wind", new List<string>{"Electricity", "Stone"}}
        //add more
    }; //this is for types that the key type TAKES extra damage FROM
}
