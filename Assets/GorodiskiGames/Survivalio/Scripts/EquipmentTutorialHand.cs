using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentTutorialHand : MonoBehaviour
{
    private void Start() {
        transform.DOScale(2,1).SetEase(Ease.InOutSine).SetLoops(-1,LoopType.Yoyo);
    }
}
