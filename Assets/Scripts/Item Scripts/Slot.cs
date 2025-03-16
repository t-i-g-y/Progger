using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private GameObject _currentItem;

    public GameObject CurrentItem
    {
        get => _currentItem;
        set => _currentItem = value;
    }
}
