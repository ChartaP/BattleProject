using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Lib;

public class Target : MonoBehaviour
{
    [SerializeField]
    private eTargetType type;
    [SerializeField]
    private UnitMng unitMng = null;
    [SerializeField]
    private UnitCtrl unitCtrl = null;
    [SerializeField]
    private Bar myBar = null;
    [SerializeField]
    private Transform hpTrans = null;



    // Start is called before the first frame update
    void Start()
    {
        switch (Type)
        {
            case eTargetType.Unit:
                unitMng = unitCtrl.unitMng;
                break;
        }
        myBar = Instantiate(Resources.Load("Prefab/Bar") as GameObject, unitMng.gameMng.interfaceMng.CanvasTrans).GetComponent<Bar>();
        myBar.name = name + "Bar";
        myBar.Set(Health, new Color32(255, 0, 0,255));
    }

    // Update is called once per frame
    void Update()
    {
        myBar.Cur(Health,CurHealth, hpTrans.position);
    }

    public void OnDestroy()
    {
        Destroy( myBar.gameObject);
    }

    public float X
    {
        get {
            switch (Type)
            {
                case eTargetType.Unit:
                    return unitCtrl.X;
            }
            return 0.0f;
        }
    }

    public float Y
    {
        get
        {
            switch (Type)
            {
                case eTargetType.Unit:
                    return unitCtrl.Y;
            }
            return 0.0f;
        }
    }

    public float Health
    {
        get {
            switch (Type)
            {
                case eTargetType.Unit:
                    return unitCtrl.Health;
            }
            return 1.0f;
        }
    }

    public float CurHealth
    {
        get
        {
            switch (Type)
            {
                case eTargetType.Unit:
                    return unitCtrl.curHealth;
            }
            return 1.0f;
        }
    }

    public Vector2 Pos
    {
        get
        {
            switch (Type)
            {
                case eTargetType.Unit:
                    return unitCtrl.Pos;
            }
            return Vector2.zero;
        }
    }

    public PlayerCtrl Owner
    {
        get
        {
            switch (Type)
            {
                case eTargetType.Unit:
                    return unitCtrl.Owner;
            }
            return null;
        }
    }

    public eTargetType Type
    {
        get { return type; }
    }

    public void GetDamage(float damage)
    {
        switch (Type)
        {
            case eTargetType.Unit:
                unitCtrl.GetDamage(damage);
                return;
        }
    }

    public eUnitState State
    {
        get { return unitCtrl.State; }
    }
}
