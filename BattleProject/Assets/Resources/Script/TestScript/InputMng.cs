using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using GameSys.Lib;
using GameSys.Building;


public class InputMng : MonoBehaviour
{
    public GameMng gameMng;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 StartPoint;
    private Vector3 EndPoint;
    private Vector3 mPosStart;
    private Vector3 mPosEnd;
    public float scrSpeed = 10.0f;
    private Vector3 mPos;
    private Vector3 CameraPos;
    public Transform peek;
    private float scroll;
    private bool isMove;

    [SerializeField]
    private eInputState inputState = eInputState.CONTROL_OBJECT;
    [SerializeField]
    private GameObject Preview = null;

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

    public void ChangeState(int state)
    {
        if (state == (int)inputState)
            return;
        switch (state)
        {
            case (int)eInputState.CONTROL_OBJECT:
                peek.Find("Cube").GetComponent<MeshRenderer>().enabled = false;
                inputState = eInputState.CONTROL_OBJECT;
                break;
            case (int)eInputState.CREATE_OBJECT:
                peek.Find("Cube").GetComponent<MeshRenderer>().enabled = true;
                inputState = eInputState.CREATE_OBJECT;
                break;
        }
    }

    /// <summary>
    /// 마우스 입력 처리 메서드,
    /// 자료출처 : https://youtu.be/ceMyupol6AQ
    /// </summary>
    private void MouseInput()
    {
        switch (inputState)
        {
            case eInputState.CONTROL_OBJECT:
                ObjectCtrl();
                break;
            case eInputState.CREATE_OBJECT:
                isMove = true;
                CreateObject();
                break;
        }
        ScrollCtrl();
    }

    private void ObjectCtrl()
    {
        mPos = Vector3.zero;
        if (Input.GetMouseButtonDown(0))
        {//왼쪽 마우스 누르기
            isMove = false;
            mPos = Input.mousePosition;
            mPosStart = GameMng.Instance.interfaceMng.MainCamera.ScreenToViewportPoint(mPos);
            Debug.Log("(" + mPos.x + "," + mPos.y + ") The Left Mouse");
            ray = GameMng.Instance.interfaceMng.MainCamera.ScreenPointToRay(mPos);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log("HitObject : " + hit.transform.name);
                StartPoint = hit.point;
                switch (hit.transform.tag)
                {
                    case "Tile":
                        gameMng.playerMng.GetControlPlayer().SelectObject(null);
                        break;
                    case "Unit":
                        gameMng.playerMng.GetControlPlayer().SelectObject(hit.transform.GetComponent<UnitCtrl>());
                        break;
                    case "Building":
                        gameMng.playerMng.GetControlPlayer().SelectObject(hit.transform.GetComponent<BuildingCtrl>());
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
            mPosEnd = GameMng.Instance.interfaceMng.MainCamera.ScreenToViewportPoint(mPos);
        }

        if (Input.GetMouseButtonDown(1))//오른쪽 마우스 누르기
        {

        }
        else if (Input.GetMouseButtonUp(1))//오른쪽 마우스 떼기
        {
            mPos = Input.mousePosition;
            ray = GameMng.Instance.interfaceMng.MainCamera.ScreenPointToRay(mPos);
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 point;
                switch (hit.transform.tag)
                {
                    case "Tile":
                        //gameMng.unitMng.IssueMoveOrder(hit.transform.localPosition);
                        peek.position = hit.point;
                        point = peek.localPosition;
                        foreach(MeshRenderer mesh in peek.GetComponentsInChildren<MeshRenderer>())
                        {
                            mesh.material = gameMng.unitMng.RangeMater[0];
                        }

                        peek.GetComponentInChildren<Animation>().Play();
                        gameMng.playerMng.CtrlPlayer.OrderObjects(GameSys.Lib.eOrder.MovePos, peek);
                        break;
                    case "Unit":
                        peek.position = hit.transform.position;
                        point = peek.localPosition;
                        if (hit.transform.GetComponent<UnitCtrl>().Owner == gameMng.playerMng.CtrlPlayer)
                        {
                            foreach (MeshRenderer mesh in peek.GetComponentsInChildren<MeshRenderer>())
                            {
                                mesh.material = gameMng.unitMng.RangeMater[0];
                            }
                            peek.GetComponentInChildren<Animation>().Play();
                            gameMng.playerMng.CtrlPlayer.OrderObjects(GameSys.Lib.eOrder.MoveTarget, hit.transform);
                        }
                        else
                        {
                            foreach (MeshRenderer mesh in peek.GetComponentsInChildren<MeshRenderer>())
                            {
                                mesh.material = gameMng.unitMng.RangeMater[1];
                            }
                            peek.GetComponentInChildren<Animation>().Play();
                            gameMng.playerMng.CtrlPlayer.OrderObjects(GameSys.Lib.eOrder.AtkTarget, hit.transform);
                        }
                        break;
                    case "Building":
                        peek.position = hit.transform.position;
                        point = peek.localPosition;
                        if (hit.transform.GetComponent<BuildingCtrl>().Owner == gameMng.playerMng.CtrlPlayer)
                        {
                            foreach (MeshRenderer mesh in peek.GetComponentsInChildren<MeshRenderer>())
                            {
                                mesh.material = gameMng.unitMng.RangeMater[0];
                            }
                            peek.GetComponentInChildren<Animation>().Play();
                            gameMng.playerMng.CtrlPlayer.OrderObjects(GameSys.Lib.eOrder.MoveTarget, hit.transform);
                        }
                        else
                        {
                            foreach (MeshRenderer mesh in peek.GetComponentsInChildren<MeshRenderer>())
                            {
                                mesh.material = gameMng.unitMng.RangeMater[1];
                            }
                            peek.GetComponentInChildren<Animation>().Play();
                            gameMng.playerMng.CtrlPlayer.OrderObjects(GameSys.Lib.eOrder.AtkTarget, hit.transform);
                        }
                        break;
                }
            }
        }
        else if (Input.GetMouseButton(1))//오른쪽 마우스 드래그
        {

        }
    }

