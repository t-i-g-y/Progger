using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Trap : MonoBehaviour
{
    [SerializeField] private float _bounceForce;
    [SerializeField] private float _trapDamage;
    
    private void HandlePlayerBounce(GameObject player)
    {
        Rigidbody2D playerBody = player.GetComponent<Rigidbody2D>();
        playerBody.velocity = new Vector2(playerBody.velocity.x, 0f);
        playerBody.AddForce(Vector2.up * _bounceForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            if (_trapDamage > 0)
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(_trapDamage);
            }
            HandlePlayerBounce(collision.gameObject);
        }
    }
}
