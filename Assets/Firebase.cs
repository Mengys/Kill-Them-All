using DG.Tweening.Core.Easing;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseInitializer : MonoBehaviour {

    [HideInInspector] public Firebase.FirebaseApp App;
    public static FirebaseInitializer Instance { get; private set; }

    private bool _isInitialized = false;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                App = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                _isInitialized = true;
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    private void OnApplicationPause(bool pause) {
        if (!_isInitialized) return;
        Debug.Log("SendEventGamePause");
        FirebaseAnalytics.LogEvent(
            "GamePause",
            new Parameter("Level", Statistics.Level),
            new Parameter("Attempt", Statistics.Attempt),
            new Parameter("PlayedTime", Time.time),
            new Parameter("Pause", pause.ToString())
        );
    }

    public void LogLevelStart(int level, int attempt) {
        if (!_isInitialized) return;
        FirebaseAnalytics.LogEvent(
            "LevelStart",
            new Parameter("Level", level),
            new Parameter("Time", Time.time),
            new Parameter("Time", attempt)
        );
    }

    public void LogLevelEnd(bool isWin, int level, float startTime) {
        if (!_isInitialized) return;
        FirebaseAnalytics.LogEvent(
            "LevelEnd",
            new Parameter("Level", level),
            new Parameter("IsWin", isWin.ToString()),
            new Parameter("Time", Time.time - startTime)
        );
    }
}
