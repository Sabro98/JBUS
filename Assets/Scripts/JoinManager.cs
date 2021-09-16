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
    [SerializeField] GameObject ErrorText, NormalComponent, LoadingComponent;

    const string LOBBY_SCENE = "LOBBY";
    const string JOIN_URL = "https://jbus.herokuapp.com/user/join";

    public string PlayerID { get; set; }
    public string PlayerModel { get; set; }
    public string PlayerNickName { get; set; }

    const string FAIL_TO_JOIN_ERROR_MSG = "중복된 아이디가 존재합니다!";

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
        var prevPrefab = currentPrefab;
        PlayerModel = text;
        currentPrefab = Instantiate(Resources.Load(PlayerModel)) as GameObject;
        Destroy(prevPrefab);
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

    //로딩화면 출력
    void DisplayLoading()
    {
        if(currentPrefab != null) currentPrefab.SetActive(false);
        NormalComponent.SetActive(false);
        LoadingComponent.SetActive(true);
    }

    //일반 화면을 출력
    void DisplayNormal()
    {
        if (currentPrefab != null) currentPrefab.SetActive(true);
        NormalComponent.SetActive(true);
        LoadingComponent.SetActive(false);
    }

    //에러 메세지를 보여줌
    void DisplayErrorMsg(string msg)
    {
        ErrorText.SetActive(true);
        ErrorText.GetComponent<TMP_Text>().text = msg;
    }

    void Function_Dropdown(TMP_Dropdown select)
    {
        string selected = select.options[select.value].text;
        changePrefab(selected);
    }

    public void OnJoinBtn()
    {
        PlayerID = IDInput.text;
        PlayerNickName = NickNameInput.text;
        if (string.IsNullOrEmpty(PlayerID)) return;
        if (string.IsNullOrEmpty(PlayerNickName)) return;

        //join player
        DisplayLoading();
        StartCoroutine(Join_REST());
    }

    public void OnBackBtn()
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
                DisplayNormal();
                DisplayErrorMsg(FAIL_TO_JOIN_ERROR_MSG);
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
