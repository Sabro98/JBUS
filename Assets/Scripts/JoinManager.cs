using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class JoinManager : MonoBehaviour
{
    public float rotateSpeed = 60f;

    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] TMP_InputField IDInput, NickNameInput;
    [SerializeField] List<GameObject> prefabs;

    const string LOBBY_SCENE = "LOBBY";
    const string JOIN_URL = "https://jbus.herokuapp.com/user/join";

    public string PlayerID { get; set; }
    public string PlayerModel { get; set; }
    public string PlayerNickName { get; set; }


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
        PlayerModel = dropdown.options[dropdown.value].text;
        dropdown.onValueChanged.AddListener(delegate
        {
            Function_Dropdown(dropdown);
        });
    }

    void initPrefabs()
    {
        currentPrefab = null;
        changePrefab(PlayerModel);
    }

    void changePrefab(string text)
    {

        foreach (var prefab in prefabs)
        {
            if (prefab.name.Equals(text))
            {
                var prevPrefab = currentPrefab;
                currentPrefab = Instantiate(prefab);
                PlayerModel = text;
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

    public void joinPlayer()
    {
        PlayerID = IDInput.text;
        PlayerNickName = NickNameInput.text;
        if (string.IsNullOrEmpty(PlayerID)) return;
        if (string.IsNullOrEmpty(PlayerNickName)) return;

        //join player
        StartCoroutine(Join_REST());
    }

    public void BackToLobby()
    {
        SceneManager.LoadScene(LOBBY_SCENE);
    }

    IEnumerator Join_REST()
    {
        WWWForm form = new WWWForm();
        form.AddField("playerID", PlayerID);
        form.AddField("playerNickName", PlayerNickName);
        form.AddField("playerModel", PlayerModel);

        using (UnityWebRequest www = UnityWebRequest.Post(JOIN_URL, form))
        {
            yield return www.SendWebRequest();
            if(www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                //존재하는 ID 오류 체크 해야
                if (www.isDone)
                {
                    System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    SceneManager.LoadScene(LOBBY_SCENE);
                }
            }
        }
    }
}
