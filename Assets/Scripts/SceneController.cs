using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SceneController : MonoBehaviour
{
    public bool isSetupDone;
    public bool isUndergroundPuzzleSolved;

    public PrefabManager prefabManager;
    public ARPlaneManager planeManager;
 
    private Vector3 centerPosition;
    private List<Vector3> edgePositions = new List<Vector3>();     // 0: Nord, 1: Est, 2: Sud, 3 = Ouest
    private List<Vector3> cornerPositions = new List<Vector3>();   // 0: NE, 1: SE, 2: SW, 3: NW


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        prefabManager = FindAnyObjectByType<PrefabManager>();
        planeManager = FindAnyObjectByType<ARPlaneManager>();
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

        // Calcul du centre global
        Vector3 total = Vector3.zero;
        foreach (var plane in planes)
        {
            total += plane.center;
        }

        centerPosition = total / planes.Count;

        // Initialiser les extrêmes pour trouver les 4 bords
        Vector3? north = null, south = null, east = null, west = null;
        float maxZ = float.NegativeInfinity, minZ = float.PositiveInfinity;
        float maxX = float.NegativeInfinity, minX = float.PositiveInfinity;

        foreach (var plane in planes)
        {
            var mesh = plane.GetComponent<MeshFilter>()?.mesh;
            if (mesh == null) continue;

            foreach (var vertex in mesh.vertices)
            {
                Vector3 worldPos = plane.transform.TransformPoint(vertex);

                float dist = Vector3.Distance(centerPosition, worldPos);
                if (dist < 0.5f || dist > 3f) continue; // Ignore trop proche ou trop loin

                if (worldPos.z > maxZ)
                {
                    maxZ = worldPos.z;
                    north = worldPos;
                }

                if (worldPos.z < minZ)
                {
                    minZ = worldPos.z;
                    south = worldPos;
                }

                if (worldPos.x > maxX)
                {
                    maxX = worldPos.x;
                    east = worldPos;
                }

                if (worldPos.x < minX)
                {
                    minX = worldPos.x;
                    west = worldPos;
                }
            }
        }

        // Stocker dans l’ordre : Nord, Est, Sud, Ouest
        edgePositions.Clear();
        edgePositions.Add(north ?? centerPosition);
        edgePositions.Add(east ?? centerPosition);
        edgePositions.Add(south ?? centerPosition);
        edgePositions.Add(west ?? centerPosition);
        
        // Calcul des coins (NE, SE, SW, NW)
        cornerPositions.Clear();
        Vector3? ne = null, se = null, sw = null, nw = null;
        float maxDistNE = 0f, maxDistSE = 0f, maxDistSW = 0f, maxDistNW = 0f;

        foreach (var plane in planes)
        {
            var mesh = plane.GetComponent<MeshFilter>()?.mesh;
            if (mesh == null) continue;

            foreach (var vertex in mesh.vertices)
            {
                Vector3 worldPos = plane.transform.TransformPoint(vertex);
                float dist = Vector3.Distance(centerPosition, worldPos);
                if (dist < 0.5f || dist > 3f) continue;

                Vector3 dir = worldPos - centerPosition;

                if (dir.x >= 0 && dir.z >= 0 && dist > maxDistNE)
                {
                    ne = worldPos;
                    maxDistNE = dist;
                }
                else if (dir.x >= 0 && dir.z < 0 && dist > maxDistSE)
                {
                    se = worldPos;
                    maxDistSE = dist;
                }
                else if (dir.x < 0 && dir.z < 0 && dist > maxDistSW)
                {
                    sw = worldPos;
                    maxDistSW = dist;
                }
                else if (dir.x < 0 && dir.z >= 0 && dist > maxDistNW)
                {
                    nw = worldPos;
                    maxDistNW = dist;
                }
            }
        }

        cornerPositions.Add(ne ?? centerPosition);
        cornerPositions.Add(se ?? centerPosition);
        cornerPositions.Add(sw ?? centerPosition);
        cornerPositions.Add(nw ?? centerPosition);

        Debug.Log($"Placement data calculated. Center: {centerPosition}, Edge count: {edgePositions.Count}");
    }


    public void Setup()
    {

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Debug.Log("Scene 1 Setup");
            Instantiate(prefabManager.GetPrefab("TutorialDesk"), centerPosition + Vector3.up * 0.1f, Quaternion.identity);
            Instantiate(prefabManager.GetPrefab("DoorToStart"), edgePositions[3] + Vector3.up * 0.1f, Quaternion.identity);

        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            Debug.Log("Scene 2 (Start room) Setup");
            if (edgePositions.Count > 0)
            {
                Instantiate(prefabManager.GetPrefab("DoorToLabo"), edgePositions[0] + Vector3.up * 0.1f, Quaternion.identity);
                Instantiate(prefabManager.GetPrefab("DoorToSerre"), edgePositions[2] + Vector3.up * 0.1f, Quaternion.identity);
            }
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            Debug.Log("Scene 3 (Labo room) Setup");
            if (edgePositions.Count > 0)
            {
                Instantiate(prefabManager.GetPrefab("DoorToLibrairie"), edgePositions[2] + Vector3.up * 0.1f, Quaternion.identity);
                Instantiate(prefabManager.GetPrefab("Cauldron"), centerPosition + Vector3.up * 0.1f, Quaternion.identity);
                Instantiate(prefabManager.GetPrefab("DoorToStart"), edgePositions[0] + Vector3.up * 0.1f, Quaternion.identity);
            }
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            Debug.Log("Scene 4 (Librairie room) Setup");
            if (edgePositions.Count > 0)
            {
                Instantiate(prefabManager.GetPrefab("DoorToLabo"), edgePositions[2] + Vector3.up * 0.1f, Quaternion.identity);
            }
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            Debug.Log("Scene 5 (Serre room) Setup");
            if (edgePositions.Count > 0)
            {
                Instantiate(prefabManager.GetPrefab("DoorToStart"), edgePositions[2] + Vector3.up * 0.1f, Quaternion.identity);
                Instantiate(prefabManager.GetPrefab("DoorToUnderground"), edgePositions[1] + Vector3.up * 0.1f, Quaternion.identity);
            }
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            Debug.Log("Scene 6 (Underground room) Setup");
            if (edgePositions.Count > 0)
            {
                Instantiate(prefabManager.GetPrefab("DoorToSerreBis"), edgePositions[1] + Vector3.up * 0.1f, Quaternion.identity);
                Instantiate(prefabManager.GetPrefab("EnigmeValve"), centerPosition + Vector3.up * 0.1f, Quaternion.identity);
                Instantiate(prefabManager.GetPrefab("Mushroom Planter"), edgePositions[2] + Vector3.up * 0.1f, Quaternion.identity);
            }
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

        if (!isSetupDone)
        {
            Debug.LogWarning("Scene not ready, skipping setup.");
            return;
        }

        Setup();
    }
}
