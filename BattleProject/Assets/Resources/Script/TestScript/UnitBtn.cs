﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitBtn : MonoBehaviour
{
    [SerializeField]
    private Image BtnImage = null;
    [SerializeField]
    private Button Btn = null;

    public UnitCtrl myUnit = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBtn(UnitCtrl myUnit)
    {
        this.myUnit = myUnit;
        if (myUnit == null)
        {
            BtnImage.enabled = false;
            Btn.enabled = false;
        }
        else
        {
            BtnImage.enabled = true;
            BtnImage.sprite = Resources.Load<Sprite>("Texture/" + myUnit.Icon);
            Btn.enabled = true;
        }
    }

    public void SelectUnit()
    {
        GameMng.Instance.playerMng.CtrlPlayer.SelectObject(myUnit);
    }

}
