using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HorizontalSnakeManager : MonoBehaviour
{
    [SerializeField] private GameObject _snakePrefab;
    [SerializeField] private Transform _player;
    [SerializeField] private CameraFollow _cameraFollow;

    private HorizontalSnakeController activeSnake;
    private bool controllingSnake;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG).transform;
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
        GameObject snakeObj = Instantiate(_snakePrefab, _player.position, Quaternion.identity);
        activeSnake = snakeObj.GetComponent<HorizontalSnakeController>();
        controllingSnake = true;

        PlayerMovement.InputBlocked = true;
        _cameraFollow.SetTarget(snakeObj.transform);
    }

    public void ReturnToPlayer()
    {
        if (activeSnake != null)
            Destroy(activeSnake.gameObject);

        PlayerMovement.InputBlocked = false;
        _cameraFollow.SetTarget(_player);
        controllingSnake = false;
    }
}
