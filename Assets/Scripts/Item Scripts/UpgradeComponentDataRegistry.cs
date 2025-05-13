using System.Collections.Generic;
using UnityEngine;

public class UpgradeComponentDataRegistry : MonoBehaviour
{
    public static UpgradeComponentDataRegistry Instance;

    [System.Serializable]
    public class ComponentEntry
    {
        public int id;
        public UpgradeComponentData data;
    }

    [SerializeField] private List<ComponentEntry> components = new();

    private Dictionary<int, UpgradeComponentData> dataLookup = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            foreach (var entry in components)
                dataLookup[entry.id] = entry.data;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static UpgradeComponentData GetByID(int id)
    {
        return Instance.dataLookup.TryGetValue(id, out var data) ? data : null;
    }
}

