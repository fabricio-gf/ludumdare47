using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ToiletIcon : MonoBehaviour
{
    // Start is called before the first frame update

    public void StartShaking()
    {
        transform.DOPunchRotation(new Vector3(0, 0,10), 0.15f).SetLoops(-1, LoopType.Yoyo).SetId("this");
    }

    public void StopShaking()
    {
        DOTween.Kill("this");
    }
}
