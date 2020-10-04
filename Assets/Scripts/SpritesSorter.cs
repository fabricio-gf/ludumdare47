using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpritesSorter : MonoBehaviour
{
    private static SpritesSorter _instance;

    public static SpritesSorter Instance
    {
        get
        {
            if (_instance == null)
            {
                var spritesSorter = new GameObject("SpritesSorter");
                var newInstance = spritesSorter.AddComponent<SpritesSorter>();
                _instance = newInstance;
            }
            return _instance;
        }
    }

    [HideInInspector] public List<SortableSprite> allSprites = new List<SortableSprite>();

    private void Update()
    {
        List<SortableSprite> sortedSprites = allSprites.OrderBy(o=>o.Height).ToList();
        sortedSprites.Reverse();
        for (int i = 0; i < sortedSprites.Count; i++)
        {
            sortedSprites[i]._spriteRenderer.sortingOrder = i;
        }
    }
}
