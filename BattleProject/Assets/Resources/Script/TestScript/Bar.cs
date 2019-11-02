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
    private RectTransform HungryBarTrans;
    [SerializeField]
    private RectTransform HungryBackTrans;
    [SerializeField]
    private RectTransform HungryBlankTrans;
    [SerializeField]
    private Image BarImage;
    [SerializeField]
    private Text NameText;

    private float Length = 1;

    public void SetHP(float Max,Color32 BarColor)
    {
        this.Length = Max/2;
        HealthBarTrans.sizeDelta = new Vector2(Length, 8);
        HealthBlankTrans.sizeDelta = new Vector2(Length, 8);
        HealthBackTrans.sizeDelta = new Vector2(Length+4, 12);
        BarImage.color = BarColor;
    }

    public void CurHP(float Max,float Cur,Vector3 pos)
    {

        HealthBarTrans.sizeDelta = new Vector2(Length * Cur/Max, 8);

        transform.position = Camera.main.WorldToScreenPoint(pos);
    }

    public void SetName(string name)
    {
        NameText.text = name;
    }

    public void SetHungry(int Max)
    {
        HungryBackTrans.gameObject.SetActive(true);
        HungryBarTrans.sizeDelta = new Vector2(Max*15, 6);
        HungryBlankTrans.sizeDelta = new Vector2(Max * 15, 6);
        HungryBackTrans.sizeDelta = new Vector2(Max * 15 + 4, 10);
    }

    public void CurHungry(int Cur)
    {
        HungryBarTrans.sizeDelta = new Vector2(Cur * 15, 6);
    }
}
