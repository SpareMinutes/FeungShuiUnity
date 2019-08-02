using System.Collections.Generic;

public abstract class Natures2
{
    //this class is just to store all the personalities in one class with appropriate methods to help
    public static List<string> personalityKeys = new List<string>{"Hardy","Lonely","Brave","Adamant"}; // just the pokemon ones for placeholders
    public static Dictionary<string, List<float>> personalityDict = new Dictionary<string, List<float>>{
        // the list of modifiers are in order that they are stated above (activehealth,crithealth,atk,def,int,res,spd)
        //since they are multipliers 1 means no change >1 means increase, <1 means decrease (gives potential to nullify a stat, eg shedinja always has 1 health, or we
                //could make it not have any defense(no reduction in physical damage)) 
        {"Hardy", new List<float>{1, 1, 1, 1, 1, 1, 1}}, //no modifiers
        {"Lonely", new List<float>{1, 1.25f, 0.75f, 1, 1, 1, 1}}, // +Atk, - spA
        {"Brave", new List<float>{1, 1.25f, 1, 1, 1, 1, 0.75f}}, // +Atk, - Spd
        {"Adamant", new List<float>{1, 1.25f, 1, 0.75f, 1, 1, 1}} // +Atk, - spD
        //obviously change them for the final game
    };
}
