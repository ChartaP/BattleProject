using System.Collections;
using System.Collections.Generic;
using GameSys.Lib;
using UnityEngine;

public class PlayerMng : MonoBehaviour
{
    public GameMng gameMng;
    public List<PlayerCtrl> PlayerList;
    public PlayerCtrl CtrlPlayer;
    public GameObject gPlayerCtrl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetPlayerInfo()
    {
        foreach(PlayerInfo info in GameInfo.playerList)
        {
            PlayerCtrl player = Instantiate(gPlayerCtrl, transform).GetComponent<PlayerCtrl>();
            player.playerInfo = info;
            player.playerMng = this;
            PlayerList.Add(player );
            if (GameInfo.nCtrlPlayerID == info.nID)
                CtrlPlayer = player;
        }
    }

    public void SpawnPlayer()
    {
        foreach(PlayerCtrl player in PlayerList)
        {
            //플레이어 스폰
            if (player.playerInfo.eType == ePlayerType.Player)
            {

            }
            //컴퓨터 스폰
            else if (player.playerInfo.eType == ePlayerType.Computer)
            {

            }
            //관전자 스폰
            else
            {

            }
        }
    }

    public PlayerCtrl GetControlPlayer()
    {
        return CtrlPlayer;
    }
}
