using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("Interactions/Question")]
public class QuestionNode : InteractionNode {
    [TextArea(1, 2)]
    public string message;
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output(dynamicPortList = true)] public List<string> answers;

    public override void Execute() {
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().ShowMessage(message, false);
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().ShowAnswers(answers.ToArray());
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().SetActiveNode(this);
    }
}