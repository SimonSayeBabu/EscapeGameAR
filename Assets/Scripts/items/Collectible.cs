using UnityEngine;

public interface Collectible
{
    int id { get; set; }
    Sprite icon { get; set; }
    bool active { get; set; }
    
    void Interact();

    Collectible Collect();
}
