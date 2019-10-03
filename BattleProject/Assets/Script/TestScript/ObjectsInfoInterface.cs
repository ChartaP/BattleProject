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

    public void GetUnitList(List<UnitCtrl> unitList)
    {
        string sName = "";
        string sLeft = "";
        string sRight = "";
        if (unitList.Count == 0)
        {
            GetInfo(sName, sLeft, sRight);
            foreach(UnitBtn btn in unitBtnList)
            {
                btn.myUnit = null;
            }
        }
        else if (unitList.Count == 1)
        {
            sName = unitList[0].Name;
            sLeft = "체력 : " + unitList[0].curHealth + "/" + unitList[0].Health + "\n";

            GetInfo(sName, sLeft, sRight);
            foreach (UnitBtn btn in unitBtnList)
            {
                btn.myUnit = null;
            }

        }
        else
        {
            GetInfo(sName, sLeft, sRight);
            int nRow=1;
            int nCol=2;
            for(nRow=1; unitList.Count/nRow < nRow*2;nRow++)
            {

            }
            nCol = nRow * 2;
            for(int i = 0; i < unitList.Count; i++)
            {
                if (i > 8)
                    break;
                unitBtnList[i].myUnit = unitList[i];
            }

        }
    }
}
