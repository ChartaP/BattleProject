using System.Collections;
using System.Collections.Generic;
using GameSys.Lib;
using UnityEngine;

public class PlayerMng : MonoBehaviour
{
    public GameMng gameMng;
    public List<PlayerCtrl> PlayerList;
    //public PlayerCtrl AnimalCtrl;
    //public PlayerCtrl BarbarianVtrl;
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
            player.name = info.Name;
            player.playerMng = this;
            PlayerList.Add(player );
            if (GameInfo.nCtrlPlayerID == info.ID)
                CtrlPlayer = player;
        }
    }

    public void SpawnPlayer()
    {
        foreach(PlayerCtrl player in PlayerList)
        {

            //플레이어 스폰
            if (player.playerInfo.Type == ePlayerType.Player)
            {
                Vector3 spawnPos = new Vector3(Random.Range(2,GameInfo.nXSize-2),0,Random.Range(2,GameInfo.nYSize-2));
                UnitCtrl unitTemp = null;
                if (player == CtrlPlayer)
                {
                    gameMng.interfaceMng.MainCameraCarrier.localPosition = new Vector3(spawnPos.x, 16, spawnPos.z);
                }
                unitTemp = gameMng.unitMng.CreateUnit(spawnPos, player, eUnitType.People);
                gameMng.unitMng.ChangeJob(unitTemp, eUnitJob.Leader);

            }
            //컴퓨터 플레이어 스폰
            else if (player.playerInfo.Type == ePlayerType.Computer)
            {
                Vector3 spawnPos = new Vector3(Random.Range(2, GameInfo.nXSize - 2), 0, Random.Range(2, GameInfo.nYSize - 2));
                UnitCtrl unitTemp = null;
                unitTemp = gameMng.unitMng.CreateUnit(spawnPos, player, eUnitType.People);
                gameMng.unitMng.ChangeJob(unitTemp, eUnitJob.Leader);
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
