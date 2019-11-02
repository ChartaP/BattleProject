using System.Collections;
using System.Collections.Generic;
using GameSys.Lib;
using GameSys.Order;
using UnityEngine;

namespace GameSys
{
    namespace Lib
    {
        public class PlayerInfo
        {
            private static int PlayerCnt = 0;
            private int nID;
            private string sName;
            private ePlayerType eType;
            private eDifficulty eDifficulty;

            public PlayerInfo(string sName,ePlayerType eType,eDifficulty eDifficulty)
            {
                nID = PlayerCnt++;
                this.sName = sName;
                this.eType = eType;
                this.eDifficulty = eDifficulty;
            }

            public int ID
            {
                get { return nID; }
            }
            public string Name
            {
                get { return sName; }
            }
            public ePlayerType Type
            {
                get { return eType; }
            }
            public eDifficulty Difficulty
            {
                get { return eDifficulty; }
            }
        }
    }
}

public class PlayerCtrl : MonoBehaviour
{
    public int PlayerID;
    public PlayerMng playerMng;
    public PlayerInfo playerInfo;

    //플레이어의 유닛 목록
    public List<UnitCtrl> UnitList;
    //플레이어의 건물 목록
    public List<BuildingCtrl> BuildingList;
    //플레이어가 선택한 오브젝트 목록
    public List<ObjectCtrl> selectableObject = new List<ObjectCtrl>();
    public UnitCtrl LeaderUnit;
    public Material playerMater;

    public int[,] FoW; //0 안개, 1 보이는곳, 2 한번 봤던 곳

    public Dictionary<string, int> dicResource = new Dictionary<string, int>();

    public byte selectBuild = 0;

