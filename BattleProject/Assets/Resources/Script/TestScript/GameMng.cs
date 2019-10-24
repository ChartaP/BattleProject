using System.Collections;
using System.Collections.Generic;
using GameSys.Lib;
using UnityEngine;

public class GameMng : MonoBehaviour
{
    public InputMng inputMng;
    public UnitMng unitMng;
    public MapMng mapMng;
    public InterfaceMng interfaceMng;
    public PlayerMng playerMng;

    public bool bGameStart = false;

    //싱글톤 형식으로 제작
    private static GameMng instance = null;

    public static GameMng Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(GameMng)) as GameMng;
            }
            return instance;
        }
    }
    //싱글톤 만들기 끝

    private void Awake()
    {
        mapMng.CreateMap();
        GameInfo.nCtrlPlayerID = 0;
        GameInfo.playerList.Add(new PlayerInfo("Test",ePlayerType.Player,eDifficulty.Commonness));
        GameInfo.playerList.Add(new PlayerInfo("AI", ePlayerType.Computer, eDifficulty.Commonness));
        playerMng.GetPlayerInfo();
        playerMng.SpawnPlayer();
        bGameStart = true;
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
