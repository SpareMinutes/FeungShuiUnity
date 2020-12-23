using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Data/HasItem"), NodeTint(200, 150, 80)]
public class HasItemNode : ProcessorNode {
    public BagTab tab;
    public Item item;
    [Output] public bool output;

    public override object GetValue(GameObject context) {
        return GameObject.Find("WalkableCharacter").GetComponent<Inventory>().GetList(tab).Contains(item);
    }
}