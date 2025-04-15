using UnityEngine;

public class KeyAlchemy : MonoBehaviour, Collectible
{
    public int id { get; set; } = 2;
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
