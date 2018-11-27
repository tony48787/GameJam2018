using UnityEngine;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    public MouseInputStatus mouseInputStatus;

    public float vertExtent;

    public float horzExtent;

    private Canvas mainCanvas;

    private TextMeshProUGUI waveText;
    private TextMeshProUGUI coinText;
    private TextMeshProUGUI levelText;
    private Text hintText;

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

        mouseInputStatus = MouseInputStatus.Attack;
    }

    void Start()
    {
        GameObject player = Instantiate(PrefabManager.instance.player, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));

        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        waveText = GameObject.Find("WaveText").GetComponent<TextMeshProUGUI>();
        coinText = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();
        levelText = GameObject.Find("PlayerLevelText").GetComponent<TextMeshProUGUI>();
        hintText = GameObject.Find("HintText").GetComponent<Text>();
        
        UpdateCursorTexture();
        
        IncrementCoinBy(1000);
    }

    void Update()
    {
        if (hintText.enabled) {
            UpdateHintTextPosition();
        }
    }

    void EndGame()
    {
        SceneManager.LoadScene("menu");
    }

    public void IncrementWaveBy(int delta = 1)
    {
        wave += delta;

        waveText.text = "Wave: " + wave;
    }

    public void IncrementCoinBy(long delta = 1)
    {
        coin += delta;

        coinText.text = "Coin: " + coin;
    }

    // do state checking inside method
    public void UpdateCursorTexture()
    {
        Texture2D cursorType = null;
        CursorMode cursorMode = CursorMode.Auto;
        switch (mouseInputStatus) {
            case MouseInputStatus.Attack:
                cursorType = PrefabManager.instance.crosshairCursorType;
                break;
            case MouseInputStatus.AddTower:
                cursorType = PrefabManager.instance.addTowerCursorType;
                cursorMode = CursorMode.ForceSoftware;
                break;
            case MouseInputStatus.UpgradeTower:
                cursorType = PrefabManager.instance.upgradeTowerCursorType;
                cursorMode = CursorMode.ForceSoftware;
                break;
            case MouseInputStatus.InteractUI:
                cursorType = null; 
                break;
            default:
                cursorType = PrefabManager.instance.crosshairCursorType;
                break;
        }
        Cursor.SetCursor(cursorType, new Vector2(cursorType.width/2, cursorType.height/2), cursorMode);
    }

    public void ShowHintText(string text)
    {
        hintText.enabled = true;
        hintText.text = text;
    }

    public void HideHintText()
    {
        hintText.enabled = false;
    }

    private void UpdateHintTextPosition()
    {
        Vector2 newPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y + 40);
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.transform as RectTransform, newPos, mainCanvas.worldCamera, out pos);
        hintText.transform.position = mainCanvas.transform.TransformPoint(pos);
    }
}