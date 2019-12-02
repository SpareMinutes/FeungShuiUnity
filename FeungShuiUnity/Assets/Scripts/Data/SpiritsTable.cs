using System.Collections.Generic;

public class SpiritsTable {
    //0-health, 1-mana, 2-attack, 3-defense, 4-intelligence, 5-resistance, 6-speed
    private static Dictionary<string, Species> Master = new Dictionary<string, Species>{
        {"Fantabel", new Species(Type.Metal, Type.None, new int[]{80, 70, 60, 90, 65, 70, 55})},
        {"Strython", new Species(Type.Beast, Type.None, new int[]{65, 80, 50, 65, 120, 95, 90})},
        {"Mostone", new Species(Type.Earth, Type.None, new int[]{100, 75, 90, 120, 65, 80, 45})},
        {"Oculune", new Species(Type.Dark, Type.Beast, new int[]{95, 80, 120, 75, 90, 65, 100})},
        {"Otulcat", new Species(Type.Earth, Type.None, new int[]{75, 90, 60, 70, 80, 75, 80})},
        {"Vivix", new Species(Type.Wood, Type.None, new int[]{90, 120, 75, 65, 90, 120, 100})}
    };

    public static Species Find(string name) {
        return Master[name];
    }
}
