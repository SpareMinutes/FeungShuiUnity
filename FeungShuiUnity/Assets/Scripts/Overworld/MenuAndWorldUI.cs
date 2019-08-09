using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuAndWorldUI : MonoBehaviour
{
    public GameObject button;
    public EventSystem ES;
    public GameObject player;

    public void Start () {
        player.GetComponent<Walk>().canWalk = false;
        player.transform.GetChild (1).gameObject.SetActive(false);
        button.GetComponent<Button>().interactable = true;
        ES.SetSelectedGameObject(button);
    }

    public void disableButton () {
        button.GetComponent<Button>().interactable = false;
        player.GetComponent<Walk>().canWalk = true;
        player.transform.GetChild (1).gameObject.SetActive(true);
    }
}
