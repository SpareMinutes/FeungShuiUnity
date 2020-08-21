using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Interactions/Text")]
public class TextNode : InteractionNode {
    [TextArea(1, 2)]
    public string message;
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool next;

    public override void Execute(GameObject context) {
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().ShowMessage(message, true);
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().SetActiveNode(this);
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().SetDialogueContext(context);
    }
}