using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToSystem : MonoBehaviour
{
    [SerializeField] private Transform goToMarker;
    [SerializeField] private Transform player;
    private bool canGoTo = false;
    private void Update()
    {
        if (canGoTo && Input.GetKeyDown(KeyCode.E))
        {
            PlayerGoTo();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("GoTo callable");
            canGoTo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canGoTo = false;
        }
    }

    private void PlayerGoTo()
    {
        player.position = goToMarker.position;
    }
}
