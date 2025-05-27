using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class RaycastController : MonoBehaviour
{
    private GameObject placedPrefab;
    private int cornerNumber = 0;
    private bool isTapped = false;
    private float timeSinceLastTap = 0f;
    private Vector3[] corners = new Vector3[4];
    private ARRaycastManager arRaycastManager;
    public float holdTime;
    public UIHandler UIHandler;
    public bool isCornerSetupDone = false;
    public Camera arCamera;
    public Inventory inventory;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public GameObject PlacedPrefab
    {
        get => placedPrefab;
        set => placedPrefab = value;
    }

    void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
        UIHandler = FindAnyObjectByType<UIHandler>();
        StartCoroutine(UIHandler.UpdateInv("0"));
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
        if (!TryGetTouchPosition(out Vector2 touchPosition) || !isCornerSetupDone) return;
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
                    StartCoroutine(UIHandler.UpdateInv(this.inventory.ToString()));
                }
            }
        }
    }

    private void TouchCollectible(MonoBehaviour ObjectTouched)
    {
        if (Time.time - timeSinceLastTap >= holdTime)
        {
            Collectible CollectedObject = (Collectible)ObjectTouched;
            this.inventory.addItem(CollectedObject.Collect());
        }
    }

    private void TouchInteractible(MonoBehaviour ObjectTouched)
    {
        Debug.Log("TouchInteractible" +  ObjectTouched.GetType());
        Interactible IntercactedObject = (Interactible)ObjectTouched;
        if (Time.time - timeSinceLastTap >= holdTime)
        {
            IntercactedObject.LongInteract();
        }
        else
        {
            Debug.Log("TouchInteractible DANS LE ELSE");
            IntercactedObject.Interact(this.inventory);
        }
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

    public void SetupCorner()
    {
        corners[cornerNumber] = arCamera.transform.position;
        if (cornerNumber<3)
        {
            cornerNumber++;            
        }
        else
        {
            cornerNumber = 0;
        }
        this.isCornerSetupDone = !corners.Contains(Vector3.zero);
    }

    public void Setup()
    {
        Instantiate(UIHandler.TutoDesk, Vector3.Lerp(corners[0], corners[1], 0.5f)+Vector3.up, Quaternion.identity);
    }

}