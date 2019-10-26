using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsInfoInterface : MonoBehaviour
{
    [SerializeField]
    private Text tName=null;
    [SerializeField]
    private Text tLeft = null;
    [SerializeField]
    private Text tRight = null;
    [SerializeField]
    private GameObject gUnitBtn = null;
    [SerializeField]
    private List<UnitBtn> unitBtnList = new List<UnitBtn>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetInfo(string sName,string sLeft,string sRight)
    {
        tName.text = sName;
        tLeft.text = sLeft;
        tRight.text = sRight;
    }

    public void GetObjectList(List<ObjectCtrl> objList)
    {
        string sName = "";
        string sLeft = "";
        string sRight = "";
        foreach (UnitBtn btn in unitBtnList)
        {
            btn.myUnit = null;
        }
        if (objList.Count == 0)
        {
            GetInfo(sName, sLeft, sRight);
        }
        else if (objList.Count == 1)
        {
            sName = objList[0].Name;
            sLeft = "체력 : " + objList[0].curHealth + "/" + objList[0].Stat("Health") + "\n";

            GetInfo(sName, sLeft, sRight);
            

        }
        else
        {
            GetInfo(sName, sLeft, sRight);
            int nRow=1;
            int nCol=2;
            for(nRow=1; objList.Count/nRow < nRow*2;nRow++)
            {

            }
            nCol = nRow * 2;

            for(int i = 0; i < objList.Count; i++)
            {
                if (i > 8)
                    break;
                unitBtnList[i].myUnit = objList[i] as UnitCtrl;
            }

        }
    }
}
