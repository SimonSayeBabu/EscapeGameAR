using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactible
{
    void Interact(Inventory playerInventory = null);

    void LongInteract(Inventory playerInventory = null);
}
