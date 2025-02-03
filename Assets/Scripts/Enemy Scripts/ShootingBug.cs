using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShootingBug : MonoBehaviour
{
    private float timer = 2f;
    private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float collisionDamage = .5f;
    [SerializeField] private float minShootTime = 2f, maxShootTime = 5f;

    private void Awake()
    {
        bulletSpawnPoint = transform.GetChild(0).transform;
    }
    
    private void Start()
    {
        StartCoroutine(StartShooting());
    }

    private void ShootBullet()
    {
        Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);
    }

    IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(Random.Range(minShootTime, maxShootTime));
        ShootBullet();

        StartCoroutine(StartShooting());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(collisionDamage);
        }
    }
}
