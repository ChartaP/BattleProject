using System.Collections;
using System.Collections.Generic;
using GameSys.Lib;
using GameSys.Order;
using UnityEngine;

public class UnitCtrl : MonoBehaviour
{
    private static int nUintCnt = 0;
    public UnitMng unitMng;
    [SerializeField]
    private eUnitType unitType;//유닛 종류
    [SerializeField]
    private eUnitJob unitJob;//유닛의 직업
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

    private List<Order> OrderList = new List<Order>();

    public void SetUnit(UnitMng unitMng, eUnitType unitType,PlayerCtrl Owner,Vector3 unitPos)
    {
        this.unitMng = unitMng;
        this.unitType = unitType;
        this.Owner = Owner;
        unitJob = eUnitJob.Jobless;
        transform.localPosition = unitPos;
        transform.name = (nUintCnt++) +"-"+ Owner.name+ "-Unit";
        Owner.RegisterPlayerUnit(this);
    }

    public void SetJob(eUnitJob job)
    {
        unitJob = job;
    }
    
    void Start()
    {
        ani.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        OnGround();
        fulfilOrder();
    }

    public void receiptOrder(Order order)
    {
        OrderList.Clear();
        OrderList.Add(order);
        ani.CrossFade("WalkAni");
        Rotate(order.TargetPos);
        Debug.Log("Receipt");
    }

    private void fulfilOrder()
    {
        if (OrderList.Count > 0)
        {
            OrderList[0].Works(this);
            if (OrderList[0].Achievement(this))
            {
                OrderList.RemoveAt(0);
            }
        }
    }

    public void Move(Vector2 target)
    {
        Vector2 temp = new Vector2( transform.localPosition.x ,transform.localPosition.z);
        temp = Vector2.MoveTowards(temp, target, 5.0f * Time.deltaTime);
        transform.localPosition = new Vector3(temp.x,transform.localPosition.y,temp.y);
    }

    public void Stop()
    {
        ani.CrossFade("StandAni");

    }

    public void Rotate(Vector2 target)
    {
        Vector3 dir = new Vector3(target.x, 0, target.y) - new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        dir.Normalize();
        transform.rotation = Quaternion.LookRotation(dir);
    }

    private void OnGround()
    {
        transform.localPosition = new Vector3(transform.localPosition.x,
            unitMng.gameMng.mapMng.GetHeight((int)transform.localPosition.x, (int)transform.localPosition.z),
            transform.localPosition.z);
    }
    
    public eUnitType Type
    {
        get { return unitType; }
    }

    public eUnitJob Job
    {
        get { return unitJob; }
    }
    
}
