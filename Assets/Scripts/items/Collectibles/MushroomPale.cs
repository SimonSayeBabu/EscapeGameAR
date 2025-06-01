using UnityEngine;

public class MushroomPale : MonoBehaviour, Collectible
{
    public int id { get; set; } = 15;
    public bool active { get; set; } = true;
    [SerializeField] private Sprite _icon;
    public Sprite icon
    {
        get => _icon ?? Resources.Load<Sprite>("sprite/missing_texture");
        set => _icon = value;
    }
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

    public Collectible Collect()
    {
        Destroy(gameObject);
        return this;
    }
}
