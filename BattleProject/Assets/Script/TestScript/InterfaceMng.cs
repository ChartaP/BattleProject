using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum numFace{
    NullUnit=-1,
    TestUnit=0
}
public class InterfaceMng : MonoBehaviour
{
    public Camera MainCamera;
    public Camera UICamera;
    public Transform MainCameraCarrier;
    public  Image FaceSprite;
    public RectTransform SelecteBox;
    public Sprite[] UnitFace;
    private Sprite DefFace;
    // Start is called before the first frame update
    void Start()
    {
        DefFace = FaceSprite.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeFaceInterface(numFace faceNum)
    {
        if((int)faceNum <= -1)
        {
            FaceSprite.sprite = DefFace;
        }
        else
        {
            FaceSprite.sprite = UnitFace[(int)faceNum];
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
        Vector3 squareStart = Camera.main.WorldToScreenPoint(startPoint);
        squareStart.z = 0;
        Vector3 center = (squareStart + endPoint) / 2f;
        SelecteBox.position = center;
        float sizeX = Mathf.Abs(squareStart.x - endPoint.x);
        float sizeY = Mathf.Abs(squareStart.y - endPoint.y);
        SelecteBox.sizeDelta = new Vector2(sizeX, sizeY);
    }
}
