using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSys.Lib;

public class InterfaceMng : MonoBehaviour
{
    public GameMng gameMng;
    public PlayerCtrl CtrlPlayer;
    public Camera MainCamera;
    public Camera UICamera;
    public Transform MainCameraCarrier;
    public  Image FaceSprite;
    public RectTransform SelecteBox;
    public Sprite[] UnitFace;
    public Transform CanvasTrans;
    public Text WideText;
    public ObjectsInfoInterface ObInfoInterface;
    public CreateInfoInterface CreateInfoInterface;
    public List<Text> ResourceTestList = new List<Text>();
    private Sprite DefFace;
    public Text alert = null;
    private Color32 invisible = new Color32(255, 255, 255, 0);


    // Start is called before the first frame update
    void Start()
    {
        DefFace = FaceSprite.sprite;
        CtrlPlayer = gameMng.playerMng.GetControlPlayer();
        StartCoroutine("Interface");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Interface()
    {
        while (true)
        {
            if (gameMng.bGameStart)
            {
                ResourceInterfase();
                ObjectsInformationInterface(gameMng.playerMng.CtrlPlayer.selectableObject);
                if (CtrlPlayer.selectableObject.Count > 0)
                {
                    ChangeFaceInterface(CtrlPlayer.selectableObject[0].Icon);
                }
                else
                {
                    ChangeFaceInterface("");
                }
            }
            yield return new WaitForSecondsRealtime(0.4f);
        }
        yield break;
    }

    public void ResourceInterfase()
    {
        foreach(Text text in ResourceTestList)
        {
            switch (text.transform.name)
            {
                case "Population":
                    text.text = "인구 : "+ CtrlPlayer.CurResource("WorkPopulation")+ "/" + CtrlPlayer.UnitList.Count + "()";
                    break;
                case "Food":
                    text.text = "식량 : "+CtrlPlayer.CurResource("Food")+ "/" + CtrlPlayer.CurResource("FoodStorage") + "()";
                    break;
            }
        }
    }

    public void ChangeFaceInterface(string Face)
    {
        if(Face == "")
        {
            FaceSprite.sprite = DefFace;
        }
        else
        {
            FaceSprite.sprite = Resources.Load<Sprite>("Texture/"+Face);
        }
    }

    /// <summary>
    /// 드래그 UI 출력 메서드, 
    /// 자료출처 : https://youtu.be/ceMyupol6AQ
    /// </summary>
    /// <param name="startPoint">드래그 시작점</param>
    /// <param name="endPoint">드래그 도착점</param>
    public void DragRectInterface(Vector3 startPoint,Vector3 endPoint)
    {
        if (!SelecteBox.gameObject.activeInHierarchy)
        {
            SelecteBox.gameObject.SetActive(true);
        }
        Vector3 squareStart = GameMng.Instance.interfaceMng.MainCamera.WorldToScreenPoint(startPoint);
        squareStart.z = 0;
        Vector3 center = (squareStart + endPoint) / 2f ;
        SelecteBox.position = center;
        float sizeX = Mathf.Abs(squareStart.x - endPoint.x) ;
        float sizeY = Mathf.Abs(squareStart.y - endPoint.y) ;
        SelecteBox.sizeDelta = new Vector2(sizeX   , sizeY );
    }

    public void ObjectsInformationInterface(List<ObjectCtrl> objList)
    {
        ObInfoInterface.GetObjectList(objList);
    }

    public void CreateInfomationInterface()
    {
        CreateInfoInterface.CreateButtonClick();
    }

    public void DisplayText(string text)
    {
        WideText.text = text;
    }

    public void AlertText(string text)
    {
        StopAllCoroutines();
        StartCoroutine("Alert", text);
    }
    IEnumerator Alert(string text)
    {
        alert.color = Color.black;
        alert.text = text;
        yield return new WaitForSecondsRealtime(1.0f);
        while (alert.color.a != 0)
        {
            alert.color = Color32.LerpUnclamped(alert.color,invisible,2.0f*Time.deltaTime);
            yield return null;
        }
        yield break;
    }
}
