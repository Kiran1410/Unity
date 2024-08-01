using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class LobbyScenePresenter : MonoBehaviour
{
    [SerializeField] LobbySceneView _view;

    public void SendRequestToNative() {

        Dictionary<string, object> initialisedRequest = new Dictionary<string, object>
        {
             { "type", MessageConstants.Iinitialised}
        };

        string jsonRequest = Json.Serialize(initialisedRequest);

        NativeMessageHandler.instance.SendMessagetoNative(jsonRequest);

        // For Testing
        //SceneLoader.Instance.SwitchScene(GameConstants.GAME_SCENE);
    }

}
