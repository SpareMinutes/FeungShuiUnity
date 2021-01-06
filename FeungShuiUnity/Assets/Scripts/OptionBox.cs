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
        foreach (Transform child in Answers.transform) {
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
            answer.transform.parent = Answers.transform;
            answer.GetComponent<RectTransform>().localPosition = new Vector3(0, (actionsLen-i-1) * 10, 0);
            maxLen = Mathf.Max(maxLen, labels[i].Length);
        }
        //Set box height
        for (int i = 6; i < 9; i++) {
            RectTransform rt = Background.transform.GetChild(i).GetComponent<RectTransform>();
            rt.localPosition = new Vector3(rt.localPosition.x, 10 * actions.Length - 1, 0);
        }
        for (int i = 3; i < 6; i++) {
            RectTransform rt = Background.transform.GetChild(i).GetComponent<RectTransform>();
            rt.transform.localScale = new Vector3(rt.transform.localScale.x, (float)(2.5 * actions.Length - 0.25), 1);
        }
        //Set box width
        for (int i = 2; i <= 8; i += 3) {
            RectTransform rt = Background.transform.GetChild(i).GetComponent<RectTransform>();
            rt.localPosition = new Vector3(6 * maxLen, rt.localPosition.y, 0);
        }
        for (int i = 1; i <= 7; i += 3) {
            RectTransform rt = Background.transform.GetChild(i).GetComponent<RectTransform>();
            rt.transform.localScale = new Vector3((float)(1.5 * maxLen), rt.transform.localScale.y, 1);
        }

        ES = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    private void OnEnable() {
        ES.SetSelectedGameObject(null);
    }

    private void Update() {
        if (lastSelected == null)
            lastSelected = Answers.transform.GetChild(0).gameObject;
        if (ES.currentSelectedGameObject == null) {
            ES.SetSelectedGameObject(lastSelected);
        } else {
            lastSelected = ES.currentSelectedGameObject;
        }
    }
}
