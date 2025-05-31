using UnityEngine;

public class MushroomWhite : MonoBehaviour, Collectible
{
    public int id { get; set; } = 12;
    public Sprite icon { get; set; }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        
    }

    public int Collect()
    {
        Destroy(this.gameObject);
        return this.id;
    }
}
