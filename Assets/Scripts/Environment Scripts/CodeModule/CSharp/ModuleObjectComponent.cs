using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModuleObjectComponentType
{
    Healer,
    Damage,
    Amplifier,
    Nullifier,
    Shield,
    Bounce,
    Catcher
}

public class ModuleObjectComponent : MonoBehaviour
{
    private Vector3 targetPosition;
    private GameObject targetObject;
    private ICSharpModifiable targetModifiable;
    private ModuleObjectComponentType componentType;

    public ModuleObjectComponentType ComponentType
    {
        get => componentType;
        set => componentType = value;
    }
    
    public GameObject TargetObject => targetObject;
    public Vector3 TargetPosition => targetPosition;

    public void Initialize(ModuleObjectComponentType componentType, Vector3 position, GameObject objectToModify)
    {
        ComponentType = componentType;
        targetPosition = position;
        targetObject = objectToModify;
        targetModifiable = targetObject.GetComponent<ICSharpModifiable>();

        if (targetModifiable != null)
        {
            targetModifiable.ApplyModuleComponent(componentType);
        }
    }

    public void Detach()
    {
        if (targetModifiable != null)
        {
            targetModifiable.RemoveModuleComponent(componentType);
        }
        targetObject = null;
        targetModifiable = null;
    }
}
