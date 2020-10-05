using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    private int currentStep = 1;
    private Animator animator;
    private string animatorTrigger;

    public Animator arrowAnimator;
    private bool isShowingArrow = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            NextStep();
        }
    }

    public void NextStep()
    {
        currentStep++;

        if (currentStep == 13)
        {
            SceneLoader.Instance.LoadScene("Menu");
            //SceneManager.LoadScene("Menu");
        }
        
        if (isShowingArrow)
        {
            arrowAnimator.SetTrigger("HideArrow");
            isShowingArrow = false;
        }
        animatorTrigger = "Step" + currentStep;
        animator.SetTrigger(animatorTrigger);
    }

    public void ShowArrow()
    {
        arrowAnimator.SetTrigger("ShowArrow");
        isShowingArrow = true;
    }
    
}
