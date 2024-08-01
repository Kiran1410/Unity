using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySceneView : MonoBehaviour
{
    [SerializeField] LobbyScenePresenter lsp;
    [SerializeField] private Button startBtn;

    public void OnStartClick() {
        lsp.SendRequestToNative();
    }
}
