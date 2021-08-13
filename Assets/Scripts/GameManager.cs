using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    const string LOGIN_SCENE = "EnterToGame";

    [SerializeField] GameObject playerPrefab;


    LoginManager loginScript;

    public float spawnX;
    public float spawnZ;

    private void Awake()
    {
        var loginScene = GameObject.Find(LOGIN_SCENE);
        loginScript = loginScene.GetComponent<LoginManager>();
        SpawnPlayer();
    }


    void SpawnPlayer()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-spawnX, spawnX), 5, Random.Range(-spawnZ, spawnZ));
        var player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        print(player);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
