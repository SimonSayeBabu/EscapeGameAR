using UnityEngine;
using System.Collections.Generic;

public class PrefabManager : MonoBehaviour
{
    public List<GameObject> prefabList;

    private Dictionary<string, GameObject> prefabDict;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        prefabDict = new Dictionary<string, GameObject>();

        foreach (GameObject prefab in prefabList)
        {
            if (prefab != null)
            {
                string key = prefab.name;

                if (!prefabDict.TryAdd(key, prefab))
                {
                    Debug.LogWarning($"PrefabManager: Clé en double détectée : {key}");
                }
            }
        }
    }

    public GameObject GetPrefab(string prefabName)
    {
        if (prefabDict.TryGetValue(prefabName, out GameObject prefab))
        {
            return prefab;
        }

        Debug.LogError($"PrefabManager: Aucun prefab trouvé avec le nom '{name}'");
        return null;
    }
}