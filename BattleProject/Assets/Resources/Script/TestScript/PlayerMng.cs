using System.Collections;
using System.Collections.Generic;
using GameSys.Lib;
using UnityEngine;

public class PlayerMng : MonoBehaviour
{
    public GameMng gameMng;
    public List<PlayerCtrl> PlayerList;
    public List<PlayerCtrl> ObserberList;
    public List<PlayerCtrl> NatureList;
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
        gameMng.interfaceMng.ObjectsInformationInterface(CtrlPlayer.selectableObject);
    }

    /// <summary>
    ///GameInfo의 플레이어 정보를 받아오는 메서드
    /// </summary>
    public void GetPlayerInfo()
    {
        int cnt = 0;
        foreach(PlayerInfo info in GameInfo.playerList)
        {
            PlayerCtrl player = Instantiate(gPlayerCtrl, transform).GetComponent<PlayerCtrl>();
            player.playerInfo = info;
            player.name = info.Name;
            player.PlayerID = cnt;
            switch (cnt)
            {
                case 0:
                    player.playerMater = Resources.Load("Texture/Materials/Blue") as Material;
                    break;
                case 1:
                    player.playerMater = Resources.Load("Texture/Materials/Red") as Material;
                    break;
            }
            player.playerMng = this;
            PlayerList.Add(player );
            if (GameInfo.nCtrlPlayerID == info.ID)
                CtrlPlayer = player;
            cnt++;
        }
    }

    /// <summary>
    /// 플레이어 탈락 메서드
    /// </summary>
    /// <param name="player"></param>
    public void fallPlayer(PlayerCtrl player)
    {
        PlayerList.Remove(player);
        if (player == CtrlPlayer)//현재 플레이어 탈락
        {
            Time.timeScale = 0;
            gameMng.interfaceMng.DisplayText("패배");
        }
        else if(PlayerList.Count == 1)//현재 플레이어 승리
        {
            Time.timeScale = 0;
            gameMng.interfaceMng.DisplayText("승리");
        }
    }

    /// <summary>
    /// 플레이어 스폰 메서드
    /// </summary>
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

                unitTemp = gameMng.unitMng.CreateUnit(spawnPos, player, eUnitType.People);
                gameMng.unitMng.ChangeJob(unitTemp, eUnitJob.Jobless);
                unitTemp = gameMng.unitMng.CreateUnit(spawnPos, player, eUnitType.People);
                gameMng.unitMng.ChangeJob(unitTemp, eUnitJob.Jobless);
                unitTemp = gameMng.unitMng.CreateUnit(spawnPos, player, eUnitType.People);
                gameMng.unitMng.ChangeJob(unitTemp, eUnitJob.Jobless);

            }
            //컴퓨터 플레이어 스폰
            else if (player.playerInfo.Type == ePlayerType.Computer)
            {
                Vector3 spawnPos = new Vector3(Random.Range(2, GameInfo.nXSize - 2), 0, Random.Range(2, GameInfo.nYSize - 2));
                UnitCtrl unitTemp = null;
                unitTemp = gameMng.unitMng.CreateUnit(spawnPos, player, eUnitType.People);
                gameMng.unitMng.ChangeJob(unitTemp, eUnitJob.Leader);

                unitTemp = gameMng.unitMng.CreateUnit(spawnPos, player, eUnitType.People);
                gameMng.unitMng.ChangeJob(unitTemp, eUnitJob.Jobless);
                unitTemp = gameMng.unitMng.CreateUnit(spawnPos, player, eUnitType.People);
                gameMng.unitMng.ChangeJob(unitTemp, eUnitJob.Jobless);
                unitTemp = gameMng.unitMng.CreateUnit(spawnPos, player, eUnitType.People);
                gameMng.unitMng.ChangeJob(unitTemp, eUnitJob.Jobless);
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

    public void CtrlPlayerSelectBuild(int id)
    {
        CtrlPlayer.selectBuild = (byte)id;
    }
}
