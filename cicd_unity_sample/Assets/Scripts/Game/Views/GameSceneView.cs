using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneView : MonoBehaviour
{
    [SerializeField] GameScenePresenter gsp;
    [SerializeField] private Button backToLobbyBtn;

    public void OnBackToLobbyClick()
    {
        gsp.SendRequestToNative();
    }
}
