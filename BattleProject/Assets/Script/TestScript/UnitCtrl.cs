using System.Collections;
using System.Collections.Generic;
using GameSys.Lib;
using UnityEngine;

public class UnitCtrl : MonoBehaviour
{
    public UnitMng unitMng;
    public eUnits unitID;//유닛 종류
    public PlayerCtrl Owner;
    public MeshRenderer SelectMesh;
    // Start is called before the first frame update

    public void SetUnit(UnitMng unitMng, eUnits unitID,PlayerCtrl Owner)
    {
        this.unitMng = unitMng;
        this.unitID = unitID;
        this.Owner = Owner;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    

   
}