    private void Awake()
    {
        dicResource.Add("Food", 100);
        dicResource.Add("FoodStorage", 100);
        dicResource.Add("WorkPopulation", 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    
    
    public void UnselectObject()
    {
        foreach (ObjectCtrl obj in selectableObject)
        {
            obj.SelectMesh.enabled = false;
        }
        selectableObject.Clear();
    }

    /// <summary>
    /// 지정 범위의 유닛 다중 선택 메서드
    /// 자료출처 : https://youtu.be/ceMyupol6AQ
    /// </summary>
    /// <param name="mPosStart">드래그 시작점</param>
    /// <param name="mPosEnd">드래그 도착점</param>
    public void SelectObject(Vector3 mPosStart, Vector3 mPosEnd)
    {
        //찾은 유닛 리스트
        List<UnitCtrl> findUnits = new List<UnitCtrl>();
        //찾은 건물 리스트
        List<BuildingCtrl> findBuildings = new List<BuildingCtrl>();

        Rect selectRect = new Rect(mPosStart.x, mPosStart.y, mPosEnd.x - mPosStart.x, mPosEnd.y - mPosStart.y);
        foreach (UnitCtrl unit in UnitList)
        {
            if (selectRect.Contains(Camera.main.WorldToViewportPoint(unit.transform.position), true))
            {
                findUnits.Add(unit);
            }
        }
        if(findUnits.Count == 0 )//찾은 내 유닛 목록수0
        {
            //내 건물 목록 찾기
            foreach (BuildingCtrl building in BuildingList)
            {
                if (selectRect.Contains(Camera.main.WorldToViewportPoint(building.transform.position), true))
                {
                    findBuildings.Add(building);
                }
            }

            if (findBuildings.Count == 0)//찾은 내 건물 목록수 0
            {
                foreach(PlayerCtrl player in playerMng.PlayerList)
                {
                    if (player == this)//만약 검색한 플레이어가 나일 경우 무시
                        continue;
                    
                    foreach (UnitCtrl unit in player.UnitList)
                    {
                        if (selectRect.Contains(Camera.main.WorldToViewportPoint(unit.transform.position), true))
                        {
                            findUnits.Add(unit);
                            break;//현재 플레이어가 아닌 플레이어의 유닛은 여럿 찾을 필요 없으므로 반복문 탈출
                        }
                    }
                    if(findUnits.Count > 0)//이 플레이어의 유닛 존재
                    {
                        break;
                    }
                    else//유닛이 존재하지 않음
                    {
                        //내 건물 목록 찾기
                        foreach (BuildingCtrl building in player.BuildingList)
                        {
                            if (selectRect.Contains(Camera.main.WorldToViewportPoint(building.transform.position), true))
                            {
                                findBuildings.Add(building);
                            }
                        }
                        if (findBuildings.Count > 0)//이 플레이어의 유닛 존재
                        {
                            break;
                        }
                    }
                }
            }
        }
        if (findUnits.Count > 0)
        {
            UnselectObject();
            foreach (UnitCtrl unit in findUnits)
            {
                unit.SelectMesh.enabled = true;
                selectableObject.Add(unit);
            }
            GameMng.Instance.interfaceMng.CreateInfomationInterface();
        }
        else if(findBuildings.Count > 0)
        {
            UnselectObject();
            foreach (BuildingCtrl unit in findBuildings)
            {
                unit.SelectMesh.enabled = true;
                selectableObject.Add(unit);
            }
            GameMng.Instance.interfaceMng.CreateInfomationInterface();
        }
    }

    /// <summary>
    /// 넘겨받은 유닛리스트를 선택
    /// </summary>
    /// <param name="unitList">유닛리스트</param>
    public void SelectUnits(List<ObjectCtrl> objectList)
    {
        if (objectList.Count == 0)
        {
        }
        else
        {
            foreach (ObjectCtrl obj in objectList)
            {
                obj.SelectMesh.enabled = false;
                if (UnitList.Contains(obj as UnitCtrl))
                {
                    selectableObject.Add(obj);
                    obj.SelectMesh.enabled = true;
                }
            }
            GameMng.Instance.interfaceMng.CreateInfomationInterface();
        }
    }
    
    /// <summary>
    /// 유닛 선택 메서드
    /// </summary>
    /// <param name="unit"></param>
    public void SelectObject(ObjectCtrl obj)
    {
        if (obj != null)
        {
            UnselectObject();
            selectableObject.Add(obj);
            obj.SelectMesh.enabled = true;
            GameMng.Instance.interfaceMng.CreateInfomationInterface();
        }
    }

    /// <summary>
    /// 플레이어 유닛 등록 메서드
    /// </summary>
    /// <param name="unit"></param>
    public void RegisterPlayerUnit(UnitCtrl unit)
    {
        UnitList.Add(unit);
        dicResource["WorkPopulation"] += 1;
    }
    /// <summary>
    /// 플레이어 유닛 해지 메서드
    /// </summary>
    /// <param name="unit"></param>
    public void UnregisterPlayerUnit(UnitCtrl unit)
    {
        UnitList.Remove(unit);
        dicResource["WorkPopulation"] -= 1;
        selectableObject.Remove(unit);
        if (unit.Job == eUnitJob.Leader)
            playerMng.fallPlayer(this);

    }
    
    /// <summary>
    /// 선택된 유닛에 명령 메서드
    /// </summary>
    /// <param name="target"></param>
    public void OrderObjects(eOrder type, Transform target)
    {
        if (selectableObject.Count == 0)
            return;
        if (selectableObject[0].Owner != this)
            return;

        Vector2 center = SelectCenterPos();

        foreach (ObjectCtrl obj in selectableObject)
        {
            switch (type)
            {
                case eOrder.MoveTarget:
                    obj.receiptOrder(new MoveTarget(target.GetComponent<Target>()));
                    break;
                case eOrder.MovePos:
                    obj.receiptOrder(new MovePos( new Vector2(obj.X- SelectCenterPos().x + target.localPosition.x, obj.Y- SelectCenterPos().y+ target.localPosition.z )));
                    break;
                case eOrder.AtkTarget:
                    obj.receiptOrder(new AtkTarget(target.GetComponent<Target>()));
                    break;
                case eOrder.AtkPos:
                    obj.receiptOrder(new AtkPos(new Vector2(obj.X- SelectCenterPos().x+ target.localPosition.x, obj.Y- SelectCenterPos().y + target.localPosition.z)));
                    break;
            }
        }
    }

    public Vector2 SelectCenterPos()
    {
        float X=0;
        float Y=0;
        foreach(ObjectCtrl obj in selectableObject)
        {
            X += obj.X;
            Y += obj.Y;
        }
        X /= selectableObject.Count;
        Y /= selectableObject.Count;

        return new Vector2(X, Y);
    }

    public ePlayerType Type
    {
        get { return playerInfo.Type; }
    }

    /// <summary>
    /// 플레이어 유닛 등록 메서드
    /// </summary>
    /// <param name="unit"></param>
    public void RegisterPlayerBuilding(BuildingCtrl building)
    {
        BuildingList.Add(building);
    }
    /// <summary>
    /// 플레이어 유닛 해지 메서드
    /// </summary>
    /// <param name="unit"></param>
    public void UnregisterPlayerBuilding(BuildingCtrl building)
    {
        BuildingList.Remove(building);
        selectableObject.Remove(building);
    }
}
