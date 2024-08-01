using UnityEngine;

public class DontDestroyOnLoadObjects : MonoBehaviour
{
    private static DontDestroyOnLoadObjects _instance = null;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        DontDestroyOnLoad(this.gameObject);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
