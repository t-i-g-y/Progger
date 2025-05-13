using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _player;
    private void Start()
    {
        PlayerHealth.OnPlayerDied += GameOverScreen;
        _gameOverScreen.SetActive(false);
    }

    private void GameOverScreen()
    {
        _gameOverScreen.SetActive(true);
    }

    private void Respawn()
    {
        _gameOverScreen.SetActive(false);
    }
}
