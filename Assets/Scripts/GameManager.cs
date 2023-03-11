using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlayerObject playerObject;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI keyText;
    public static GameManager inst;
    void Awake()
    {
        if (inst == null)
        {
            GameManager.inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        SetInfo();
    }

    void SetInfo()
    {
        lifeText.text = "Lifes: " + playerObject.Life.ToString("00");
        coinText.text = "Coins: " + playerObject.Coins.ToString("00");
        keyText.text = "Keys: " + playerObject.Keys.ToString("00");
    }

    public void ExitGame()
    {   
        print("salir del juego");
        Application.Quit();
    }

}


[System.Serializable]
public class PlayerObject
{
    public int Life = 200;
    public int Coins = 150;
    public int Keys = 0;

    public void AddCoin(int coin)
    {
        Coins += coin;
    }

    public void DecreaseCoin(int coin)
    {
        Coins -= coin;
    }

    public int GetCoin()
    {
        return Coins;
    }

    public void DrecreaseLife(int life)
    {
        Life -= life;
        if (Life <= 0)
        {
            Life = 0;
            Time.timeScale = 0;
        }
        
    }
}