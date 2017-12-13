using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    GameObject physicsManager = null;

    private int currentScene = 0;

    public static GameManager gameManager;

    private void Awake()
    {
        if (gameManager == null)
        {
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameObject.Find("Canvas").transform.Find("MainMenu").Find("Layout").Find("Button1").GetComponent<Button>().onClick.AddListener(delegate { GameManager.gameManager.LoadSceneByIndex(1); });
        GameObject.Find("Canvas").transform.Find("MainMenu").Find("Layout").Find("Button2").GetComponent<Button>().onClick.AddListener(delegate { GameManager.gameManager.LoadSceneByIndex(2); });
        GameObject.Find("Canvas").transform.Find("MainMenu").Find("Layout").Find("Button3").GetComponent<Button>().onClick.AddListener(delegate { GameManager.gameManager.Quit(); });
        currentScene = SceneManager.GetActiveScene().buildIndex; 
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Quit()
    {
        if(currentScene == 0)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
        }
        else
        {
            LoadSceneByIndex(0);
        }

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
        if(mode == LoadSceneMode.Single)
        {
            currentScene = scene.buildIndex;
        }
        if(currentScene == 0)
        {
            GameObject.Find("Canvas").transform.Find("MainMenu").Find("Layout").Find("Button1").GetComponent<Button>().onClick.AddListener(delegate { GameManager.gameManager.LoadSceneByIndex(1); });
            GameObject.Find("Canvas").transform.Find("MainMenu").Find("Layout").Find("Button2").GetComponent<Button>().onClick.AddListener(delegate { GameManager.gameManager.LoadSceneByIndex(2); });
            GameObject.Find("Canvas").transform.Find("MainMenu").Find("Layout").Find("Button3").GetComponent<Button>().onClick.AddListener(delegate { GameManager.gameManager.Quit(); });
        }
    }

    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            Debug.Log("should quit");
            Quit();
        }
    }

}
