using UnityEngine;
using System.Collections;

public class Drawers : MonoBehaviour, Interactible
{
    public int side = 1;
    public bool isLocked;
    private bool isOpen = false;

    private bool isAnimating = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    public float openAngle = 120f;
    public float animationDuration = 0.5f;
    private UIHandler uiHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        uiHandler = FindAnyObjectByType<UIHandler>();
        Debug.Log("Script on " + gameObject.name);
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle * side);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Interact(Inventory playerInventory)
    {
        Debug.Log("touched " + gameObject.name);

        if (isLocked)
        {
            if (playerInventory.contains(1) != -1)
            {
                if (!isAnimating)
                    StartCoroutine(RotateDrawer());
            }
            else
            {
                uiHandler.showDoorLocked();
            }
        }
        else
        {
            if (!isAnimating)
                StartCoroutine(RotateDrawer());
        }
    }

    public void LongInteract(Inventory playerInventory) {}
    
    private IEnumerator RotateDrawer()
    {
        isAnimating = true;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = isOpen ? closedRotation : openRotation;
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / animationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        isOpen = !isOpen;
        isAnimating = false;
    }
}
