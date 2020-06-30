using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {
    public int startBranch;
    public InteractionBranch[] branches;
    int currBranch, currStep;

    // Start is called before the first frame update
    void Start(){
        currBranch = startBranch;
        currStep = 0;
    }

    public void RunStep() {
        InteractionStep step = branches[currBranch].steps[currStep];
        switch (step.type) {
            case StepType.Question:
                GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().ShowMessage(step.message, false);
                GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().ShowAnswers(step.strings);
                break;
            case StepType.Simple:
                GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().ShowMessage(step.message, true);
                break;
            case StepType.Battle:
                GetComponent<Battle>().StartBattle();
                break;
        }
    }

    public void SetBranch(int branch) {
        currBranch = branch;
        currStep = 0;
    }

    [System.Serializable]
    public class InteractionBranch {
        public string branchName;
        public InteractionStep[] steps;
    }

    [System.Serializable]
    public class InteractionStep {
        [TextArea(1, 2)]
        public string message;
        public string[] strings;
        public int[] ints;
        public StepType type;
    }

    public enum StepType {
        Simple, //Message: text to show
        Question, //Message: text to show, Strings: options, Ints: coresponding branch to switch to; Not yet implemented
        Battle //Ints: branch to play... [0]-upon victory, [1]-upon defeat, [2]-when rematched, [3]-when talked to again after defeat
    }
}
