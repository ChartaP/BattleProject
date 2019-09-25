using System.Collections;
using System.Collections.Generic;
using GameSys.Lib;
using GameSys.Order;
using GameSys.Unit;
using UnityEngine;

public class UnitCtrl : MonoBehaviour
{
    private static int nUintCnt = 0;
    public UnitMng unitMng;
    private JobInfo myJobInfo = null;
    [SerializeField]
    private eUnitType unitType;//유닛 종류
    [SerializeField]
    private eUnitJob unitJob;//유닛의 직업
    [SerializeField]
    private eUnitState unitState = eUnitState.Standby;//유닛의 상태
    [SerializeField]
    private PlayerCtrl Owner;

    public MeshRenderer SelectMesh;
    public Animation ani;
    
    [SerializeField]
    private Transform tLHand;
    [SerializeField]
    private Transform tRHand;
    [SerializeField]
    private Transform tHead;
    [SerializeField]
    private Transform tChest;
    [SerializeField]
    private Transform tUnit;
    
    public Transform tViewRange;
    public Transform tAtkRange;

    public float curHealth=1;

    private Order curOrder = null;

    public List<UnitCtrl> viewList = new List<UnitCtrl>();

    public void SetUnit(UnitMng unitMng, eUnitType unitType,PlayerCtrl Owner,Vector3 unitPos)
    {
        this.unitMng = unitMng;
        this.unitType = unitType;
        this.Owner = Owner;
        SetJob(eUnitJob.Jobless);
        transform.localPosition = unitPos;
        transform.name = (nUintCnt++) +"-"+ Owner.name+ "-Unit";
        OnGround();
        Owner.RegisterPlayerUnit(this);
    }

    public void SetJob(eUnitJob job)
    {
        unitJob = job;
        myJobInfo = JobInfoMng.Instance.Job((int)job);
        CapsuleCollider collider;
        tUnit.localScale = new Vector3(myJobInfo.Size, myJobInfo.Size, myJobInfo.Size);
        collider = transform.GetComponent<CapsuleCollider>();
        collider.radius = myJobInfo.ColRadius;
        collider.height = myJobInfo.ColHeight;
        collider.center = new Vector3(0, myJobInfo.ColHeight / 2, 0);
        curHealth = Health;
    }
    
    void Start()
    {
        ani.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (myJobInfo == null)
            return;
        if(unitState == eUnitState.Standby)
        {
            View();
            if (viewList.Count > 0)
            {
                receiptOrder(new ATK(viewList[0].transform));
            }
            fulfilOrder();
            RangeUpdate();

        }
        else if(unitState == eUnitState.Move)
        {
            View();
            OnGround();
            fulfilOrder();
            RangeUpdate();
        }
        else if(unitState == eUnitState.Atk)
        {
            View();
            fulfilOrder();
            RangeUpdate();
        }
        else if(unitState == eUnitState.Dead)
        {

        }
    }

    public void RangeUpdate()
    {
        tViewRange.localScale = new Vector3(ViewRange*2.0f, 0.1f, ViewRange * 2.0f);
        tAtkRange.localScale = new Vector3(AtkRange * 2.0f, 0.1f, AtkRange * 2.0f);
    }

    public void receiptOrder(Order order)
    {
        curOrder = order;
        Debug.Log("Receipt");
    }

    private void fulfilOrder()
    {
        if (curOrder == null)
            return;
        curOrder.Works(this);
        if (curOrder.Achievement(this))
        {
            curOrder = null;
            StateChange(eUnitState.Standby);
        }
    }

    /// <summary>
    /// 목표 지점 이동 메서드
    /// </summary>
    /// <param name="target"></param>
    public void Move(Vector2 target)
    {
        StateChange(eUnitState.Move);
        Vector2 temp = Pos;
        temp = Vector2.MoveTowards(temp, target, MoveSpeed * Time.deltaTime);
        transform.localPosition = new Vector3(temp.x,Height,temp.y);
        Rotate(target);
    }

    /// <summary>
    /// 보이는적 공격 메서드
    /// </summary>
    public void AtkInView()
    {
        if (viewList.Count > 0)
        {
            float distance = Vector2.Distance(Pos, viewList[0].Pos);
            if(distance < AtkRange)
            {//공격범위 안
                AtkUnit(viewList[0]);
            }
            else
            {//공격범위 밖
                Move(viewList[0].Pos);
            }
        }
    }

    public void AtkUnit(UnitCtrl unit)
    {
        StateChange(eUnitState.Atk);
        Rotate(unit.Pos);
    }

    public void View()
    {
        foreach(UnitCtrl unit in unitMng.unitList)
        {
            if (unit.Onwer == Onwer)
            {
                continue;
            }
            float distance = Vector2.Distance(Pos, unit.Pos);
            if (distance < ViewRange)
            {
                if (!viewList.Contains(unit))
                {
                    viewList.Add(unit);
                }
            }
            else
            {
                if (viewList.Contains(unit))
                {
                    viewList.Remove(unit);
                }
            }
        }
    }

    public void Stop()
    {
        StateChange(eUnitState.Standby);
    }

    public void Rotate(Vector2 target)
    {
        if (target == Pos)
            return;
        Vector3 dir = new Vector3(target.x, 0, target.y) - new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        dir.Normalize();
        transform.rotation = Quaternion.LookRotation(dir);
    }

    /// <summary>
    /// 유닛이 땅위에 있게 해주기
    /// </summary>
    private void OnGround()
    {
        transform.localPosition = new Vector3(X, Height, Y);
    }
    
    public eUnitType Type
    {
        get { return unitType; }
    }

    public eUnitJob Job
    {
        get { return unitJob; }
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
        get { return unitMng.gameMng.mapMng.GetHeight((int)X, (int)Y); }
    }

    public Vector2 Pos
    {
        get { return new Vector2(X, Y); }
    }

    public PlayerCtrl Onwer
    {
        get { return Owner; }
    }

    public float MoveSpeed
    {
        get
        {
            return myJobInfo.MoveSpeed;
        }
    }
    public float RotateSpeed
    {
        get
        {
            return myJobInfo.RoateSpeed;
        }
    }
    public int Health
    {
        get
        {
            return myJobInfo.Health;
        }
    }
    public int AtkPower
    {
        get
        {
            return myJobInfo.AtkPower;
        }
    }
    public float AtkSpeed
    {
        get
        {
            return myJobInfo.AtkSpeed;
        }
    }
    public int DefValue
    {
        get
        {
            return myJobInfo.DefValue;
        }
    }
    public float ViewRange
    {
        get
        {
            return myJobInfo.ViewRange;
        }
    }
    public float AtkRange
    {
        get
        {
            return myJobInfo.AtkRange;
        }
    }

    public void StateChange(eUnitState state)
    {
        if(unitState != state)
        {
            unitState = state;
            switch (state)
            {
                case eUnitState.Standby:
                    ani.Play("StandAni");
                    break;
                case eUnitState.Move:
                    ani.Play("WalkAni");
                    break;
                case eUnitState.Atk:
                    ani.Play("AtkAni");
                    break;
                case eUnitState.Dead:
                    ani.Play("DeadAni");
                    break;
            }
        }
    }
    


}
