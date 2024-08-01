using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

public class GameScenePresenter : MonoBehaviour
{

    [SerializeField] GameSceneView _view;

    public void SendRequestToNative()
    {
        Dictionary<string, object> backToLobby = new Dictionary<string, object>
        {
            { "type", MessageConstants.UnloadGame}
        };

        string jsonRequest = Json.Serialize(backToLobby);

        NativeMessageHandler.instance.SendMessagetoNative(jsonRequest);

        // for Testing
        //SceneLoader.Instance.SwitchScene(GameConstants.LOBBY_SCENE);
    }

}
