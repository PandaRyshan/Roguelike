using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject[] hearts;
    public TextMeshProUGUI coinText;
    public GameObject keyIcon;
    public TextMeshProUGUI levelText;
    public RawImage map;

    public static UI instance;

    void Awake()
    {
        instance = this;
    }

    public void UpdateHealth(int health)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < health);
        }
    }

    public void UpdateCoinText(int coins)
    {
        coinText.text = coins.ToString();
    }

    public void ToggleKeyIcon(bool toggle)
    {
        keyIcon.SetActive(toggle);
    }

    public void UpdateLevelText(int level)
    {
        levelText.text = "Level " + level;
    }
}
