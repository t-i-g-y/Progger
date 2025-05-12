using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;

public class PushMoveableObject : MonoBehaviour
{
    [SerializeField] private float _pushForce;
    private bool nearPushableObject;
    private bool isPushing;
    private BoxCollider2D boxCol2D;
    private Rigidbody2D myBody;
    private Rigidbody2D pushedObject;

    private void Awake()    
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (pushedObject && Input.GetKey(KeyCode.E))
            isPushing = true;
        else
            isPushing = false;
    }

    private void FixedUpdate()
    {
        if (nearPushableObject && isPushing)
        {
            int pushDirection = 0;
            if (myBody.velocity.x > 0)
                pushDirection = 1;
            else if (myBody.velocity.x < 0)
                pushDirection = -1;
            pushedObject.velocity = new Vector2(pushDirection * _pushForce, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.MOVEABLE_OBJECT_TAG))
        {
            nearPushableObject = true;
            pushedObject = other.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.MOVEABLE_OBJECT_TAG))
        {
            nearPushableObject = false;
            if (pushedObject)
            {
                pushedObject.velocity = Vector2.zero;
                pushedObject.drag = 10f;
            }
            pushedObject = null;
        }
    }
    
}
