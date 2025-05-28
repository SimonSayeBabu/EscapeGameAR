using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Collectible
{
    int id { get; set; }
    void Interact();

    int Collect();
}
