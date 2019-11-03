using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBtn : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1.0f;
    }
}
