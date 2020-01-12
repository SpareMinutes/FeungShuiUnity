using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour {
    public Item item;
    public Text displayText;

    public void Start () {
        displayText.text = item.displayName;
    }
}
