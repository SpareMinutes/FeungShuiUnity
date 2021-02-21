using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionBox : MonoBehaviour{
    [SerializeField]
    private GameObject Answers, Background, AnswerPrefab;
    public Action[] actions;
    public GameObject lastSelected;
    public int chosen;

    private EventSystem ES;

    public void Populate(string[] labels, Action[] actionsIn) {
        //Create buttons, set actions, and count length of longest label for use later
        actions = new Action[0];
        int maxLen = 0;
        int actionsLen = Mathf.Min(labels.Length, actionsIn.Length);
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        for (int i=0; i<actionsLen; i++) {
            actions = actions.Append(actionsIn[i]).ToArray();
            GameObject answer = Instantiate(AnswerPrefab);
            answer.name = "" + i;
            answer.GetComponent<Text>().text = labels[i];
            answer.GetComponent<Button>().onClick.AddListener(delegate {
                chosen = int.Parse(answer.name);
                Debug.Log(chosen);
                actions[chosen]();
                gameObject.SetActive(false);
            });
            answer.transform.parent = transform;
            answer.GetComponent<RectTransform>().localPosition = new Vector3(4, 4+(actionsLen-i-1) * 10, 0);
            answer.GetComponent<RectTransform>().localScale = new Vector3(0.2f, 0.2f, 1);
            maxLen = Mathf.Max(maxLen, labels[i].Length);
        }
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 6 * maxLen + 8);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10 * actionsLen + 8);

        ES = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        gameObject.SetActive(true);
    }

    private void OnEnable() {
        ES.SetSelectedGameObject(null);
    }

    private void Update() {
        if (lastSelected == null)
            lastSelected = transform.GetChild(0).gameObject;
        if (ES.currentSelectedGameObject == null) {
            ES.SetSelectedGameObject(lastSelected);
        } else {
            lastSelected = ES.currentSelectedGameObject;
        }
    }
}
