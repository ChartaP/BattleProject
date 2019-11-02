using System.Collections;
using System.Collections.Generic;
using GameSys.Lib;
using GameSys.Order;
using GameSys.Unit;
using UnityEngine;

public abstract class ObjectCtrl : MonoBehaviour
{
    [SerializeField]
    protected PlayerCtrl owner;
    [SerializeField]
    protected Target myTarget;
    [SerializeField]
    protected Vector2 curPos;
    [SerializeField]
    protected ParticleSystem DamageParicle = null;
    [SerializeField]
    protected Target curAtkTarget = null;

    protected Order curOrder = null;

    protected Dictionary<string, float> docStats;

    protected string sName = "Object";
    protected string sIcon = "ObjectIcon";

    public MeshRenderer SelectMesh;
    public float curHealth = 1;
    public List<Target> viewList = new List<Target>();
    
    // Start is called before the first frame update

    protected List<IEnumerator> enumerators = new List<IEnumerator>();

    protected void Start()
    {
        curPos = Pos;
    }

    // Update is called once per frame
    protected void Update()
    {

    }
    
    /// <summary>
    /// 명령 접수
    /// </summary>
    /// <param name="order"></param>
    public void receiptOrder(Order order)
    {
        
        curOrder = order;
        curOrder.Start(this);
        Debug.Log("Receipt");
    }

    /// <summary>
    /// 명령 수행
    /// </summary>
    protected void fulfilOrder()
    {
        if (curOrder == null)
            return;
        curOrder.Works(this);
        if (curOrder.Achievement(this))//명령 달성 여부 확인
        {
            curOrder = null;
        }
    }

    /// <summary>
    /// 가까운 타겟 포착 메서드
    /// </summary>
    public void View()
    {
        for (int i = 0; i < viewList.Count; i++)
        {
            if (viewList[i] == null)
            {
                viewList.Remove(viewList[i]);
                i = i - 1 < 0 ? i - 1 : 0;
            }
        }
        foreach (UnitCtrl unit in GameMng.Instance.unitMng.unitList)
        {
            float distance = Vector2.Distance(Pos, unit.Pos);
            if (distance < docStats["ViewRange"])
            {
                if (!viewList.Contains(unit.myTarget))
                {
                    viewList.Add(unit.myTarget);
                }
            }
            else
            {
                if (viewList.Contains(unit.myTarget))
                {
                    viewList.Remove(unit.myTarget);
                }
            }
        }
        foreach (BuildingCtrl building in GameMng.Instance.buildingMng.buildingList)
        {
            float distance = Vector2.Distance(Pos, building.Pos);
            if (distance < docStats["ViewRange"])
            {
                if (!viewList.Contains(building.myTarget))
                {
                    viewList.Add(building.myTarget);
                }
            }
            else
            {
                if (viewList.Contains(building.myTarget))
                {
                    viewList.Remove(building.myTarget);
                }
            }
        }
    }

    /// <summary>
    /// 유닛이 땅위에 있게 해주기
    /// </summary>
    protected void OnGround()
    {
        transform.localPosition = new Vector3(X, Height, Y);
    }

    public virtual void GetDamage(float damage)
    {
        if (damage > 0)
        {
            DamageParicle.Play();
        }
        curHealth = (curHealth - damage <= 0) ? 0 : (curHealth - damage);
    }

    public Target Target
    {
        get { return myTarget; }
    }
    /// <summary>
    /// 유닛의 X좌표 반환
    /// </summary>
    public float X
    {
        get { return transform.localPosition.x; }
    }

    /// <summary>
    /// 유닛의 Y좌표(localPos의 z) 반환
    /// </summary>
    public float Y
    {
        get { return transform.localPosition.z; }
    }

    /// <summary>
    /// 유닛의 현재 높이
    /// </summary>
    public int Height
    {
        get { return GameMng.Instance.mapMng.GetHeight((int)X, (int)Y); }
    }

    public Vector2 Pos
    {
        get { return new Vector2(X, Y); }
    }

    public string Name
    {
        get { return sName; }
    }
    public string Icon
    {
        get { return sIcon; }
    }

    public PlayerCtrl Owner
    {
        get { return owner; }
    }
    
    protected abstract void RegisterStats();

    public abstract float Stat(string name);
}
