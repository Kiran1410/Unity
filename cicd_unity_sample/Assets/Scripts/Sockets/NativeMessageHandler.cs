using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

public class NativeMessageHandler : MonoBehaviour
{
    private static NativeMessageHandler _instance = null;
    public static NativeMessageHandler instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(NativeMessageHandler)) as NativeMessageHandler;
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    public void SendMessagetoNative(string messageStr)
    {
        BridgeManager.SendMessageToIOS(messageStr);
    }

    public void MessageFromNative(string jsonData)
    {

#if UNITY_EDITOR
        Debug.Log("<color=#00FF00> Response: </color>" + jsonData);
#else
        Debug.Log("UnityDebug: messageFromNative: " + jsonData);
#endif
        Dictionary<string, object> totalData = Json.Deserialize(jsonData) as Dictionary<string, object>;

        string messageGroup = totalData[MessageConstants.MainMessageType] as string;

        //Arguments Data
        Dictionary<string, object> messageArgs = totalData[MessageConstants.MessageArgs] as Dictionary<string, object>;

        if (messageGroup.Equals(MessageConstants.GameAvailable))
        {
            SceneLoader.Instance.SwitchScene(GameConstants.GAME_SCENE);
        }
        else if (messageGroup == MessageConstants.UnloadGame)
        {
            SceneLoader.Instance.SwitchScene(GameConstants.LOBBY_SCENE);
        }
    }
}
