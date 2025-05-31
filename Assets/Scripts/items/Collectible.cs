using UnityEngine;

public interface Collectible
{
    int id { get; set; }
    Sprite icon { get; set; }
    void Interact();

    int Collect();
}
