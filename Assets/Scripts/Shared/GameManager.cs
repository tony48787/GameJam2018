using UnityEngine;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;             //Static instance of GameManager which allows it to be accessed by any other script.                    //Store a reference to our BoardManager which will set up the level.
    public int wave = 1;                                  //Current level number, expressed in game as "Day 1".
    public long coin = 0;

    // Player related variables
    public GameObject player;
    public PlayerStatus playerStatus;
    public WeaponStatus weaponStatus;

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

        // set player status variables
        playerStatus.maxHp = 100f;
        playerStatus.currentHp = 100f;
        playerStatus.maxChargeBarValue = 1000f;
        playerStatus.currentChargeBarValue = 0f;
        playerStatus.chargeCoolDownDuration = 3f;
        playerStatus.chargeBarIncreaseRate = 5f;
        playerStatus.chargeBarDecreaseRate = 10f;
        playerStatus.shootCoolDownDuration = 0.2f;
        playerStatus.invincibleDuration = 2f;
        playerStatus.currentAttackType = PlayerAttackType.Shoot;

        // set weapon status variables
        weaponStatus.bulletDamage = 10f;
        weaponStatus.bulletSpeed = 15f;
        weaponStatus.swordDamage = 10f;
        weaponStatus.swordRepelForce = 10f;
        weaponStatus.chargeBulletDamage = 50f;
        weaponStatus.chargeBulletSpeed = 10f;
        weaponStatus.chargeSwordDamage = 100f;
        weaponStatus.chargeSwordSpeed = 3f;

        IncrementCoinBy(1000);
    }

    void Start()
    {
        GameObject player = Instantiate(PrefabManager.instance.player, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
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

    public void IncrementCoinBy(long delta = 1)
    {
        coin += delta;

        GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>().text = "Coin: " + coin;
    }

}