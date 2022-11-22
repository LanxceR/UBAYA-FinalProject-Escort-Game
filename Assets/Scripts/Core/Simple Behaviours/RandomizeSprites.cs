using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for various sprite randomizations functionality
/// </summary>
public class RandomizeSprites : MonoBehaviour
{    
    [Header("Components Settings")]
    [SerializeField] internal SpriteRenderer spriteRenderer;

    [Header("Sprites Settings")]
    [SerializeField] internal Sprite[] sprites;

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
