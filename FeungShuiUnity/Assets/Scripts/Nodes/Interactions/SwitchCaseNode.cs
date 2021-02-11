using UnityEngine;
using XNode;

[CreateNodeMenu("Interactions/Flow Control/SwitchCase") ]
public class SwitchCaseNode : ExecutableNode {
    [Input(backingValue = ShowBackingValue.Never)] public string value;
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output(backingValue = ShowBackingValue.Always)] public string[] cases;
    [Output] public string defaultNext;

    public override void Execute(GameObject context) {
        //Get the value to match
        string v = ((ProcessorNode)GetInputPort("value").GetConnection(0).node).GetValue(context).ToString();
        //Find the index of the match
        int index = -1;
        for(int i=0; i<cases.Length; i++) {
            if(cases[i].Equals(v)){
                index = i;
                break;
            }
        }
        //Execute the branch corresponding to the match
        if (index == -1)
            ExecuteNext(GetOutputPort("defaultNext"), context);
        else {
            NodePort connection = GetOutputPort("cases").GetConnection(index);
            ExecutableNode next = (ExecutableNode)connection.node;
            next.Execute(context);
        }
    }
}