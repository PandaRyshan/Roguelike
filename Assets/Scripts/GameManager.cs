using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int level;
    public int baseSeed;
    private int prevRoomPlayerHealth;
    private int prevRoomPlayerCoins;
    private Player player;
    public static GameManager instance;

    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        level = 1;
        baseSeed = PlayerPrefs.GetInt("Seed");
        player = FindObjectOfType<Player>();

        Random.InitState(baseSeed);
        Generation.instance.Generate();
        UI.instance.UpdateLevelText(level);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Game")
        {
            Destroy(gameObject);
            return;
        }

        player = FindObjectOfType<Player>();
        level ++;
        baseSeed ++;

        Generation.instance.Generate();

        player.curHp = prevRoomPlayerHealth;
        player.coins = prevRoomPlayerCoins;
        UI.instance.UpdateHealth(player.curHp);
        UI.instance.UpdateCoinText(player.coins);
        UI.instance.UpdateLevelText(level);
    }

    public void GoToNextLevel()
    {
        prevRoomPlayerHealth = player.curHp;
        prevRoomPlayerCoins = player.coins;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Restart()
    {
        prevRoomPlayerHealth = player.maxHp;
        prevRoomPlayerCoins = 0;
        level = 1;
        SceneManager.LoadScene(0);
    }

}
