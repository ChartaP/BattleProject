using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField]
    private RectTransform BarTrans;
    [SerializeField]
    private RectTransform BackTrans;
    [SerializeField]
    private Image BarImage;

    private float Length = 1;
    // Start is called before the first frame update
    public void Set(float Max,Color32 BarColor)
    {
        this.Length = Max/2;
        BarTrans.sizeDelta = new Vector2(Length, 4);
        BackTrans.sizeDelta = new Vector2(Length, 4);
        BarImage.color = BarColor;
    }

    public void Cur(float Max,float Cur,Vector3 pos)
    {
        BarTrans.sizeDelta = new Vector2(Length * Cur/Max, 4);

        transform.position = Camera.main.WorldToScreenPoint(pos);
    }
}
