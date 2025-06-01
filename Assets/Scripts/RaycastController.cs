using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class RaycastController : MonoBehaviour
{
    private GameObject placedPrefab;
    
    public SceneController sceneController;
    public UIHandler UIHandler;
    public Camera arCamera;
    public Inventory inventory;
    
    private ARRaycastManager arRaycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public float holdTime;
    private bool isTapped;
    private float timeSinceLastTap;
    public float interactionCooldown = 0.5f; 
    private float lastInteractionTime = -Mathf.Infinity;

    public GameObject PlacedPrefab
    {
        get => placedPrefab;
        set => placedPrefab = value;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        sceneController = FindAnyObjectByType<SceneController>();
        inventory = FindAnyObjectByType<Inventory>();
        UIHandler = FindAnyObjectByType<UIHandler>();
        UIHandler.UpdateInv(inventory);
    }

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
        if (Time.time - lastInteractionTime < interactionCooldown)
            return;
        if (!TryGetTouchPosition(out Vector2 touchPosition) || !sceneController.isSetupDone) return;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            IsTappedSet(touch);
            if (isTapped)
            {
                touchPosition = touch.position;
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;

                if (Physics.Raycast(ray, out hitObject))
                {
                    MonoBehaviour ObjectTouched = hitObject.transform.GetComponent<MonoBehaviour>();
                    Debug.Log(ObjectTouched.GetType());
                    switch (ObjectTouched)
                    {
                        case Interactible:
                        {
                            TouchInteractible(ObjectTouched);
                            break;
                        }
                        case Collectible:
                        {
                            Debug.Log("collectible");
                            TouchCollectible(ObjectTouched);
                            break;
                        }
                        default:
                        {
                            if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon) && (Time.time - timeSinceLastTap <= holdTime))
                            {
                                var hitPose = hits[0].pose;
                                Instantiate(placedPrefab, hitPose.position+Vector3.up, hitPose.rotation);
                            }
                            break;
                        }
                    }
                    UIHandler.UpdateInv(inventory);
                }
            }
        }
    }

    private void TouchCollectible(MonoBehaviour ObjectTouched)
    {
        if (Time.time - timeSinceLastTap >= holdTime)
        {
            Collectible CollectedObject = (Collectible)ObjectTouched;

            if (inventory.contains(CollectedObject.id) == -1)
            {
                inventory.addItem(CollectedObject.Collect());
                lastInteractionTime = Time.time;
            }
        }
    }


    private void TouchInteractible(MonoBehaviour ObjectTouched)
    {
        Interactible InteractedObject = (Interactible)ObjectTouched;
        if (Time.time - timeSinceLastTap >= holdTime)
        {
            InteractedObject.LongInteract();
        }
        else
        {
            InteractedObject.Interact(this.inventory);
        }

        lastInteractionTime = Time.time;
    }

    private void IsTappedSet(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                isTapped = true;
                timeSinceLastTap = Time.time;
                break;
            case TouchPhase.Ended:
                isTapped = false;
                timeSinceLastTap = 0f;
                break;
            case TouchPhase.Moved:
                break;
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Canceled:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}