using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public string SceneNameToGo;
    public float XPos, YPos;

    public void OnTriggerEnter2D () {
        //Debug.Log("go there");
        SceneManager.LoadScene(SceneNameToGo, LoadSceneMode.Single);
        PersistentStats.SceneChangePosX = XPos;
        PersistentStats.SceneChangePosY = YPos;
        PersistentStats.SceneChanged = true;
    }
}
