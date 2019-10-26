using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CreateInfoInterface : MonoBehaviour
{
    public Transform transBuildList;
    public Transform transUnitList;
    private ObjectCtrl beforeObject = null;

    public List<Image> sels = new List<Image>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach(Image img in sels)
            {
                img.enabled = false;
            }
        }
    }
    public void UnSelectCreate()
    {
    }
    
    public void CreateButtonClick()
    {
        PlayerCtrl ctrl = GameMng.Instance.playerMng.CtrlPlayer;
        if (ctrl.selectableObject.Count == 0)
        {
            BuilderSet();
        }
        else if(ctrl.selectableObject[0] is UnitCtrl)
        {
            BuilderSet();
            beforeObject = ctrl.selectableObject[0];
        }
        else if(ctrl.selectableObject[0] is BuildingCtrl)
        {
            if (ctrl.selectableObject[0].Owner == ctrl)
            {
                UnitCreateSet();
                beforeObject = ctrl.selectableObject[0];
            }
        }
    }

    private void BuilderSet()
    {
        transBuildList.gameObject.SetActive(true);
        transUnitList.gameObject.SetActive(false);
    }

    private void UnitCreateSet()
    {
        GameMng.Instance.inputMng.ChangeState(0);
        transUnitList.gameObject.SetActive(true);
        transBuildList.gameObject.SetActive(false);
    }
}
