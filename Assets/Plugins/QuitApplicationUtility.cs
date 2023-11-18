using UnityEngine;

public class QuitApplicationUtility
{
    public static void MoveAndroidApplicationToBack()
    {
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call<bool>("moveTaskToBack", true);
    }
}



