using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICSharpModifiable
{
    void ApplyModuleComponent(ModuleObjectComponentType componentType);
    void RemoveModuleComponent(ModuleObjectComponentType componentType);
}
