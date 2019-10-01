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
    private PlayerCtrl owner;
    [SerializeField]
    private Target myTarget;

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
    [SerializeField]
    private Transform tLHandEquip;
    [SerializeField]
    private Transform tRHandEquip;
    [SerializeField]
    private Transform tHeadEquip;
    [SerializeField]
    private Transform tChestEquip;
    [SerializeField]
    private Transform tUnitEquip;

    public Transform tViewRange;
    public Transform tAtkRange;

    public float curHealth=1;

    private Order curOrder = null;
    [SerializeField]
    private Target curTarget = null;

    public List<Target> viewList = new List<Target>();

    public void SetUnit(UnitMng unitMng, eUnitType unitType,PlayerCtrl owner,Vector3 unitPos)
    {
        this.unitMng = unitMng;
        this.unitType = unitType;
        this.owner = owner;
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

        string Head = JobEquipInfoMng.Instance.JobEquip(ID).Head;
        string RHand = JobEquipInfoMng.Instance.JobEquip(ID).RHand;
        if (tHeadEquip!= null)
        {
            Destroy( tHeadEquip.gameObject);
            tHeadEquip = null;
        }
        if (tRHandEquip != null)
        {
            Destroy(tRHandEquip.gameObject);
            tRHandEquip = null;
        }
        if ( Head != "null")
        {
            Debug.Log(Head);
            tHeadEquip = Instantiate(Resources.Load("Prefab/" + Head) as GameObject, tHead).transform;

        }
        if (RHand != "null")
        {
            Debug.Log(RHand);
            tRHandEquip = Instantiate(Resources.Load("Prefab/" + RHand) as GameObject, tRHand).transform;
        }
    }
    
    void Start()
    {
        ani.Play();
        StartCoroutine("Decide");
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Decide()
    {
        while (true)
        {
            if (myJobInfo == null)
                continue;
            if (unitState == eUnitState.Dead)
                yield break ;
            if (unitState == eUnitState.Standby)
            {
                View();
                AtkInView();
                fulfilOrder();
                RangeUpdate();

            }
            else if (unitState == eUnitState.Move)
            {
                View();
                OnGround();
                fulfilOrder();
                RangeUpdate();
            }
            else if (unitState == eUnitState.Atk)
            {
                View();
                fulfilOrder();
                RangeUpdate();
            }
            yield return new WaitForSecondsRealtime(0.4f);
        }
        yield break;
    }

    public void RangeUpdate()
    {
        tViewRange.localScale = new Vector3(ViewRange*2.0f, 0.1f, ViewRange * 2.0f);
        tAtkRange.localScale = new Vector3(AtkRange * 2.0f, 0.1f, AtkRange * 2.0f);
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
    private void fulfilOrder()
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
    /// 보이는적 공격 메서드
    /// 대기 상태일때만 쓰시오
    /// </summary>
    public void AtkInView()
    {
        Target temp = EnemyInView();
        if(temp != null)
        {
            receiptOrder(new AtkTarget(temp));
        }
    }

    public Target EnemyInView()
    {
        if (viewList.Count > 0)
        {
            foreach (Target target in viewList)
            {
                if (target.Owner == Owner)
                    continue;
                return target;
            }
        }
        return null;
    }

    /// <summary>
    /// 공격 메서드
    /// 공격중이면 유닛 회전
    /// </summary>
    /// <param name="unit"></param>
    public void AtkTarget(Target target)
    {
        curTarget = target;
        StateChange(eUnitState.Atk);
        Rotate(target.Pos);
    }

    public void MovePos(Vector2 pos)
    {
        StopCoroutine("Move");
        StartCoroutine("Move", pos);
    }

    public void MoveTarget(Target pos)
    {
        StopCoroutine("Move");
        StartCoroutine("Move", pos);
    }
    IEnumerator Move(Vector2 target)
    {
        StateChange(eUnitState.Move);
        while (true)
        {
            Vector2 temp = Pos;
            temp = Vector2.MoveTowards(temp, target, MoveSpeed * Time.deltaTime);
            transform.localPosition = new Vector3(temp.x, Height, temp.y);
            Rotate(target);
            if (Pos == target)
                break;
            yield return null;
        }
        
        yield break;
    }

    IEnumerator Move(Target target)
    {
        StateChange(eUnitState.Move);
        while (true)
        {
            Vector2 temp = Pos;
            temp = Vector2.MoveTowards(temp, target.Pos, MoveSpeed * Time.deltaTime);
            transform.localPosition = new Vector3(temp.x, Height, temp.y);
            Rotate(target.Pos);
            if (Pos == target.Pos)
                break;
            yield return null;
        }
        yield break;
    }

    IEnumerator Attack()
    {
        while (unitState == eUnitState.Atk)
        {
            ani.Stop();
            ani.Play(JobEquipInfoMng.Instance.JobEquip(myJobInfo.ID).AtkReadyAni);
            while (ani.isPlaying == true)
            {
                yield return null;
                if (curTarget == null)
                {
                    yield break;
                }
            }
            ani.Play(JobEquipInfoMng.Instance.JobEquip(myJobInfo.ID).AtkingAni);
            while (ani.isPlaying == true)
            {
                yield return null;
                if (curTarget == null)
                {
                    yield break;
                }
            }
            yield return null;
        }
        yield break;
    }

    IEnumerator Dead()
    {
        ani.Stop();
        ani.Play("DeadAni");
        while (ani.isPlaying == true)
        {
            yield return null;
        }
        Owner.UnregisterPlayerUnit(this);
        unitMng.RemoveUnit(this);
        yield break;
    }

    /// <summary>
    /// 가까운 타겟 포착 메서드
    /// </summary>
    public void View()
    {
        for(int i = 0; i < viewList.Count; i++)
        {
            if (viewList[i] == null)
            {
                viewList.Remove(viewList[i]);
                i = i - 1 < 0 ? i - 1 : 0;
            }
        }
        foreach(UnitCtrl unit in unitMng.unitList)
        {
            float distance = Vector2.Distance(Pos, unit.Pos);
            if (distance < ViewRange)
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
    }

    public void TargetDamage()
    {
        Debug.Log(name + "가"+curTarget.transform.name+"에게"+AtkPower+"데미지");
        curTarget.GetDamage(AtkPower);
    }

    public void GetDamage(float damage)
    {
        curHealth = (curHealth-damage<=0)? 0: (curHealth - damage);
        if (curHealth==0)
        {
            StateChange(eUnitState.Dead);
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

    public PlayerCtrl Owner
    {
        get { return owner; }
    }
    public Target Target
    {
        get { return myTarget; }
    }
    public byte ID
    {
        get
        {
            return myJobInfo.ID;
        }
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
    public float Size
    {
        get { return myJobInfo.Size; }
    }
    public eUnitState State
    {
        get { return unitState; }
    }

    public void StateChange(eUnitState state)
    {
        if(unitState != state)
        {
            switch (unitState)
            {
                case eUnitState.Standby:
                    break;
                case eUnitState.Move:
                    StopCoroutine("Move");
                    break;
                case eUnitState.Atk:
                    StopCoroutine("Attack");
                    break;
            }
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
                    Debug.Log("changeAtk");
                    StartCoroutine("Attack");
                    break;
                case eUnitState.Dead:
                    StopAllCoroutines();
                    StartCoroutine("Dead");
                    break;
            }
        }
    }
    


}
