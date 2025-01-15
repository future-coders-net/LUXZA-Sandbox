using Firebase.Analytics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using Firebase.Crashlytics;
using TMPro;

public class FirebaseAnalyticsTest : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private Button btnSetCrash;
    [SerializeField] private Button btnTest1;
    [SerializeField] private Button btnTest2;
    [SerializeField] private Button btnTest3;

    [SerializeField] private Button btnTest5;
    [SerializeField] private Button btnTest6;
    [SerializeField] private Button btnTest7;
    [SerializeField] private Button btnTest8;
    [SerializeField] private Button btnNone;


    public State SetupState { get; protected set; } = State.Init;

    public enum State
    {
        Init,
        Success,
        Fail
    }

    void Start()
    {
        btnSetCrash.onClick.AddListener(TestCrash);
        btnTest1.onClick.AddListener(Test1);
        btnTest2.onClick.AddListener(Test2);
        btnTest3.onClick.AddListener(Test3);

        btnTest5.onClick.AddListener(Test5);
        btnTest6.onClick.AddListener(Test6);
        btnTest7.onClick.AddListener(Test7);
        btnTest8.onClick.AddListener(Test8);

        Initialize();
    }

    private void Initialize()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                Crashlytics.ReportUncaughtExceptionsAsFatal = true;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
                SetupState = State.Success;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
                SetupState = State.Fail;
            }
        });

        // 新しい方法で全てのボタンを取得
        List<Button> buttons = new List<Button>();
        GameObject[] allObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject obj in allObjects)
        {
            buttons.AddRange(obj.GetComponentsInChildren<Button>(true)); // 非アクティブなボタンも含む
        }

        // 各ボタンにリスナーを追加
        foreach (Button button in buttons)
        {
            string buttonName = button.name;
            button.onClick.AddListener(() => LogButtonClick(buttonName));
        }

    }

    void LogButtonClick(string buttonName)
    {
        // Firebase Analyticsでイベントを記録
        FirebaseAnalytics.LogEvent("button_click", "button_name", buttonName);
        Debug.Log($"Button clicked: {buttonName}");
    }

    private void TestCrash()
    {
        Debug.Log("TestCrash start");
        label.text = "TestCrash clicked";
        throw new System.Exception("(ignore) this is a test crash");
    }

    private void Test1()
    {
        label.text = "Test1";
        Firebase.Analytics.FirebaseAnalytics.LogEvent(
            Firebase.Analytics.FirebaseAnalytics.EventPostScore,
            Firebase.Analytics.FirebaseAnalytics.ParameterScore,
            42
          );
    }
    private void Test2()
    {
        label.text = "Test2";
        Firebase.Analytics.FirebaseAnalytics.LogEvent(
            Firebase.Analytics.FirebaseAnalytics.EventJoinGroup,
            FirebaseAnalytics.ParameterGroupID,
            "spoon_welders"
          );
    }
    private void Test3()
    {
        label.text = "Test3";
        Firebase.Analytics.FirebaseAnalytics.LogEvent("progress", "percent", 0.4f);
    }

    private void Test5()
    {
        label.text = "Test5";
        btnNone.tag = "hello world";
    }

    private void Test6()
    {
        label.text = "Test6";
    }

    private void Test7()
    {
        label.text = "Test7";
    }

    private void Test8()
    {
        label.text = "Test8";
    }
}

