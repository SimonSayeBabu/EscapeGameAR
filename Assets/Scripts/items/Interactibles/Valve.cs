using System.Collections;
using UnityEngine;

public class Valve : MonoBehaviour, Interactible
{
    public EnigmeValve enigmeValve;
    public int nValve; 

    private int isOpen = 0;
    private bool isAnimating = false;
    private Coroutine currentAnimation;

    void Start()
    {
        if (enigmeValve.IsValveOpen(nValve))
        {
            isOpen = 1;
            transform.rotation = Quaternion.Euler(0, -45f, 0);
        }
    }

    public void Interact(Inventory playerInventory)
    {
        if ((isOpen == 1 || enigmeValve.CanOpenAnotherValve()) && !isAnimating)
        {
            isOpen = (isOpen + 1) % 2;

            if (currentAnimation != null)
                StopCoroutine(currentAnimation);

            currentAnimation = StartCoroutine(AnimateValve(isOpen == 1));
            enigmeValve.SetValve(isOpen, nValve);
        }
    }

    private IEnumerator AnimateValve(bool opening)
    {
        isAnimating = true;

        float duration = 0.5f;
        float elapsed = 0f;
        float angle = opening ? -45f : 45f;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = startRot * Quaternion.Euler(0, angle, 0);

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRot;
        yield return new WaitForSeconds(0.3f);
        isAnimating = false;
    }

    public void LongInteract(Inventory playerInventory) { }
}