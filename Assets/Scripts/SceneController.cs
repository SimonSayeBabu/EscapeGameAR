using System;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SceneController : MonoBehaviour
{
    public bool isSetupDone;
    public bool isUndergroundPuzzleSolved;
    public int solvedTubes;
    public int[] valves = new int[6];
    
    public PrefabManager prefabManager;
    public ARPlaneManager planeManager;
 
    private Vector3 centerPosition;
    private List<Vector3> edgePositions = new List<Vector3>();     // 0: Nord, 1: Est, 2: Sud, 3 = Ouest
    private List<Vector3> cornerPositions = new List<Vector3>();   // 0: NE, 1: SE, 2: SW, 3: NW

    private float timeElapsed = 0f;
    private bool isTiming = false;
    private bool hasStartedTimer = false;


    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        prefabManager = FindAnyObjectByType<PrefabManager>();
        planeManager = FindAnyObjectByType<ARPlaneManager>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (isTiming)
        {
            timeElapsed += Time.deltaTime;
        }
    }

    private void DisplayTimeElapsed()
    {
        // Trouver le texte dans la scène 7 (assure-toi qu'il a le tag ou le nom "TimeTakenText" par exemple)
        Text timeText = GameObject.Find("TimeTakenText")?.GetComponent<Text>();

        if (timeText != null)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeElapsed);
            string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", 
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            timeText.text = "Temps écoulé : " + formattedTime;
        }
        else
        {
            Debug.LogWarning("Text UI pour afficher le temps non trouvé !");
        }
    }

    
    public void SwitchScenes(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SwitchScenes(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void CalculatePlacementData()
    {
        List<ARPlane> planes = new List<ARPlane>();
        foreach (var plane in planeManager.trackables)
        {
            if (plane.alignment == PlaneAlignment.HorizontalUp)
                planes.Add(plane);
        }

        if (planes.Count == 0)
        {
            Debug.LogWarning("No horizontal planes detected for placement.");
            return;
        }

        // 1. Calcul du centre (centre des extrêmes X et Z)
        Vector3 min = new Vector3(float.MaxValue, 0f, float.MaxValue);
        Vector3 max = new Vector3(float.MinValue, 0f, float.MinValue);

        List<Vector3> allWorldVertices = new List<Vector3>();

        foreach (var plane in planes)
        {
            var mesh = plane.GetComponent<MeshFilter>()?.mesh;
            if (mesh == null) continue;

            foreach (var vertex in mesh.vertices)
            {
                Vector3 worldPos = plane.transform.TransformPoint(vertex);
                allWorldVertices.Add(worldPos);

                if (worldPos.x < min.x) min.x = worldPos.x;
                if (worldPos.z < min.z) min.z = worldPos.z;
                if (worldPos.x > max.x) max.x = worldPos.x;
                if (worldPos.z > max.z) max.z = worldPos.z;
            }
        }

        centerPosition = new Vector3((min.x + max.x) / 2f, 0f, (min.z + max.z) / 2f);

        // 2. Initialisation des bords
        Vector3? north = null, south = null, east = null, west = null;
        float maxZ = float.NegativeInfinity, minZ = float.PositiveInfinity;
        float maxX = float.NegativeInfinity, minX = float.PositiveInfinity;

        foreach (var pos in allWorldVertices)
        {
            if (pos.z > maxZ) { maxZ = pos.z; north = pos; }
            if (pos.z < minZ) { minZ = pos.z; south = pos; }
            if (pos.x > maxX) { maxX = pos.x; east = pos; }
            if (pos.x < minX) { minX = pos.x; west = pos; }
        }

        edgePositions.Clear();
        edgePositions.Add(north ?? centerPosition); // 0: Nord
        edgePositions.Add(east ?? centerPosition);  // 1: Est
        edgePositions.Add(south ?? centerPosition); // 2: Sud
        edgePositions.Add(west ?? centerPosition);  // 3: Ouest

        // 3. Initialisation des coins
        Vector3? ne = null, se = null, sw = null, nw = null;
        float maxDistNE = 0f, maxDistSE = 0f, maxDistSW = 0f, maxDistNW = 0f;

        foreach (var pos in allWorldVertices)
        {
            Vector3 dir = pos - centerPosition;
            float dist = dir.magnitude;

            if (dir.x >= 0 && dir.z >= 0 && dist > maxDistNE)
            {
                maxDistNE = dist;
                ne = pos;
            }
            else if (dir.x >= 0 && dir.z < 0 && dist > maxDistSE)
            {
                maxDistSE = dist;
                se = pos;
            }
            else if (dir.x < 0 && dir.z < 0 && dist > maxDistSW)
            {
                maxDistSW = dist;
                sw = pos;
            }
            else if (dir.x < 0 && dir.z >= 0 && dist > maxDistNW)
            {
                maxDistNW = dist;
                nw = pos;
            }
        }

        cornerPositions.Clear();
        cornerPositions.Add(ne ?? centerPosition); // 0: NE
        cornerPositions.Add(se ?? centerPosition); // 1: SE
        cornerPositions.Add(sw ?? centerPosition); // 2: SW
        cornerPositions.Add(nw ?? centerPosition); // 3: NW

        Debug.Log($"Placement data calculated. Center: {centerPosition}, Edges: {edgePositions.Count}, Corners: {cornerPositions.Count}");
    }

    Quaternion GetRotationTowardsCenter(Vector3 fromPosition)
    {
        Vector3 direction = centerPosition - fromPosition;
        direction.y = 0f; // Ignore la hauteur pour rester parallèle au sol

        if (direction == Vector3.zero)
            return Quaternion.identity;

        return Quaternion.LookRotation(direction.normalized);
    }

    public void Setup()
    {

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Debug.Log("Scene 1 Setup");
            Instantiate(prefabManager.GetPrefab("TutorialDesk"), centerPosition + Vector3.up * 0.1f, Quaternion.identity);
            Instantiate(prefabManager.GetPrefab("DoorToStart"), edgePositions[3] + Vector3.up * 0.1f, GetRotationTowardsCenter(edgePositions[3]));

        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            Debug.Log("Scene 2 (Start room) Setup");
            if (edgePositions.Count > 0)
            {
                Instantiate(prefabManager.GetPrefab("Intro Table"), centerPosition + Vector3.up * 0.1f, Quaternion.identity);
                Instantiate(prefabManager.GetPrefab("DoorToLabo"), edgePositions[0] + Vector3.up * 0.1f, GetRotationTowardsCenter(edgePositions[0]));
                Instantiate(prefabManager.GetPrefab("Commode"), edgePositions[1] + Vector3.up * 0.1f, GetRotationTowardsCenter(edgePositions[1]));
                Instantiate(prefabManager.GetPrefab("DoorToSerre"), edgePositions[2] + Vector3.up * 0.1f, GetRotationTowardsCenter(edgePositions[2]));
            }
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            Debug.Log("Scene 3 (Labo room) Setup");
            if (edgePositions.Count > 0)
            {
                Instantiate(prefabManager.GetPrefab("Cauldron"), centerPosition + Vector3.up * 0.3f, Quaternion.identity);
                Instantiate(prefabManager.GetPrefab("DoorToLibrairie"), edgePositions[2] + Vector3.up * 0.1f, GetRotationTowardsCenter(edgePositions[2]));
                Instantiate(prefabManager.GetPrefab("Commode Labo"), edgePositions[1] + Vector3.up * 0.2f, GetRotationTowardsCenter(edgePositions[1]));
                Instantiate(prefabManager.GetPrefab("DoorToStart"), edgePositions[0] + Vector3.up * 0.1f, GetRotationTowardsCenter(edgePositions[0]));
            }
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            Debug.Log("Scene 4 (Librairie room) Setup");
            if (edgePositions.Count > 0)
            {
                Instantiate(prefabManager.GetPrefab("LibraryDesks"), centerPosition + Vector3.up * 0.2f, Quaternion.identity);
                Instantiate(prefabManager.GetPrefab("DoorToLabo"), edgePositions[2] + Vector3.up * 0.1f, GetRotationTowardsCenter(edgePositions[2]));
                Instantiate(prefabManager.GetPrefab("BookShelf1"), edgePositions[3] + Vector3.up * 0.1f, GetRotationTowardsCenter(edgePositions[3]));
                Instantiate(prefabManager.GetPrefab("BookShelf2"), edgePositions[0] + Vector3.up * 0.1f, GetRotationTowardsCenter(edgePositions[0]));
                Instantiate(prefabManager.GetPrefab("BookShelf3"), edgePositions[1] + Vector3.up * 0.1f, GetRotationTowardsCenter(edgePositions[1]));
                //Instantiate(prefabManager.GetPrefab("Column"), cornerPositions[0] + Vector3.up * 0.1f, Quaternion.identity);
                //Instantiate(prefabManager.GetPrefab("Column"), cornerPositions[1] + Vector3.up * 0.1f, Quaternion.identity);
                //Instantiate(prefabManager.GetPrefab("Column"), cornerPositions[2] + Vector3.up * 0.1f, Quaternion.identity);
                //Instantiate(prefabManager.GetPrefab("Column"), cornerPositions[3] + Vector3.up * 0.1f, Quaternion.identity);
            }
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            Debug.Log("Scene 5 (Serre room) Setup");
            if (edgePositions.Count > 0)
            {
                Instantiate(prefabManager.GetPrefab("PlantesSerreEnigme"), centerPosition + Vector3.up * 0.05f, Quaternion.identity);
                Instantiate(prefabManager.GetPrefab("DoorToStart"), edgePositions[2] + Vector3.up * 0.1f, GetRotationTowardsCenter(edgePositions[2]));
                Instantiate(prefabManager.GetPrefab("WaterWell"), cornerPositions[3] + Vector3.up * 0.1f, Quaternion.identity);
                Instantiate(prefabManager.GetPrefab("TreePrefab"), cornerPositions[2] + Vector3.up * 0.1f, Quaternion.identity);
                Instantiate(prefabManager.GetPrefab("DoorToUnderground"), edgePositions[1] + Vector3.up * 0.1f, GetRotationTowardsCenter(edgePositions[1]));
            }
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            Debug.Log("Scene 6 (Underground room) Setup");
            if (edgePositions.Count > 0)
            {
                Instantiate(prefabManager.GetPrefab("EnigmeValve"), centerPosition + Vector3.up * 0.1f, Quaternion.identity);
                Instantiate(prefabManager.GetPrefab("DoorToSerreBis"), edgePositions[1] + Vector3.up * 0.1f, GetRotationTowardsCenter(edgePositions[1]));
                Instantiate(prefabManager.GetPrefab("Mushroom Planter"), edgePositions[2] + Vector3.up * 0.1f, Quaternion.identity);
            }
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            Debug.Log("Scene 7 (End Screen) Setup");
            UIHandler uiHandler = FindAnyObjectByType<UIHandler>();
            Destroy(uiHandler);
            XROrigin xrOrigin = FindAnyObjectByType<XROrigin>();
            Destroy(xrOrigin);
            RaycastController raycast = FindAnyObjectByType<RaycastController>();
            Destroy(raycast);
            Inventory inventory = FindAnyObjectByType<Inventory>();
            Destroy(inventory);
            PrefabManager prefabManager= FindAnyObjectByType<PrefabManager>();
            Destroy(prefabManager);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);

        if (prefabManager == null)
            prefabManager = FindAnyObjectByType<PrefabManager>();
        if (planeManager == null)
            planeManager = FindAnyObjectByType<ARPlaneManager>();

        if (prefabManager == null )
        {
            Debug.LogError("Missing prefabManager after scene load!");
            return;
        }
        if (planeManager == null )
        {
            Debug.LogError("Missing planeManager after scene load!");
            return;
        }

        // Démarrer le timer uniquement quand on entre dans la scène 2
        if (scene.buildIndex == 2 && !hasStartedTimer)
        {
            timeElapsed = 0f;
            isTiming = true;
            hasStartedTimer = true;
            Debug.Log("Timer started");
        }


        // Arrêter le timer uniquement quand on entre dans la scène 7
        if (scene.buildIndex == 7)
        {
            isTiming = false;
            Debug.Log("Timer stopped");
            DisplayTimeElapsed();
        }

        if (!isSetupDone)
        {
            Debug.LogWarning("Scene not ready, skipping setup.");
            return;
        }

        Setup();
    }


}
