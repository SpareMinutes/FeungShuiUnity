using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateNodeMenu("Interactions/Flow Control/Random")]
public class RandomNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output(dynamicPortList = true)] public List<float> outcomes;

    public override void Execute(GameObject context) {
        float result = Random.Range(0, outcomes.ToArray().Sum());
        float total = 0f;
        for(int i=0; i< outcomes.ToArray().Length; i++) {
            float value = outcomes.ToArray()[i];
            if(result <= (total + value)) {
                ExecuteNext(GetOutputPort("outcomes " + i), context);
                break;
            } else {
                total += value;
            }
        }
    }
}