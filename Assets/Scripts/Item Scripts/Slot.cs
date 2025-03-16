using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public GameObject _currentItem;

    public GameObject CurrentItem
    {
        get => _currentItem;
        set => _currentItem = value;
    }
}
