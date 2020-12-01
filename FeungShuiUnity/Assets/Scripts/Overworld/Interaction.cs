using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {
    public int startBranch;
    public InteractionBranch[] branches;
    private int[] resultBranches;
    int currBranch, currStep;
    public InteractionGraph graph;

    // Start is called before the first frame update
    void Start(){
        SetBranch(startBranch);
    }

    public void Begin() {
        graph.Execute(gameObject);
        //currBranch = startBranch;
        //currStep = 0;
        //RunStep();
    }

    public InteractionStep getCurrStep() {
        return branches[currBranch].steps[currStep];
    }

    public bool RunStep() {
        try {
            InteractionStep step = branches[currBranch].steps[currStep];
            switch (step.type) {
                case StepType.Question:
                    resultBranches = new int[10];
                    for(int i=0; i<step.ints.Length; i++) {
                        resultBranches[10 - step.ints.Length + i] = step.ints[i];
                    }
                    GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().ShowMessage(step.message, false);
                    GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().ShowAnswers(step.strings);
                    //GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().SetActiveInteraction(this);
                    break;
                case StepType.Simple:
                    GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().ShowMessage(step.message, true);
                    //GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().SetActiveInteraction(this);
                    currStep++;
                    break;
                case StepType.Battle:
                    GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().disableButton();
                    GetComponent<Battle>().StartTrainerBattle();
                    break;
                case StepType.RandomBranch:
                    int branch = (int)Random.Range(0, step.ints.Length - 0.0000001f);
                    SetBranch(step.ints[branch]);
                    RunStep();
                    break;
                case StepType.SetStart:
                    SetStartBranch(step.ints[0]);
                    currStep++;
                    RunStep();
                    break;
                case StepType.Goto:
                    currBranch = step.ints[0];
                    currStep = step.ints[1];
                    RunStep();
                    break;
                case StepType.SetMusic:
                    AudioSource source = GameObject.Find(step.strings[0]).GetComponent<AudioSource>();
                    if (source != null){
                        source.volume = step.ints[0] / 1000.0f;
                        if (source.clip==null || !source.clip.Equals(step.Obj)) {
                            source.clip = step.Obj == null ? null : (AudioClip)step.Obj;
                            source.Play();
                        }
                    }
                    currStep++;
                    RunStep();
                    break;
            }
            return true;
        } catch (System.IndexOutOfRangeException e) {
            //the branch has ended
            SetBranch(startBranch);
            GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().disableButton();
            return false;
        }
    }

    public void RunAnswer(int selection) {
        SetBranch(resultBranches[selection]);
        RunStep();
    }

    public void SetBranch(int branch) {
        currBranch = branch;
        currStep = 0;
    }

    public void SetStartBranch(int branch) {
        startBranch = branch;
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
        public Object Obj;
    }

    public enum StepType {
        Simple, //Message: text to show
        Question, //Message: text to show, Strings: options, Ints: coresponding branch to switch to; Not yet implemented
        Battle, //Ints: branch to play... [0]-upon victory, [1]-upon defeat, [2]-when rematched, [3]-when talked to again after defeat
        RandomBranch, //Ints: possible branches
        SetStart, //Ints[0]: New starting branch branch
        Goto, //Ints[0]: new branch, Ints[1]: new step
        SetMusic, //Ints[0]: the new volume in tenths of a percent (1000=100%), Strings[0]: the name of the audio source object, //Obj: the sound track
    }
}
