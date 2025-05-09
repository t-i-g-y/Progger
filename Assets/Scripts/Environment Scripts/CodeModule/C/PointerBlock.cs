using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointerBlock : MonoBehaviour
{
    private Module module;
    private bool isActivated;
    
    [SerializeField] private int _sourceID;
    [SerializeField] private int _targetID;
    [SerializeField] private TMP_Text _targetText;
    [SerializeField] private TMP_Text _sourceText;
    [SerializeField] private float _bounceForce;

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
    
    private void HandlePlayerBounce(GameObject player)
    {
        Rigidbody2D playerBody = player.GetComponent<Rigidbody2D>();
        playerBody.velocity = new Vector2(playerBody.velocity.x, 0f);
        playerBody.AddForce(Vector2.up * _bounceForce, ForceMode2D.Impulse);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (isActivated)
            return;
        
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            if (Input.GetButtonDown(TagManager.JUMP_BUTTON) && module.CanChainBounce(this))
            {
                Debug.Log("Bounced");
                isActivated = true;
                HandlePlayerBounce(other.gameObject);
            }
        }
    }
}

