using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HorizontalSnakeManager : MonoBehaviour
{
    [SerializeField] private GameObject snakePrefab;
    [SerializeField] private Transform player;
    [SerializeField] private CameraFollow cameraFollow;

    private HorizontalSnakeController activeSnake;
    private bool controllingSnake;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG).transform;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !controllingSnake)
        {
            SpawnSnake();
        }
        else if (Input.GetKeyDown(KeyCode.F) && controllingSnake)
        {
            ReturnToPlayer();
        }
    }

    private void SpawnSnake()
    {
        GameObject snakeObj = Instantiate(snakePrefab, player.position, Quaternion.identity);
        activeSnake = snakeObj.GetComponent<HorizontalSnakeController>();
        controllingSnake = true;

        PlayerMovement.InputBlocked = true;
        cameraFollow.SetTarget(snakeObj.transform);
    }

    public void ReturnToPlayer()
    {
        if (activeSnake != null)
            Destroy(activeSnake.gameObject);

        PlayerMovement.InputBlocked = false;
        cameraFollow.SetTarget(player);
        controllingSnake = false;
    }
}
