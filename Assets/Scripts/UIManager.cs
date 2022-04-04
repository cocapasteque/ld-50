using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class UIManager : MonoBehaviour
{
    public Text TimerText;
    public Image AvailableGustImg;

    private void Update()
    {
        if (GameManager.Instance.running)
        {
            UpdateTimer();
            UpdateGustBar();
        }
    }

    public void UpdateTimer()
    {       
        int minutes = (int)GameManager.Instance.timer / 60;
        TimerText.text = $"{minutes}:{(GameManager.Instance.timer - 60f * minutes).ToString("00.###", CultureInfo.InvariantCulture)}";
    }

    private void UpdateGustBar()
    {
        AvailableGustImg.fillAmount = GameManager.Instance.PlayerObj.AvailableGust;
    }
}
