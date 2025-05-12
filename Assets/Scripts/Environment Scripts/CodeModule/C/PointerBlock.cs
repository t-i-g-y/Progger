using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointerBlock : MonoBehaviour, ICSharpModifiable
{
    private Module module;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool _isActivated;
    [SerializeField] private int _sourceID;
    [SerializeField] private int _targetID;
    [SerializeField] private TMP_Text _targetText;
    [SerializeField] private TMP_Text _sourceText;
    [SerializeField] private float _bounceForce;
    [SerializeField] private Color _deactivatedColor = Color.white;
    [SerializeField] private Color _activatedColor = Color.gray;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public int SourceID
    {
        get => _sourceID;
        set => _sourceID = value;
    }

    public int TargetID
    {
        get => _targetID;
        set => _targetID = value;
    }
    
    public float BounceForce => _bounceForce;
    
    public void Initialize(int source, int target, Module module)
    {
        _sourceID = source;
        _targetID = target;
        this.module = module;

        if (_sourceText != null) 
            _sourceText.text = source.ToString();
        if (_targetText != null) 
            _targetText.text = target.ToString();
    }
    
    public bool TryUsePointerBlock()
    {
        if (module.CanChainBounce(this))
        {
            ActivatePointerBlock();
            return true;
        }
        
        return false;
    }

    private void ActivatePointerBlock()
    {
        _isActivated = true;
        spriteRenderer.color = _activatedColor;
    }

    public void DeactivatePointerBlock()
    {
        _isActivated = false;
        spriteRenderer.color = _deactivatedColor;
    }

    public void ApplyModuleComponent(ModuleObjectComponentType componentType)
    {
        
    }

    public void RemoveModuleComponent(ModuleObjectComponentType componentType)
    {
        
    }
}

