using System.Collections;
using System.Collections.Generic;
using GameSys.Lib;
using UnityEngine;

/// <summary>
/// 게임 로딩부터 종료시까지 살아있는 정적클래스.
/// 게임의 정보를 저장하고 있음
/// </summary>
public static class GameInfo
{
    public static List<PlayerInfo> playerList= new List<PlayerInfo>();
    public static int nCtrlPlayerID;
    public static int nXSize=128;
    public static int nYSize=128;
}
