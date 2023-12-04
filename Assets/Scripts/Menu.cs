using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public TMP_InputField seedInput;

    void Start()
    {
        // give the input prefab a default random value
        // and the value should be a num between 0 and 1000000
        GameObject.Find("SeedInput").GetComponent<TMP_InputField>().text = Random.Range(0, 1000000).ToString();
        seedInput.text = PlayerPrefs.GetInt("Seed").ToString();
    }

    public void OnUpdateSeed()
    {
        PlayerPrefs.SetInt("Seed", int.Parse(seedInput.text));
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("Game");
    }
}
