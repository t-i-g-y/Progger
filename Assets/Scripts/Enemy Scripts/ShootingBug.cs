using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ShootingBug : MonoBehaviour
{
    private float timer = 2f;
    private Transform bulletSpawnPoint;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _collisionDamage = .5f;
    [SerializeField] private float _minShootTime = 2f;
    [SerializeField] private float _maxShootTime = 5f;

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
        Instantiate(_bullet, bulletSpawnPoint.position, Quaternion.identity);
    }

    IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(Random.Range(_minShootTime, _maxShootTime));
        ShootBullet();

        StartCoroutine(StartShooting());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(_collisionDamage);
        }
    }
}
