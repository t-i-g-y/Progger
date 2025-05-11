using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HeartType
{
    FullHeart = 0,
    HalfHeart = 1,
    EmptyHeart = 2,
    FullGoldHeart = 3,
    HalfGoldHeart = 4,
    NoHeart = 5
}

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Sprite[] _heartSprites;
    [SerializeField] private Image _heartImagePrefab;
    private List<Image> hearts = new List<Image>();

    public void SetMaxHearts(float health)
    {
        foreach (Image heart in hearts)
            Destroy(heart.gameObject);
        
        hearts.Clear();

        for (int i = 0; i < (int)health; i++)
        {
            Image newHeart = Instantiate(_heartImagePrefab, transform);
            newHeart.sprite = _heartSprites[(int)HeartType.FullHeart];
            hearts.Add(newHeart);
        }
    }
    
    public void UpdateHearts(float health)
    {
        int heartIndex = (int)(health * 2);
        for (int i = 0; i < hearts.Count; i++)
        {
            if (heartIndex > 1)
                hearts[i].sprite = _heartSprites[(int)HeartType.FullHeart];
            else if (heartIndex > 0)
                hearts[i].sprite = _heartSprites[(int)HeartType.HalfHeart];
            else
                hearts[i].sprite = _heartSprites[(int)HeartType.EmptyHeart];
            heartIndex -= 2;
        }
    }
}
