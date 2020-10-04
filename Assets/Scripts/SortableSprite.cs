using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortableSprite : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer _spriteRenderer;
    public float Height => transform.position.y;
    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SpritesSorter.Instance.allSprites.Add(this);
    }

    private void OnDisable()
    {
        SpritesSorter.Instance.allSprites.Remove(this);
        SpritesSorter.Instance.allSprites.TrimExcess();
    }
}
