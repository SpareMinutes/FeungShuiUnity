using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class AIGraph : NodeGraph {
    public BeginCheckNode Beginning;

    public void Execute(GameObject context) {
        Beginning.Execute(context);
    }
}