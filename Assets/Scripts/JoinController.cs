using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class JoinController : MonoBehaviourPunCallbacks
{
    public float rotateSpeed = 60f;

    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] TMP_InputField IDInput;
    [SerializeField] List<GameObject> prefabs;

    const string GAME_SCENE = "Game";
    const string ROOM_NAME = "JBNU";

    public string PlayerID { get; set; }
    public string CurrentPrefabName { get; set; }
    GameObject currentPrefab;


    private void Awake()
    {
        //플레이 캐릭터 초기화
        initDropdown();
        initPrefabs();

        //playerID 초기화
        PlayerID = "";
    }

    void initDropdown()
    {
        CurrentPrefabName = dropdown.options[dropdown.value].text;
        dropdown.onValueChanged.AddListener(delegate
        {
            Function_Dropdown(dropdown);
        });
    }

    void initPrefabs()
    {
        currentPrefab = null;
        changePrefab(CurrentPrefabName);
    }

    void changePrefab(string text)
    {

        foreach (var prefab in prefabs)
        {
            if (prefab.name.Equals(text))
            {
                var prevPrefab = currentPrefab;
                currentPrefab = Instantiate(prefab);
                CurrentPrefabName = text;
                Destroy(prevPrefab);
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        if(currentPrefab != null)
        {
            var rotation = currentPrefab.transform.rotation.eulerAngles;
            rotation.y -= rotateSpeed * Time.fixedDeltaTime;
            rotation.y %= 360;
            currentPrefab.transform.rotation = Quaternion.Euler(rotation);
        }
    }
    

    void Function_Dropdown(TMP_Dropdown select)
    {
        string selected = select.options[select.value].text;
        changePrefab(selected);
    }

    public void playGame()
    {
        PlayerID = IDInput.text;
        if (string.IsNullOrEmpty(PlayerID)) return;

        DontDestroyOnLoad(this.gameObject);
        PhotonNetwork.JoinRoom(ROOM_NAME);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(ROOM_NAME);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(GAME_SCENE);
    }
}
