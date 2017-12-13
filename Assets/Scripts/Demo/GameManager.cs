using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    private void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        GameObject physManager = GameObject.Find("PhysicsManager");
        if(physManager != null)
        {
            Destroy(physManager);
        }
    }

}
