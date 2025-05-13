using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _player;
    public static event Action OnReset;
    private void Start()
    {
        PlayerHealth.OnPlayerDied += GameOverScreen;
        _gameOverScreen.SetActive(false);
    }

    private void GameOverScreen()
    {
        _gameOverScreen.SetActive(true);
    }

    public void Respawn()
    {
        _gameOverScreen.SetActive(false);
        _player.transform.position = new Vector3(0, 0, 0);
        OnReset?.Invoke();
    }
}
