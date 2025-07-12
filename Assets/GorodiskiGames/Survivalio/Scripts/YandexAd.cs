using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
public class YandexAd : MonoBehaviour
{

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void ShowInterstitialAd() {
        YG2.InterstitialAdvShow();
    }
}
