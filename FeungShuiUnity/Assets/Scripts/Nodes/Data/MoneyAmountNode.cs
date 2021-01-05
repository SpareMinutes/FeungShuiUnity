using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Data/Money Amount"), NodeTint(200, 150, 80)]
public class MoneyAmountNode : ProcessorNode {
    
    [Output]public int output;

    public override object GetValue(GameObject context){
        return GameObject.Find("WalkableCharacter").GetComponent<Inventory>().money;
    }
}
