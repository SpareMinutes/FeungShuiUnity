using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using XNode;

[CreateNodeMenu("Interactions/Display/Question")]
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
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().ShowMessage(parsedMessage, false);
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().ShowAnswers(answers.ToArray());
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().SetActiveNode(this);
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().SetDialogueContext(context);
    }
}