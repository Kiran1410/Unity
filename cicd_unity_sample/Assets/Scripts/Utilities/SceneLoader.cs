using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance = null;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public void SwitchScene(string sceneName) {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
