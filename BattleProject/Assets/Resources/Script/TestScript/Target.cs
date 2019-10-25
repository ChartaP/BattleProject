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
        myBar.Set(Health, new Color32(255, 0, 0,255));
    }

    // Update is called once per frame
    void Update()
    {
        myBar.Cur(Health,CurHealth, hpTrans.position);
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
            switch (Type)
            {
                case eTargetType.Unit:
                    return objectCtrl.X;
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
                    return objectCtrl.Y;
            }
            return 0.0f;
        }
    }

    public float Radius
    {
        get
        {
            switch (Type)
            {
                case eTargetType.Unit:
                    return objectCtrl.Stat("Radius");
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
                    return objectCtrl.Stat("Health");
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
                    return objectCtrl.curHealth;
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
                    return objectCtrl.Pos;
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
                    return objectCtrl.Owner;
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
                objectCtrl.GetDamage(damage);
                return;
        }
    }
    
}
