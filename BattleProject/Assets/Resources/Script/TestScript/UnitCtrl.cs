using System.Collections;
using System.Collections.Generic;
using GameSys.Lib;
using GameSys.Order;
using GameSys.Unit;
using UnityEngine;

public class UnitCtrl : ObjectCtrl
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
    
    public Transform tAtkRange;
    
    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private bool isCollision = false;
    
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
        Owner.dicResource["WorkPopulation"] += 1;
    }

    public void SetJob(eUnitJob job)
    {
        unitJob = job;
        myJobInfo = JobInfoMng.Instance.Job((byte)job);
        RegisterStats();
        CapsuleCollider collider;
        tUnit.localScale = new Vector3(myJobInfo.Size, myJobInfo.Size, myJobInfo.Size);
        collider = transform.GetComponent<CapsuleCollider>();
        collider.radius = myJobInfo.ColRadius;
        collider.height = myJobInfo.ColHeight;
        collider.center = new Vector3(0, myJobInfo.ColHeight / 2, 0);
        curHealth = Stat("Health");
        sName = myJobInfo.Name;
        sIcon = myJobInfo.Face;

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
            tHeadEquip = Instantiate(Resources.Load("Prefab/" + Head) as GameObject, tHead).transform;

        }
        if (RHand != "null")
        {
            tRHandEquip = Instantiate(Resources.Load("Prefab/" + RHand) as GameObject, tRHand).transform;
        }
        if(job != eUnitJob.Jobless)
        {
            Owner.dicResource["WorkPopulation"] -= 1;
        }
    }

    protected override void RegisterStats()
    {
        docStats = new Dictionary<string, float>();
        docStats.Add("MoveSpeed",myJobInfo.MoveSpeed);
        docStats.Add("RotateSpeed", myJobInfo.RoateSpeed);
        docStats.Add("Health", myJobInfo.Health);
        docStats.Add("AtkPower", myJobInfo.AtkPower);
        docStats.Add("AtkSpeed", myJobInfo.AtkSpeed);
        docStats.Add("DefValue", myJobInfo.DefValue);
        docStats.Add("ViewRange", myJobInfo.ViewRange);
        docStats.Add("AtkRange", myJobInfo.AtkRange);
        docStats.Add("Size", myJobInfo.Size);
        docStats.Add("Radius", myJobInfo.ColRadius);
    }

    void Start()
    {
        base.Start();
        ani.Play();
        StartCoroutine("Decide");
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
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
                Unlump();

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

    public void Unlump()
    {
        if (IsCollisionPos( Pos))
        {
            transform.Translate(Random.Range(-1f, 1.0f) * 0.001f, 0, Random.Range(-1f, 1.0f) * 0.001f);
        }

        if (!IsCollisionPos( curPos))
        {
            if (Vector2.Distance(Pos, curPos) > 0.1f)
            {
                MovePos(curPos);
            }
        }

    }

    public override void GetDamage(float damage)
    {
        if (damage > 0)
        {
            DamageParicle.Play();
        }
        curHealth = (curHealth - damage <= 0) ? 0 : (curHealth - damage);
        if (curHealth == 0)
        {
            StateChange(eUnitState.Dead);
        }
    }

    public bool IsCollisionPos( Vector2 pos)
    {
        foreach (Target unit in viewList)
        {
            if(unit == myTarget)
                continue;
            if (Vector2.Distance(unit.Pos, pos) < unit.Radius/2 + Stat("Radius")/2)
            {
                return true;
            }
        }
        return false;
    }

    public Target TargetCollisionPos(Vector2 pos)
    {
        foreach (Target unit in viewList)
        {
            if (Vector2.Distance(unit.Pos, pos) < unit.Radius/2 + Stat("Radius") / 2)
            {
                return unit;
            }
        }
        return null;
    }

    public override void RangeUpdate()
    {
        tViewRange.localScale = new Vector3(Stat("ViewRange")*2.0f, 0.1f, Stat("ViewRange") * 2.0f);
        tAtkRange.localScale = new Vector3(Stat("AtkRange") * 2.0f, 0.1f, Stat("AtkRange") * 2.0f);
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
        Target temp = null;
        if (viewList.Count > 0)
        {
            foreach (Target target in viewList)
            {
                if (target.Owner == Owner)
                    continue;
                if(temp == null)
                    temp = target;
                else
                {
                    if(Vector2.Distance(Pos, target.Pos) < Vector2.Distance(Pos, temp.Pos))
                    {
                        temp = target;
                    }
                }
            }
        }
        return temp;
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
        curPos = pos;
        StopCoroutine("Move");
        StartCoroutine("Move", pos);
    }

    public void MoveTarget(Target pos)
    {
        curPos = pos.Pos;
        StopCoroutine("Move");
        StartCoroutine("Move", pos);
    }
    IEnumerator Move(Vector2 target)
    {
        StateChange(eUnitState.Move);
        while (true)
        {
            Rotate(target);
            transform.Translate(0, 0 , Stat("MoveSpeed") * Time.deltaTime);
            Debug.DrawRay(transform.position, transform.forward * Vector2.Distance(Pos,curPos), Color.blue, Time.deltaTime);
            if (Vector2.Distance( Pos , target)<0.1f)
                break;
            yield return null;
        }
        Stop();
        yield break;
    }

    IEnumerator Move(Target target)
    {
        StateChange(eUnitState.Move);
        while (true)
        {
            Rotate(target.Pos);
            transform.Translate(0, 0, Stat("MoveSpeed") * Time.deltaTime);
            Debug.DrawRay(transform.position, transform.forward * Vector2.Distance(Pos, curPos), Color.blue, 0.3f);
            if (Vector2.Distance(Pos, target.Pos) < 0.1f)
                break;
            yield return null;
        }
        Stop();
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
    
    public void TargetDamage()
    {
        Debug.Log(name + "가"+curTarget.transform.name+"에게"+ Stat("AtkPower")+"데미지");
        curTarget.GetDamage(Stat("AtkPower"));
    }
    public void Stop()
    {
        StateChange(eUnitState.Standby);
    }

    
    public void Rotate(Vector2 target)
    {
        if (target == Pos)
            return;
        Vector3 dir = new Vector3(target.x, 0, target.y) - new Vector3(X, 0, Y);
        dir.Normalize();
        transform.rotation = Quaternion.LookRotation(dir);
    }
    
    public eUnitType Type
    {
        get { return unitType; }
    }

    public eUnitJob Job
    {
        get { return unitJob; }
    }
    
    public byte ID
    {
        get
        {
            return myJobInfo.ID;
        }
    }
    
    public override float Stat(string name)
    {
        if (docStats.ContainsKey(name))
            return docStats[name];
        else
            return 0.0f;
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
    
    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.tag == "Unit")
            isCollision = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Unit")
            isCollision = false;
    }
}
