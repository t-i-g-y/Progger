using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ModuleSaveHelper
{
    public static List<ModuleSaveData> SaveAllModules()
    {
        List<ModuleSaveData> result = new();
        foreach (Module module in GameObject.FindObjectsOfType<Module>())
        {
            var modData = new ModuleSaveData
            {
                moduleName = module.name,
                placedObjects = new List<PlacedObjectData>()
            };

            foreach (var kvp in module.PlacedObjects)
            {
                int prefabID = kvp.Value.GetComponent<ModuleObject>().PrefabID;
                modData.placedObjects.Add(new PlacedObjectData
                {
                    position = kvp.Key,
                    prefabID = prefabID
                });
            }

            result.Add(modData);
        }
        return result;
    }

    public static void LoadAllModules(List<ModuleSaveData> dataList)
    {
        foreach (var modData in dataList)
        {
            Module module = GameObject.Find(modData.moduleName)?.GetComponent<Module>();
            if (module == null) continue;

            foreach (var obj in modData.placedObjects)
            {
                GameObject prefab = ModuleRegistry.GetPrefabByID(obj.prefabID);
                GameObject go = GameObject.Instantiate(prefab, obj.position, Quaternion.identity, module.PlacedObjectsParent);
                module.RegisterPlacedObject(obj.position, go);
            }
        }
    }
}

