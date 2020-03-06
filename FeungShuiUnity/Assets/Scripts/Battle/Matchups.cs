using System.Collections.Generic;

public abstract class Matchups{
    //Type effectiveness dictionaries
    //this is for types that the key type deals EXTRA damage to
    private static Dictionary<Type,List<Type>> strongTypeEffectiveness = new Dictionary<Type,List<Type>>{
        {Type.Fire, new List<Type>{Type.Wood, Type.Metal, Type.Ice}},
        {Type.Wood, new List<Type>{Type.Water, Type.Earth, Type.Smog}},
        {Type.Water, new List<Type>{Type.Metal, Type.Fire, Type.Sky}},
        {Type.Metal, new List<Type>{Type.Earth, Type.Wood, Type.Thunder}},
        {Type.Earth, new List<Type>{Type.Fire, Type.Water, Type.Beast}},
        {Type.Thunder, new List<Type>{Type.Wood, Type.Beast}},
        {Type.Beast, new List<Type>{Type.Water, Type.Ice}},
        {Type.Ice, new List<Type>{Type.Metal, Type.Smog}},
        {Type.Smog, new List<Type>{Type.Earth, Type.Sky}},
        {Type.Sky, new List<Type>{Type.Fire, Type.Thunder}},
        {Type.Light, new List<Type>{Type.Dark}},
        {Type.Dark, new List<Type>{Type.Light}}
    }; 

     //this is for types that the key type deals LESS damage to
    private static Dictionary<Type,List<Type>> weakTypeEffectiveness = new Dictionary<Type,List<Type>>{
        {Type.Fire, new List<Type>{Type.Earth, Type.Water, Type.Sky, Type.Fire}},
        {Type.Wood, new List<Type>{Type.Fire, Type.Metal, Type.Thunder, Type.Wood}},
        {Type.Water, new List<Type>{Type.Wood, Type.Earth, Type.Beast, Type.Water}},
        {Type.Metal, new List<Type>{Type.Water, Type.Fire, Type.Ice, Type.Metal}},
        {Type.Earth, new List<Type>{Type.Metal, Type.Wood, Type.Smog, Type.Earth}},
        {Type.Thunder, new List<Type>{Type.Metal, Type.Sky, Type.Thunder}},
        {Type.Beast, new List<Type>{Type.Earth, Type.Thunder, Type.Beast}},
        {Type.Ice, new List<Type>{Type.Fire, Type.Beast, Type.Ice}},
        {Type.Smog, new List<Type>{Type.Wood, Type.Ice, Type.Smog}},
        {Type.Sky, new List<Type>{Type.Water, Type.Smog, Type.Sky}},
        {Type.Light, new List<Type>{Type.Light}},
        {Type.Dark, new List<Type>{Type.Dark}}
    };

    //used mainly for STAB bonuses maybe also for semi type effectiveness (?)
    //linked is for secondary STAB bonuses
    private static Dictionary<Type, List<Type>> linkedTypes = new Dictionary<Type, List<Type>>{
        {Type.Fire, new List<Type>{Type.Thunder,Type.Earth}},
        {Type.Earth, new List<Type>{Type.Sky,Type.Metal}},
        {Type.Metal, new List<Type>{Type.Stone,Type.Water}},
        {Type.Water, new List<Type>{Type.Ice,Type.Wood}},
        {Type.Wood, new List<Type>{Type.Beast,Type.Fire}},
        {Type.Thunder, new List<Type>{Type.Fire}},
        {Type.Sky, new List<Type>{Type.Earth}},
        {Type.Stone, new List<Type>{Type.Metal}},
        {Type.Ice, new List<Type>{Type.Water}},
        {Type.Beast, new List<Type>{Type.Wood}},
        {Type.Light, new List<Type>{}},
        {Type.Dark, new List<Type>{}}
    };
    //unlinked is for secondary STAB losses
    private static Dictionary<Type,List<Type>> unlinkedTypes = new Dictionary<Type,List<Type>> {
        {Type.Fire, new List<Type>{Type.Metal,Type.Water}},
        {Type.Earth, new List<Type>{Type.Water,Type.Wood}},
        {Type.Metal, new List<Type>{Type.Fire,Type.Wood}},
        {Type.Water, new List<Type>{Type.Fire,Type.Earth}},
        {Type.Wood, new List<Type>{Type.Metal,Type.Earth}},
        {Type.Thunder, new List<Type>{}},
        {Type.Sky, new List<Type>{}},
        {Type.Stone, new List<Type>{}},
        {Type.Ice, new List<Type>{}},
        {Type.Beast, new List<Type>{}},
        {Type.Light, new List<Type>{Type.Dark}},
        {Type.Dark, new List<Type>{Type.Light}}
    };

    public static List<Type> getStrongTypeEffectiveness (Type type) {
        return strongTypeEffectiveness[type];
    }

    public static List<Type> getWeakTypeEffectiveness (Type type) {
        return weakTypeEffectiveness[type];
    }

    public static List<Type> getLinkedTypes (Type type) {
        return linkedTypes[type];
    }

    public static List<Type> getUnlinkedTypes (Type type) {
        return unlinkedTypes[type];
    }
}
