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

    private void Awake()
    {
        mapMng.CreateMap();
        GameInfo.nCtrlPlayerID = 0;
        GameInfo.playerList.Add(new PlayerInfo(0,"Test",ePlayerType.Player,eDifficulty.Commonness));
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
