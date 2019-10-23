using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSys.Lib;

public class TimeMng : MonoBehaviour
{
    public GameMng gameMng = null;
    public Text clockText = null;
    public Transform transCenter = null;
    public Transform transSun = null;
    public Transform transMoon = null;
    public Transform transSunRevolve = null;
    public Transform transMoonRevolve = null;
    public Transform transLight = null;

    public GameObject objRain = null;
    public GameObject objSnow = null;
    public GameObject objFlower = null;
    public GameObject objMaple = null;

    private float fDateTime = 9.0f;
    private float fDateSpeed = 1.0f;
    private eSeason curSeason = eSeason.SPRING;
    private eDayState curState = eDayState.DAY;
    private bool bPause = false;

    //싱글톤 형식으로 제작
    private static TimeMng instance = null;

    public static TimeMng Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof( TimeMng)) as TimeMng;
            }
            return instance;
        }
    }
    //싱글톤 만들기 끝

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TimeTick");
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TimeTick()
    {
        while (true)
        {
            if (!gameMng.bGameStart || bPause)
            {
                yield return null;
                continue;
            }

            fDateTime += Time.deltaTime/DateSpeed;
            if(fDateTime < 8.0f && fDateTime >= 7.0f && curState == eDayState.NIGHT)
            {
                transSunRevolve.localRotation = Quaternion.Euler(0,0, 15);
                transSun.localPosition = new Vector3(0, 36, 0);
                curState = eDayState.DAY;
            }
            if (fDateTime < 20.0f && fDateTime >= 19.0f && curState == eDayState.DAY)
            {
                transMoonRevolve.localRotation = Quaternion.Euler(0, 0, 15);
                transMoon.localPosition = new Vector3(0, -36, 0);
                curState = eDayState.NIGHT;
            }
            if (fDateTime >= 24.0f)
            {
                fDateTime = 0.0f;
                PassSeason();
            }
            transLight.Rotate(Vector3.forward, (Time.deltaTime / DateSpeed) * 15f );
            transCenter.Rotate(Vector3.forward, (Time.deltaTime / DateSpeed) * 15f);
            clockText.text = CurSeasonKR() + " " + CurTime();

            if(curState == eDayState.DAY)
            {
                transSunRevolve.Rotate(Vector3.forward, (Time.deltaTime / DateSpeed) * -15f);
                if (fDateTime >= 7.0f && fDateTime < 16.0f)
                {
                    transSun.localPosition = Vector3.MoveTowards(transSun.localPosition, new Vector3(0, 90, 0), Time.deltaTime / DateSpeed * 20);
                }
                else if(fDateTime >= 16.0f && fDateTime < 19.0f)
                {
                    transSun.localPosition = Vector3.MoveTowards(transSun.localPosition, new Vector3(0, 36, 0), Time.deltaTime / DateSpeed * 20);
                }
            }
            else if(curState == eDayState.NIGHT)
            {
                transMoonRevolve.Rotate(Vector3.forward, (Time.deltaTime / DateSpeed) * -15f);
                if (fDateTime >= 19.0f && fDateTime < 23.0f)
                {
                    transMoon.localPosition = Vector3.MoveTowards(transMoon.localPosition, new Vector3(0, -90, 0), Time.deltaTime / DateSpeed * 20);
                }
                else if (fDateTime >= 4.0f && fDateTime < 7.0f)
                {
                    transMoon.localPosition = Vector3.MoveTowards(transMoon.localPosition, new Vector3(0, -36, 0), Time.deltaTime / DateSpeed * 20);
                }
            }

            yield return null;
        }
    }

    private void PassSeason()
    {
        switch (curSeason)
        {
            case eSeason.SPRING:
                objFlower.SetActive(false);
                objRain.SetActive(true);
                transLight.GetComponentInChildren<Light>().intensity = 0.3f;
                curSeason = eSeason.SUMMER;
                break;
            case eSeason.SUMMER:
                objRain.SetActive(false);
                objMaple.SetActive(true);
                transLight.GetComponentInChildren<Light>().intensity = 1.0f;
                curSeason = eSeason.FALL;
                break;
            case eSeason.FALL:
                objMaple.SetActive(false);
                objSnow.SetActive(true);
                transLight.GetComponentInChildren<Light>().intensity = 0.5f;
                curSeason = eSeason.WINTER;
                break;
            case eSeason.WINTER:
                objSnow.SetActive(false);
                objFlower.SetActive(true);
                transLight.GetComponentInChildren<Light>().intensity = 1.0f;
                curSeason = eSeason.SPRING;
                break;
        }
    }

    private string CurSeasonKR()
    {
        switch (curSeason)
        {
            case eSeason.SPRING:
                return "봄 ";
            case eSeason.SUMMER:
                return "여름";
            case eSeason.FALL:
                return "가을";
            case eSeason.WINTER:
                return "겨울";
        }
        return "오류";
    }

    private string CurTime()
    {
        return (Hour < 10 ? ("0" + Hour) : Hour + "") + ":" + (Minit < 10?("0"+ Minit) : Minit + "");
    }

    private float DateSpeed
    {
        get
        {
            if (curState == eDayState.DAY)
                return fDateSpeed * 1.5f;
            else
                return fDateSpeed * 0.5f;
        }
    }

    public int Hour
    {
        get
        {
            return (int)fDateTime;
        }
    }

    public int Minit
    {
        get
        {
            return (int)((fDateTime - (float)Hour) * 60); 
        }
    }

    public eDayState CurDayState
    {
        get
        {
            return curState;
        }
    }


}
