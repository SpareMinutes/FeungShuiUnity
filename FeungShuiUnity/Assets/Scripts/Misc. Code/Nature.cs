using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nature {
    //Examples
    public static Nature lazy = new Nature(0, 0.9f, 1.2f, 0.9f, 1.2f, 0.85f);
    public static Nature aggressive = new Nature(1, 1.22f, 0.9f, 1f, 1f, 0.9f);
    int[][] compatibility;
    int id;
    float atkScale, defScale, intScale, resScale, spdScale;

    public Nature(int idIn, float asIn, float dsIn, float isIn, float rsIn, float ssIn)
    {
        this.id = idIn;
        this.atkScale = asIn;
        this.defScale = dsIn;
        this.intScale = isIn;
        this.resScale = rsIn;
        this.spdScale = ssIn;
    }
}
