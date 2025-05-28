using System;
using UnityEngine;
using UnityEngine.SceneManagement;  
using System.Linq;

public class SceneController : MonoBehaviour
{
    private int cornerNumber;
    public bool isCornerSetupDone;
    public PrefabManager prefabManager;
    private Vector3[] corners = new Vector3[4];

    private void Start()
    {
        prefabManager = FindAnyObjectByType<PrefabManager>();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SwitchScenes(string sceneName)
    {
        SceneManager.LoadScene(sceneName); 

    }
    
    public void SwitchScenes(int sceneId)
    {
        SceneManager.LoadScene(sceneId); 

    }
    
    
    public void SetupCorner(Vector3 position)
    {
        corners[cornerNumber] = position;
        if (cornerNumber<3)
        {
            cornerNumber++;            
        }
        else
        {
            cornerNumber = 0;
        }
        this.isCornerSetupDone = !corners.Contains(Vector3.zero);
        Debug.Log(isCornerSetupDone);
    }


    public void Setup()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Debug.Log("Scene 1 Setup");
            Instantiate(prefabManager.GetPrefab("TutorialDesk"), Vector3.Lerp(corners[0], corners[1], 0.5f)+Vector3.up, Quaternion.identity);
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            Debug.Log("Scene 2 Setup");
            Instantiate(prefabManager.GetPrefab("Cauldron"), Vector3.Lerp(corners[0], corners[1], 0.5f)+Vector3.up, Quaternion.identity);
        }
    }    
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);

        if (prefabManager == null)
        {
            prefabManager = FindAnyObjectByType<PrefabManager>();
        }

        if (prefabManager == null)
        {
            Debug.LogError("PrefabManager is NULL after scene load!");
            return;
        }

        if (!isCornerSetupDone)
        {
            Debug.LogWarning("Corners not ready, skipping setup.");
            return;
        }

        Setup();
    }

    
}
