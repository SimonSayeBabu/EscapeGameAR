using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneDetectionController : MonoBehaviour
{
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private ARSession arSession;
    [SerializeField] private GameObject frozenPlanePrefab;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (arPlaneManager == null)
        {
            arPlaneManager = FindObjectOfType<ARPlaneManager>();
        }

        arPlaneManager.enabled = false;
        SetPlanesVisible(false);
    }

    public void StartPlaneDetection()
    {
        arPlaneManager.enabled = true;
        SetPlanesVisible(true);
        Debug.Log("Plane detection started.");
    }
    
    public void StopPlaneDetection()
    {
        arPlaneManager.requestedDetectionMode = PlaneDetectionMode.None;
        arPlaneManager.enabled = false;
        foreach (var plane in arPlaneManager.trackables)
        {
            plane.enabled = false;
        }

        Debug.Log("Plane detection and new plane instantiation stopped.");
    }



    public void ClearAllPlanes()
    {
        foreach (var plane in arPlaneManager.trackables)
        {
            Destroy(plane.gameObject);
        }

        Debug.Log("All planes cleared.");
    }

    public void FullRestartPlaneDetection()
    {
        StartCoroutine(ResetSessionAndRestart());
    }

    private IEnumerator ResetSessionAndRestart()
    {
        arPlaneManager.enabled = false;

        arSession.Reset();

        yield return new WaitForSeconds(0.5f);

        arPlaneManager.enabled = true;

        Debug.Log("AR Session and Plane Detection restarted.");
    }

    
    private void SetPlanesVisible(bool visible)
    {
        foreach (var plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(visible);
        }
    }
}