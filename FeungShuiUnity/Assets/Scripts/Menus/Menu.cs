using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public abstract class Menu : MonoBehaviour {
    protected Menu LastMenu;

    protected bool paused;

    public void OpenNewMenu(string SceneName) {
        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += SetThisAsLastMenu;
        gameObject.SetActive(false);
    }

    public void SetThisAsLastMenu(Scene scene, LoadSceneMode mode) {
        Pause();
        GameObject.Find("EventSystem").GetComponent<Menu>().SetLastMenu(this);
        SceneManager.sceneLoaded -= SetThisAsLastMenu;
    }

    public void SetLastMenu(Menu last) {
        LastMenu = last;
    }

    public void ReturnToLast() {
        if(LastMenu!=null) LastMenu.Resume();
        Close();
    }

    public virtual void Pause() {
        paused = true;
    }

    public virtual void Resume() {
        paused = false;
        gameObject.SetActive(true);
    }

    public virtual void Close() {
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
