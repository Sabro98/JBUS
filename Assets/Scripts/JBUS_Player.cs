using System;
using UnityEngine;


[System.Serializable]
public class JBUS_Player : ICloneable
{
    public string playerID;
    public string playerNickName;
    public string playerModel;

    //ref: https://docs.unity3d.com/ScriptReference/JsonUtility.FromJson.html
    public static JBUS_Player CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<JBUS_Player>(jsonString);
    }

    public object Clone()
    {
        JBUS_Player copy = new JBUS_Player();
        copy.playerNickName = this.playerNickName;
        copy.playerModel = this.playerModel;
        copy.playerID = this.playerID;

        return copy;
    }

    public JBUS_Player()
    {

    }

    public JBUS_Player(string name, string model, string id)
    {
        playerNickName = name;
        playerModel = model;
        playerID = id;
    }
}
