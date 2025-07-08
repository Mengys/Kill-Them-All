using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Statistics {
    //public static Statistics Instance { get; private set; }

    private static float _levelStartTime = 0;
    private static int _currentLevel = 0;
    private static int _attempt = 0;

    public static int Level => _currentLevel;
    public static int Attempt => _attempt;

    //private void Awake() {
    //    if (Instance == null) {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    } else {
    //        Destroy(gameObject);
    //    }
    //}

    public static void LevelEnd(bool isWin) {

        if (FirebaseInitializer.Instance != null) {
            FirebaseInitializer.Instance.LogLevelEnd(isWin, _currentLevel, _levelStartTime);
        }
    }

    public static void LevelStart(int level) {
        if (_currentLevel == level) {
            _attempt++;
        } else {
            _attempt = 0;
        }

        _currentLevel = level;
        _levelStartTime = Time.time;

        if (FirebaseInitializer.Instance != null) {
            FirebaseInitializer.Instance.LogLevelStart(_currentLevel, _attempt);
        }
    }
}
