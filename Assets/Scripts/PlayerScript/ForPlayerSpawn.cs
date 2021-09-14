using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Room에 접속할 때 접속하는 대상인 Player 정보를 저장하기 위함
public class ForPlayerSpawn : MonoBehaviour
{
    public JBUS_Player SpawnPlayer { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