    private void CreateObject()
    {
        mPos = Input.mousePosition;
        ray = GameMng.Instance.interfaceMng.MainCamera.ScreenPointToRay(mPos);
        int layerMask = 1 << LayerMask.NameToLayer("TileLayer");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("UI"))
                return;
           // Debug.Log("HitObject : " + hit.collider.name + hit.point);
            peek.position = hit.collider.transform.position;
            if (GameMng.Instance.mapMng.bOpen[(int)peek.localPosition.x, (int)peek.localPosition.z]) {
                foreach (MeshRenderer mesh in peek.GetComponentsInChildren<MeshRenderer>())
                {
                    mesh.material = gameMng.unitMng.RangeMater[0];
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {

                        GameMng.Instance.buildingMng.CreateBuilding(GameMng.Instance.playerMng.CtrlPlayer,GameMng.Instance.playerMng.CtrlPlayer.selectableObject[0] as UnitCtrl, BuildingInfoMng.Instance.Building(GameMng.Instance.playerMng.CtrlPlayer.selectBuild), peek.transform.localPosition);
                        ChangeState(0);
                        GameMng.Instance.interfaceMng.CreateInfoInterface.UnSelectCreate();
                    }
                }
            }
            else
            {
                foreach (MeshRenderer mesh in peek.GetComponentsInChildren<MeshRenderer>())
                {
                    mesh.material = gameMng.unitMng.RangeMater[1];
                }
            }
        }
    }

    private void ScrollCtrl()
    {
        scroll = Input.GetAxis("Mouse ScrollWheel") * scrSpeed;

        mPos = gameMng.interfaceMng.MainCamera.transform.localPosition;
        if (mPos.z - scroll <= -4 && mPos.z - scroll >= -50)
        {
            mPos.z -= scroll;
            if (mPos.z > -6)
            {
                gameMng.interfaceMng.MainCameraCarrier.localRotation = Quaternion.AngleAxis(45, Vector3.right);
            }
            else if (mPos.z > -8)
            {
                gameMng.interfaceMng.MainCameraCarrier.localRotation = Quaternion.AngleAxis(45, Vector3.right);
            }
            else if (mPos.z > -10)
            {
                gameMng.interfaceMng.MainCameraCarrier.localRotation = Quaternion.AngleAxis(45, Vector3.right);
            }
            else if (mPos.z > -12)
            {
                gameMng.interfaceMng.MainCameraCarrier.localRotation = Quaternion.AngleAxis(45, Vector3.right);
            }
            else
            {
                gameMng.interfaceMng.MainCameraCarrier.localRotation = Quaternion.AngleAxis(45, Vector3.right);
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
        float fSpeed = 1.0f;
        if (isMove)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (CameraPos.z + fSpeed * zoom * Time.deltaTime > nYSize)
                    CameraPos.z = nYSize;
                else if (CameraPos.z < nYSize)
                    CameraPos.z += fSpeed  * zoom * Time.deltaTime;
                else
                    CameraPos.z = nYSize;
                CameraPos.y = gameMng.mapMng.GetHeight((int)CameraPos.x, (int)CameraPos.z);
                
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (CameraPos.z - fSpeed * zoom * Time.deltaTime < 0)
                    CameraPos.z = 0;
                else if (CameraPos.z> 1)
                    CameraPos.z -= fSpeed * zoom * Time.deltaTime;
                else
                    CameraPos.z = 0;
                CameraPos.y = gameMng.mapMng.GetHeight((int)CameraPos.x, (int)CameraPos.z);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (CameraPos.x - fSpeed * zoom * Time.deltaTime < 0)
                    CameraPos.x = 0;
                else if (CameraPos.x >1)
                    CameraPos.x -= fSpeed * zoom * Time.deltaTime;
                else
                    CameraPos.x = 0;
                CameraPos.y = gameMng.mapMng.GetHeight((int)CameraPos.x, (int)CameraPos.z);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if(CameraPos.x + fSpeed * zoom * Time.deltaTime > nXSize)
                    CameraPos.x = nXSize;
                else if (CameraPos.x < nXSize)
                    CameraPos.x += fSpeed * zoom * Time.deltaTime;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(inputState == eInputState.CREATE_OBJECT)
            {
                ChangeState(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Time.timeScale == 1f)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            gameMng.interfaceMng.AlertText("경고 테스트 123");
            //Debug.Log(GameSys.Item.ItemMng.Instance.Item(0).ID+ GameSys.Item.ItemMng.Instance.Item(0).Name+ GameSys.Item.ItemMng.Instance.Item(0).Icon+ GameSys.Item.ItemMng.Instance.Item(0).Size);
        }
    }




}
