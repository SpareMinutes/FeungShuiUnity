using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using XNode;

[CreateNodeMenu("Interactions/Question")]
public class QuestionNode : InteractionNode {
    [TextArea(1, 2)]
    public string message;
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output(dynamicPortList = true)] public List<string> answers;
    [Input(backingValue = ShowBackingValue.Never, dynamicPortList = true)] public string variable;

    public override void Execute(GameObject context) {
        string parsedMessage = message;
        int varNum = 0;
        foreach (NodePort varPort in DynamicPorts) {
            if (varNum >= answers.Count) {
                parsedMessage = Regex.Replace(parsedMessage, "%v" + (varNum - answers.Count), ((ProcessorNode)varPort.GetConnection(0).node).GetValue(context).ToString());
                Debug.Log(parsedMessage);
            }
            varNum++;
        }
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().ShowMessage(parsedMessage, false);
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().ShowAnswers(answers.ToArray());
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().SetActiveNode(this);
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().SetDialogueContext(context);
    }
}