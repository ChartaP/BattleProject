using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMng : MonoBehaviour
{
    public GameMng gameMng;
    public Ray ray;
    public RaycastHit hit;
    public LayerMask UnitLayer;
    public LayerMask TileLayer;
    public Vector3 StartPoint;
    public Vector3 EndPoint;
    public Vector3 mPosStart;
    public Vector3 mPosEnd;
    public float scrSpeed = 10.0f;
    private Vector3 mPos;
    private float scroll;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseInput();
        KeyboardInput();
    }

    /// <summary>
    /// 마우스 입력 처리 메서드,
    /// 자료출처 : https://youtu.be/ceMyupol6AQ
    /// </summary>
    private void MouseInput()
    {
        mPos = Vector3.zero ;
        if (Input.GetMouseButtonDown(0)) {//왼쪽 마우스 누르기
            gameMng.unitMng.ClearSelectableUnit();
            mPos = Input.mousePosition;
            mPosStart = Camera.main.ScreenToViewportPoint(mPos);
            Debug.Log("(" + mPos.x + "," + mPos.y + ") The Left Mouse");
            ray = Camera.main.ScreenPointToRay(mPos);
            if (Physics.Raycast(ray, out hit,Mathf.Infinity))
            {
                Debug.Log("HitObject : " + hit.transform.name);
                StartPoint = hit.point;
                switch (hit.transform.tag)
                {
                    case "Tile":
                        gameMng.unitMng.FocusCurUnit(null);
                        break;
                    case "Unit":
                        gameMng.unitMng.FocusCurUnit(hit.transform.GetComponent<UnitCtrl>());
                        break;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))//왼쪽 마우스 떼기
        {
            gameMng.interfaceMng.SelecteBox.gameObject.SetActive(false);
            mPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))//왼쪽 마우스 드래그
        {
            mPos = Input.mousePosition;
            EndPoint = mPos;
            gameMng.interfaceMng.DragRectInterface(StartPoint, EndPoint);
            mPosEnd = Camera.main.ScreenToViewportPoint(mPos);
            if (mPosStart != mPosEnd)
            {
                gameMng.unitMng.SelectUnits(mPosStart, mPosEnd);
            }
        }

        if (Input.GetMouseButtonDown(1))//오른쪽 마우스 누르기
        {
            
        }
        else if (Input.GetMouseButtonUp(1))//오른쪽 마우스 떼기
        {
            mPos = Input.mousePosition;
            Debug.Log("(" + mPos.x + "," + mPos.y + ") The Right Mouse");
            ray = Camera.main.ScreenPointToRay(mPos);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("HitObject : " + hit.transform.name);
                switch (hit.transform.tag)
                {
                    case "Tile":
                        gameMng.unitMng.IssueMoveOrder(hit.transform.localPosition);
                        break;
                    case "Unit":
                        break;
                }
            }
        }
        else if (Input.GetMouseButton(1))//오른쪽 마우스 드래그
        {

        }

        scroll = Input.GetAxis("Mouse ScrollWheel") * scrSpeed;

        mPos = gameMng.interfaceMng.MainCamera.transform.localPosition;
        if(mPos.z - scroll <= -1 && mPos.z - scroll >= -50)
        {
            mPos.z -= scroll;
            switch (mPos.z)
            {
                case -1:
                    gameMng.interfaceMng.MainCameraCarrier.localRotation = Quaternion.AngleAxis(55, Vector3.right);
                    break;
                case -2:
                    gameMng.interfaceMng.MainCameraCarrier.localRotation = Quaternion.AngleAxis(60, Vector3.right);
                    break;
                case -3:
                    gameMng.interfaceMng.MainCameraCarrier.localRotation = Quaternion.AngleAxis(65, Vector3.right);
                    break;
                default:
                    gameMng.interfaceMng.MainCameraCarrier.localRotation = Quaternion.AngleAxis(70, Vector3.right);
                    break;
            }
        }
        gameMng.interfaceMng.MainCamera.transform.localPosition = mPos;


    }

    private void KeyboardInput()
    {
        float zoom = -1*gameMng.interfaceMng.MainCamera.transform.localPosition.z;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            mPos = gameMng.interfaceMng.MainCameraCarrier.localPosition;
            mPos.z += 2.0f * zoom * Time.deltaTime;
            gameMng.interfaceMng.MainCameraCarrier.localPosition = mPos;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            mPos = gameMng.interfaceMng.MainCameraCarrier.localPosition;
            mPos.z -= 2.0f * zoom * Time.deltaTime;
            gameMng.interfaceMng.MainCameraCarrier.localPosition = mPos;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            mPos = gameMng.interfaceMng.MainCameraCarrier.localPosition;
            mPos.x -= 2.0f * zoom * Time.deltaTime;
            gameMng.interfaceMng.MainCameraCarrier.localPosition = mPos;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            mPos = gameMng.interfaceMng.MainCameraCarrier.localPosition;
            mPos.x += 2.0f * zoom * Time.deltaTime;
            gameMng.interfaceMng.MainCameraCarrier.localPosition = mPos;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameMng.mapMng.SetMap();
        }
    }
}
