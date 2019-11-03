using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CreateInfoInterface : MonoBehaviour
{
    public Transform transBuildList;
    public Transform transUnitList;
    private ObjectCtrl beforeObject = null;
    public Image SelectImg = null;
    public Text TitleText = null;
    public List<Image> ScheduleImg = new List<Image>();
    public Image ProductBar = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SelectImg.enabled = false;
        }
    }
    public void UnSelectCreate()
    {
        SelectImg.enabled = false;
    }
    
    public void CreateButtonClick()
    {
        PlayerCtrl ctrl = GameMng.Instance.playerMng.CtrlPlayer;
        if (ctrl.selectableObject.Count == 0)
        {
            InactiveSet();
        }
        else if(ctrl.selectableObject[0] is UnitCtrl)
        {
            UnitCtrl unit = ctrl.selectableObject[0] as UnitCtrl;

            if (unit.Job == GameSys.Lib.eUnitJob.Worker)
            {
                BuilderSet();
                beforeObject = ctrl.selectableObject[0];
            }
            else
            {
                InactiveSet();
            }
        }
        else if(ctrl.selectableObject[0] is BuildingCtrl)
        {
            if (ctrl.selectableObject[0].Owner == ctrl)
            {
                if (ctrl.selectableObject[0].Name == "BootCamp")
                {
                    UnitCreateSet();
                    beforeObject = ctrl.selectableObject[0];
                }
                else
                {
                    BuilderSet();
                    beforeObject = ctrl.selectableObject[0];
                }
            }
        }
    }

    public void SelectBuilding(int num)
    {
        SelectImg.rectTransform.localPosition = new Vector3(56 + num * 96, -2, 0);
        SelectImg.enabled = true;
    }

    private void BuilderSet()
    {
        TitleText.text = "건물 목록";
        transBuildList.gameObject.SetActive(true);
        transUnitList.gameObject.SetActive(false);
    }

    private void UnitCreateSet()
    {
        TitleText.text = "유닛 목록";
        GameMng.Instance.inputMng.ChangeState(0);
        transUnitList.gameObject.SetActive(true);
        transBuildList.gameObject.SetActive(false);
    }

    public void UnitCreateClick(int job)
    {
        PlayerCtrl ctrlPlayer = GameMng.Instance.playerMng.CtrlPlayer;
        BootCampCtrl bootcamp = ctrlPlayer.selectableObject[0] as BootCampCtrl;
        bootcamp.AddProductUnit((GameSys.Lib.eUnitJob)job);
    }

    private void InactiveSet()
    {
        transUnitList.gameObject.SetActive(false);
        transBuildList.gameObject.SetActive(false);
    }
}
