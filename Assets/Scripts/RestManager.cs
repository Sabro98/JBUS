using System;
using UnityEngine.Networking;

public class RestManager
{
    const string BASE_URL = "https://jbus.herokuapp.com/"; // on cloud
    //const string BASE_URL = "http://localhost:7000/";        // on local
    const string USER_ROUTER_URL = BASE_URL + "user/";
    const string CHAT_ROUTER_URL = BASE_URL + "chat/";
    const string WARP_ROUTER_URL = BASE_URL + "warp/";
    
    public static string LOGIN_URL = USER_ROUTER_URL + "login/";
    public static string JOIN_URL = USER_ROUTER_URL + "join/";

    public static string CHAT_URL = CHAT_ROUTER_URL + "save/";

    public static string WARP_URL = WARP_ROUTER_URL + "move/";

    public static string LOGIN_FUNC = "Login_REST";
    public static string JOIN_FUNC = "Join_REST";
    public static string CHAT_FUNC = "SaveChat_REST";
    public static string WARP_FUNC = "WarpPlayer_REST";
}
