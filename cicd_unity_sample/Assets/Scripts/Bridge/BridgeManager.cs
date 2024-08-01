using UnityEngine;

#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

public class BridgeManager : MonoBehaviour
{

#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern float messageFromGE(string message);
#endif


    public static void SendMessageToIOS(string messageToHub)
    {
#if UNITY_EDITOR
        Debug.Log("UnityDebug:<color=#00FFFF> messageToNative: </color>" + messageToHub);
#else
        Debug.Log("UnityDebug: messageToNative: " + messageToHub);
#endif

#if UNITY_IOS && !UNITY_EDITOR
         messageFromGE(messageToHub);
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
         messageToAndroid(messageToHub);
#endif
    }

    static void messageFromNative(string message)
    {
        Debug.Log(message);
    }

    static void messageToAndroid(string message)
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        AndroidJavaClass myClass = new AndroidJavaClass("com.bwin.bridge.NotificationMessageCenter");
        myClass.CallStatic<string>("messageFromUnity", message);
#endif
    }
}
