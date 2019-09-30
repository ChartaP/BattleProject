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
    private Vector3 CameraPos;
    public Transform peek;
    private float scroll;
    private bool isMove;
    // Start is called before the first frame update
    void Start()
    {
        isMove = true;
        CameraPos = gameMng.interfaceMng.MainCameraCarrier.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        MouseInput();
        KeyboardInput();
        gameMng.interfaceMng.MainCameraCarrier.localPosition = Vector3.Lerp(gameMng.interfaceMng.MainCameraCarrier.localPosition,CameraPos,3.0f*Time.deltaTime);
    }

    /// <summary>
    /// 마우스 입력 처리 메서드,
    /// 자료출처 : https://youtu.be/ceMyupol6AQ
    /// </summary>
    private void MouseInput()
    {
        mPos = Vector3.zero ;
        if (Input.GetMouseButtonDown(0)) {//왼쪽 마우스 누르기
            isMove = false;
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
                        gameMng.playerMng.GetControlPlayer().SelectUnit(null);
                        break;
                    case "Unit":
                        gameMng.playerMng.GetControlPlayer().SelectUnit(hit.transform.GetComponent<UnitCtrl>());
                        break;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))//왼쪽 마우스 떼기
        {
            isMove = true;
            gameMng.interfaceMng.SelecteBox.gameObject.SetActive(false);
            if (mPosStart != mPosEnd)
            {
                gameMng.playerMng.GetControlPlayer().SelectObject(mPosStart, mPosEnd);
            }
        }
        else if (Input.GetMouseButton(0))//왼쪽 마우스 드래그
        {
            mPos = Input.mousePosition;
            EndPoint = mPos;
            gameMng.interfaceMng.DragRectInterface(StartPoint, EndPoint);
            mPosEnd = Camera.main.ScreenToViewportPoint(mPos);
        }

        if (Input.GetMouseButtonDown(1))//오른쪽 마우스 누르기
        {
            
        }
        else if (Input.GetMouseButtonUp(1))//오른쪽 마우스 떼기
        {
            mPos = Input.mousePosition;
            ray = Camera.main.ScreenPointToRay(mPos);
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 point;
                switch (hit.transform.tag)
                {
                    case "Tile":
                        //gameMng.unitMng.IssueMoveOrder(hit.transform.localPosition);
                        peek.position = hit.point;
                        point = peek.localPosition;
                        peek.GetComponentInChildren<MeshRenderer>().material = gameMng.unitMng.RangeMater[0];
                        peek.GetComponentInChildren<Animation>().Play();
                        gameMng.playerMng.CtrlPlayer.OrderUnits(point);
                        break;
                    case "Unit":
                        peek.position = hit.transform.position;
                        point = peek.localPosition;
                        if(hit.transform.GetComponent<UnitCtrl>().Owner == gameMng.playerMng.CtrlPlayer)
                            peek.GetComponentInChildren<MeshRenderer>().material = gameMng.unitMng.RangeMater[0];
                        else
                            peek.GetComponentInChildren<MeshRenderer>().material = gameMng.unitMng.RangeMater[1];
                        peek.GetComponentInChildren<Animation>().Play();
                        gameMng.playerMng.CtrlPlayer.OrderUnits(hit.transform);
                        break;
                }
            }
        }
        else if (Input.GetMouseButton(1))//오른쪽 마우스 드래그
        {

        }

        scroll = Input.GetAxis("Mouse ScrollWheel") * scrSpeed;

        mPos = gameMng.interfaceMng.MainCamera.transform.localPosition;
        if(mPos.z - scroll <= -8 && mPos.z - scroll >= -50)
        {
            mPos.z -= scroll;
            if (mPos.z > -10)
            {
                gameMng.interfaceMng.MainCameraCarrier.localRotation = Quaternion.AngleAxis(45, Vector3.right);
            }
            else if (mPos.z > -13)
            {
                gameMng.interfaceMng.MainCameraCarrier.localRotation = Quaternion.AngleAxis(55, Vector3.right);
            }
            else if (mPos.z > -16)
            {
                gameMng.interfaceMng.MainCameraCarrier.localRotation = Quaternion.AngleAxis(65, Vector3.right);
            }
            else
            {
                gameMng.interfaceMng.MainCameraCarrier.localRotation = Quaternion.AngleAxis(75, Vector3.right);
            }
        }
        gameMng.interfaceMng.MainCamera.transform.localPosition = mPos;


    }

    /// <summary>
    /// 키보드 입력 처리 메서드
    /// </summary>
    private void KeyboardInput()
    {
        float zoom = -1*gameMng.interfaceMng.MainCamera.transform.localPosition.z+2;
        int nXSize = GameInfo.nXSize-1;
        int nYSize = GameInfo.nYSize-1;
        if (isMove)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (CameraPos.z + 4.0f * zoom * Time.deltaTime > nYSize)
                    CameraPos.z = nYSize;
                else if (CameraPos.z < nYSize)
                    CameraPos.z += 4.0f * zoom * Time.deltaTime;
                else
                    CameraPos.z = nYSize;
                CameraPos.y = gameMng.mapMng.GetHeight((int)CameraPos.x, (int)CameraPos.z);
                
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (CameraPos.z - 4.0f * zoom * Time.deltaTime < 0)
                    CameraPos.z = 0;
                else if (CameraPos.z> 1)
                    CameraPos.z -= 4.0f * zoom * Time.deltaTime;
                else
                    CameraPos.z = 0;
                CameraPos.y = gameMng.mapMng.GetHeight((int)CameraPos.x, (int)CameraPos.z);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (CameraPos.x - 4.0f * zoom * Time.deltaTime < 0)
                    CameraPos.x = 0;
                else if (CameraPos.x >1)
                    CameraPos.x -= 4.0f * zoom * Time.deltaTime;
                else
                    CameraPos.x = 0;
                CameraPos.y = gameMng.mapMng.GetHeight((int)CameraPos.x, (int)CameraPos.z);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if(CameraPos.x + 4.0f * zoom * Time.deltaTime > nXSize)
                    CameraPos.x = nXSize;
                else if (CameraPos.x < nXSize)
                    CameraPos.x += 4.0f * zoom * Time.deltaTime;
                else
                    CameraPos.x = nXSize;
                CameraPos.y = gameMng.mapMng.GetHeight((int)CameraPos.x, (int)CameraPos.z);
            }
            CameraPos.y = CameraPos.y <3 ? 3 : CameraPos.y;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameMng.mapMng.ResetMap();
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log(GameSys.Item.ItemMng.Instance.Item(0).ID+ GameSys.Item.ItemMng.Instance.Item(0).Name+ GameSys.Item.ItemMng.Instance.Item(0).Icon+ GameSys.Item.ItemMng.Instance.Item(0).Size);
        }
    }




}
