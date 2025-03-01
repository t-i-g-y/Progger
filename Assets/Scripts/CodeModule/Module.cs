using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [SerializeField] private ModuleConsole console;
    [SerializeField] private Renderer codeableAreaRenderer;
    private Bounds codeableArea;
    private void Awake()
    {
        Bounds codeableArea = codeableAreaRenderer.bounds;
    }

    private void Update()
    {
        
    }
}
