using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint(160, 160, 160)]
public class CommentNode : Node {
    [TextArea(1, 3)]
    public string text;

	public override object GetValue(NodePort port) {
		return null;
	}
}