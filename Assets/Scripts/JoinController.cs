using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class JoinController : MonoBehaviour
{
    public float rotateSpeed = 60f;

    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] List<GameObject> prefabs;

    const string LOBBY_SCENE = "Lobby";

    string currentPrefabName;
    GameObject currentPrefab;


    private void Awake()
    {
        initDropdown();
        initPrefabs();
    }

    void initDropdown()
    {
        currentPrefabName = dropdown.options[dropdown.value].text;
        dropdown.onValueChanged.AddListener(delegate
        {
            Function_Dropdown(dropdown);
        });
    }

    void initPrefabs()
    {
        currentPrefab = null;
        changePrefab(currentPrefabName);
    }

    void changePrefab(string text)
    {

        foreach (var prefab in prefabs)
        {
            if (prefab.name.Equals(text))
            {
                var prevPrefab = currentPrefab;
                currentPrefab = Instantiate(prefab);
                currentPrefabName = text;
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

    public void BackToLobby()
    {
        SceneManager.LoadScene(LOBBY_SCENE);
    }
}
