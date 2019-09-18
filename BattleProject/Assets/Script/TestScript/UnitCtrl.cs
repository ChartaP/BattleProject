using System.Collections;
using System.Collections.Generic;
using GameSys.Lib;
using UnityEngine;

public class UnitCtrl : MonoBehaviour
{
    public UnitMng unitMng;
    public eUnitType unitType;//유닛 종류
    public eUnitJob unitJob;//유닛의 직업
    public PlayerCtrl Owner;
    public MeshRenderer SelectMesh;
    public Animation a;
    // Start is called before the first frame update

    public void SetUnit(UnitMng unitMng, eUnitType unitType,PlayerCtrl Owner,Vector3 unitPos)
    {
        this.unitMng = unitMng;
        this.unitType = unitType;
        this.Owner = Owner;
        transform.localPosition = unitPos;
        Owner.RegisterPlayerUnit(this);
    }
    
    void Start()
    {
        a.Play();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x,
            unitMng.gameMng.mapMng.GetHeight((int)transform.localPosition.x, (int)transform.localPosition.z),
            transform.localPosition.z);


        
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    

   
}
