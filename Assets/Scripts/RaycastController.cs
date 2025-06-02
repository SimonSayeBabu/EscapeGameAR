using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class RaycastController : MonoBehaviour
{
    [SerializeField] private GameObject placedPrefab;

    public SceneController sceneController;
    public UIHandler UIHandler;
    public Camera arCamera;
    public Inventory inventory;

    private ARRaycastManager arRaycastManager;
    private static readonly List<ARRaycastHit> hits = new();

    public float holdTime = 0.5f;
    public float interactionCooldown = 0.5f;

    private float lastInteractionTime = -Mathf.Infinity;
    private float timeSinceLastTap;
    private bool isTapped;

    public GameObject PlacedPrefab
    {
        get => placedPrefab;
        set => placedPrefab = value;
    }

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        sceneController = FindAnyObjectByType<SceneController>();
        inventory = FindAnyObjectByType<Inventory>();
        UIHandler = FindAnyObjectByType<UIHandler>();

        if (UIHandler == null)
            Debug.LogError("[RaycastController] UIHandler not found!");
        else
            UIHandler.UpdateInv(inventory);
    }

    void Update()
    {
        if (Time.time - lastInteractionTime < interactionCooldown)
            return;

        if (!TryGetTouchPosition(out Vector2 touchPosition) || !sceneController.isSetupDone)
            return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            IsTappedSet(touch);

            if (isTapped)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    var interactible = hit.transform.GetComponentInParent<Interactible>();
                    var collectible = hit.transform.GetComponentInParent<Collectible>();

                    if (interactible != null)
                    {
                        TouchInteractible(interactible);
                    }
                    else if (collectible != null)
                    {
                        TouchCollectible(collectible);
                    }
                    else
                    {
                        TryPlacePrefab(touchPosition); // ➕ UNIQUEMENT si rien d'autre touché
                    }

                    UIHandler?.UpdateInv(inventory);
                }
                else
                {
                    // ➕ Cas où rien n’a été touché par le Physics.Raycast, donc on tente un ARRaycast
                    TryPlacePrefab(touchPosition);
                }

            }
        }
    }

    private void TryPlacePrefab(Vector2 touchPosition)
    {
        if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon) &&
            (Time.time - timeSinceLastTap <= holdTime))
        {
            Pose hitPose = hits[0].pose;
            Instantiate(placedPrefab, hitPose.position + Vector3.up, hitPose.rotation);
        }
    }

    private void TouchCollectible(Collectible target)
    {
        if (Time.time - timeSinceLastTap >= holdTime)
        {
            if (inventory.contains(target.id) == -1)
            {
                inventory.addItem(target.Collect());
                lastInteractionTime = Time.time;
            }
        }
    }

    private void TouchInteractible(Interactible target)
    {
        if (Time.time - timeSinceLastTap >= holdTime)
        {
            target.LongInteract(inventory);
        }
        else
        {
            if (target is Book book && book.uiHandler == null)
            {
                book.uiHandler = UIHandler;
            }

            target.Interact(inventory);
        }

        lastInteractionTime = Time.time;
    }

    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
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
            case TouchPhase.Canceled:
                isTapped = false;
                break;
        }
    }
}
