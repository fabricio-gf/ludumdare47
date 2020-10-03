using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LoadingBehavior : MonoBehaviour
{
    public static LoadingBehavior Instance;

    public Animator loadingAnimator;
    
    private static readonly int FadeIn = Animator.StringToHash("FadeIn");
    private static readonly int FadeOut = Animator.StringToHash("FadeOut");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        loadingAnimator.SetTrigger(FadeIn);
    }

    public void LevelOpened()
    {
        loadingAnimator.SetTrigger(FadeOut);
    }
}
