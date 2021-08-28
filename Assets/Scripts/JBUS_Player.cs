using System;
using UnityEngine;

//ref: https://docs.unity3d.com/ScriptReference/JsonUtility.FromJson.html

[System.Serializable]
public class JBUS_Player
{
    public string playerNickName;
    public string playerModel;

    public static JBUS_Player CreateFromJSON(string jsonString)
    {
        var tmp = JsonUtility.FromJson<JBUS_Player>(jsonString);
        Debug.Log(tmp.playerModel);
        Debug.Log(tmp.playerNickName);
        return JsonUtility.FromJson<JBUS_Player>(jsonString);
    }
}
