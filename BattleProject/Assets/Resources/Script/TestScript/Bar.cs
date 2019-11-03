using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField]
    private RectTransform HealthBarTrans;
    [SerializeField]
    private RectTransform HealthBackTrans;
    [SerializeField]
    private RectTransform HealthBlankTrans;
    [SerializeField]
    private RectTransform ProgressBarTrans;
    [SerializeField]
    private RectTransform ProgressBackTrans;
    [SerializeField]
    private RectTransform ProgressBlankTrans;
    [SerializeField]
    private RectTransform HungryTrans;
    [SerializeField]
    private Image BarImage;
    [SerializeField]
    private Text NameText;

    private float Length = 1;
    

    public void UpdatePos(Vector3 pos)
    {
        Vector3 temp = GameMng.Instance.interfaceMng.MainCamera.WorldToScreenPoint(pos);
        transform.position = temp;
    }
    

    public void SetHP(float Max,Color32 BarColor)
    {
        this.Length = Max/2;
        HealthBarTrans.sizeDelta = new Vector2(Length, 8);
        HealthBlankTrans.sizeDelta = new Vector2(Length, 8);
        HealthBackTrans.sizeDelta = new Vector2(Length+4, 12);
        BarImage.color = BarColor;
    }

    public void CurHP(float Max,float Cur)
    {

        HealthBarTrans.sizeDelta = new Vector2(Length * Cur/Max, 8);
        
    }

    public void SetName(string name)
    {
        NameText.text = name;
    }

    public void SetHungry(int Max)
    {
        ProgressBackTrans.gameObject.SetActive(true);
        HungryTrans.gameObject.SetActive(true);
        ProgressBarTrans.sizeDelta = new Vector2(Max*15, 6);
        ProgressBlankTrans.sizeDelta = new Vector2(Max * 15, 6);
        ProgressBackTrans.sizeDelta = new Vector2(Max * 15 + 4, 10);
    }

    public void CurHungry(int Cur)
    {
        ProgressBarTrans.sizeDelta = new Vector2(Cur * 15, 6);
    }

    public void SetProgress(int Max)
    {
        ProgressBackTrans.gameObject.SetActive(true);
        ProgressBarTrans.sizeDelta = new Vector2(Max * 15f, 6);
        ProgressBlankTrans.sizeDelta = new Vector2(Max * 15f, 6);
        ProgressBackTrans.sizeDelta = new Vector2(Max * 15f + 4, 10);
    }

    public void CurProgress(int Cur)
    {
        ProgressBarTrans.sizeDelta = new Vector2(Cur * 15f, 6);
    }

    public void InProgress()
    {
        ProgressBackTrans.gameObject.SetActive(false);
    }
}
