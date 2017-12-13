using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    GameObject physicsManager = null;

    private void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

    private void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("unloading scene");
        if(physicsManager != null)
        {
            Destroy(physicsManager);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("loading scene");
        if (physicsManager == null)
        {
            physicsManager = Instantiate(Resources.Load("Prefabs/PhysicsManager")) as GameObject;
        }
    }

}
