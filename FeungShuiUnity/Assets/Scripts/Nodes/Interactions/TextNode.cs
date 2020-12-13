using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using XNode;

[CreateNodeMenu("Interactions/Text")]
public class TextNode : InteractionNode {
    [TextArea(1, 2)]
    public string message;
    [Input(backingValue = ShowBackingValue.Never, dynamicPortList = true)] public string variable;
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool next;

    public override void Execute(GameObject context) {
        string parsedMessage = message;
        int varNum = 0;
        foreach (NodePort varPort in DynamicPorts) {
            parsedMessage = Regex.Replace(parsedMessage, "%v" + varNum, ((ProcessorNode)varPort.GetConnection(0).node).GetValue(context).ToString());
            Debug.Log(parsedMessage);
            varNum++;
        }
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().ShowMessage(parsedMessage, true);
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().SetActiveNode(this);
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().SetDialogueContext(context);
    }
}