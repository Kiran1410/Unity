using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloadingScenePresenter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneLoader.Instance.SwitchScene(GameConstants.LOBBY_SCENE);
    }
}
