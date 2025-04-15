using UnityEngine;

public class MushroomPale : MonoBehaviour, Collectible
{
    public int id { get; set; } = 15;
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
