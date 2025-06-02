using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soup : MonoBehaviour, Collectible
{
    public int id { get; set; } = 31;
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
