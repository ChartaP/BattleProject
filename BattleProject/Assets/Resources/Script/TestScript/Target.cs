using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Lib;

public class Target : MonoBehaviour
{
    [SerializeField]
    private eTargetType type;
    [SerializeField]
    private ObjectCtrl objectCtrl = null;
    [SerializeField]
    private Bar myBar = null;
    [SerializeField]
    private Transform hpTrans = null;



    // Start is called before the first frame update
    void Start()
    {
        myBar = Instantiate(Resources.Load("Prefab/Bar") as GameObject, GameMng.Instance.interfaceMng.CanvasTrans.Find("Bars")).GetComponent<Bar>();
        myBar.name = name + "Bar";

        Color32 color;
        if(Owner == GameMng.Instance.playerMng.CtrlPlayer)
        {
            color = new Color32(0, 255, 0, 255);
        }
        else
        {
            color = new Color32(255, 0, 0, 255);
        }
        
        myBar.SetHP(Health, color);
        if (Type == eTargetType.Unit)
        {
            myBar.SetHungry(Hungry);
        }
        myBar.SetName(objectCtrl.Name);
    }

    // Update is called once per frame
    void Update()
    {
        myBar.CurHP(Health,CurHealth, hpTrans.position);
        if(Type == eTargetType.Unit)
        {
            myBar.CurHungry(Hungry);
            
        }
    }

    public void OnTargetEnable()
    {
        if(myBar)
            myBar.gameObject.SetActive ( true);
    }

    public void OnTargetDisable()
    {
        if (myBar)
            myBar.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if(myBar != null)
        {
            Destroy(myBar.gameObject);
        }
    }
    public float X
    {
        get {
            return objectCtrl.X;
        }
    }

    public float Y
    {
        get
        {
            return objectCtrl.Y;
        }
    }

    public float Radius
    {
        get
        {
            return objectCtrl.Stat("Radius");
        }
    }

    public float Health
    {
        get {
            return objectCtrl.Stat("Health");
        }
    }

    public int Hungry
    {
        get
        {
            return (objectCtrl as UnitCtrl).Hungry;
        }
    }

    public float CurHealth
    {
        get
        {
            return objectCtrl.curHealth;
        }
    }

    public Vector2 Pos
    {
        get
        {
            return objectCtrl.Pos;
        }
    }

    public PlayerCtrl Owner
    {
        get
        {
            return objectCtrl.Owner;
        }
    }

    public eTargetType Type
    {
        get { return type; }
    }

    public void GetDamage(float damage)
    {
        objectCtrl.GetDamage(damage);
    }

    public ObjectCtrl TargetObject
    {
        get
        {
            return objectCtrl;
        }
    }
    
    public Bar MyBar
    {
        get
        {
            return myBar;
        }
    }
}
