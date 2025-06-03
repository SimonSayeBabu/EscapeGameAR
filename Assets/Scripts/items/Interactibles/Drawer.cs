using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drawer : MonoBehaviour, Interactible
{
    private bool isOpen = false;
    private bool isAnimating = false;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    public List<Rigidbody> objectsInsideDrawer;

    public float openDistance = 0.25f;        // Distance à tirer
    public float animationDuration = 0.5f;    // Temps de l’animation en secondes
    public Vector3 openDirection = Vector3.forward; // Direction d’ouverture (locale)

    void Start()
    {
        closedPosition = transform.localPosition;
        openPosition = closedPosition + Vector3.Scale(openDirection.normalized, new Vector3(openDistance, openDistance, openDistance));
    }

    public void Interact(Inventory playerInventory)
    {
        if (!isAnimating)
        {
            StartCoroutine(isOpen ? CloseDrawer() : OpenDrawer());
        }
    }

    public void LongInteract(Inventory playerInventory) { }

    private IEnumerator OpenDrawer()
    {
        yield return AnimateDrawer(transform.localPosition, openPosition, true);
    }

    private IEnumerator CloseDrawer()
    {
        yield return AnimateDrawer(transform.localPosition, closedPosition, false);
    }

    private List<Transform> originalParents = new List<Transform>();

    private IEnumerator AnimateDrawer(Vector3 start, Vector3 end, bool opening)
    {
        isAnimating = true;

        if (opening)
        {
            originalParents.Clear();
            foreach (var rb in objectsInsideDrawer)
            {
                if (rb != null)
                {
                    originalParents.Add(rb.transform.parent);
                    rb.transform.SetParent(transform);
                    // Ne touche pas à isKinematic
                }
            }
        }

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            transform.localPosition = Vector3.Lerp(start, end, elapsed / animationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = end;
        isOpen = opening;

        // Après animation, on déparent les objets
        for (int i = 0; i < objectsInsideDrawer.Count; i++)
        {
            var rb = objectsInsideDrawer[i];
            if (rb != null)
            {
                rb.transform.SetParent(originalParents[i] != null ? originalParents[i] : null);
            }
        }

        isAnimating = false;
    }

}