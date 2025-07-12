using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshoter : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            // Имя файла скриншота с текущим временем для уникальности
            string fileName = "Screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

            // Делает скриншот с увеличением разрешения в 2 раза (можно изменить)
            ScreenCapture.CaptureScreenshot(fileName);

            Debug.Log("Скриншот сохранён: " + fileName);
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            Time.timeScale = 0f;
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            Time.timeScale = 1f;
        }
    }
}
