using UnityEngine;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;             //Static instance of GameManager which allows it to be accessed by any other script.                    //Store a reference to our BoardManager which will set up the level.
    private int wave = 1;                                  //Current level number, expressed in game as "Day 1".
    private int coin = 0;

    public float vertExtent;

    public float horzExtent;

    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
        //SceneManager.LoadScene("menu");
        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
    }

    void EndGame()
    {
        SceneManager.LoadScene("menu");
    }

    public void IncrementWaveBy(int delta = 1)
    {
        wave += delta;

        GameObject.Find("WaveText").GetComponent<TextMeshProUGUI>().text = "Wave: " + wave;
    }

    public void IncrementCoinBy(int delta = 1)
    {
        coin += delta;

        GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>().text = "Coin: " + coin;
    }

}