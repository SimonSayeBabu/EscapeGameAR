using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class RaycastController : MonoBehaviour
{

    [SerializeField]
    private GameObject placedPrefab;
    public Camera arCamera;
    private bool isTapped = false;
    private float timeSinceLastTap = 0f;
    public float holdTime;

    public GameObject PlacedPrefab
    {
        get
        {
            return placedPrefab;
        }
        set
        {
            placedPrefab = value;
        }
    }

    private ARRaycastManager arRaycastManager;

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;

        return false;
    }

    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;
        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isTapped = true;
                timeSinceLastTap = Time.time;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                isTapped = false;
                timeSinceLastTap = 0f;
            }

            if (isTapped)
            {
                touchPosition = touch.position;

                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    MonoBehaviour ObjectTouched = hitObject.transform.GetComponent<MonoBehaviour>();
                    if (ObjectTouched is Interactible)
                    {
                        Interactible IntercactedObject = (Interactible)ObjectTouched;
                        if (Time.time - timeSinceLastTap >= holdTime)
                        {
                            IntercactedObject.LongInteract();
                        }
                        else
                        {
                            IntercactedObject.Interact();
                        }
                        return;
                    }
                    else
                    {
                        if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                        {
                            var hitPose = hits[0].pose;
                            Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                            return;
                        }
                    }
                }
            }
        }
    }


    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
}