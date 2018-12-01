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
    public MouseInputState mouseInputStatus;
    public GameState gameState;
    public PlayerLevelManager playerLevelManager;

    public float vertExtent;

    public float horzExtent;

    private Canvas mainCanvas;

    private TextMeshProUGUI waveText;
    private TextMeshProUGUI coinText;
    private TextMeshProUGUI levelText;
    private Text hintText;
    private GameObject playerMenu;
    private GameObject shootModeImageObj;
    private Image shootModeImage;
    private GameObject swordModeImageObj;
    private Image swordModeImage;

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

        mouseInputStatus = MouseInputState.Attack;

        gameState = GameState.Transiting;
        
        playerLevelManager = new PlayerLevelManager();
        playerLevelManager.SetVitalityToLevel(1);
        playerLevelManager.SetSkillToLevel(1);
        playerLevelManager.SetStrengthToLevel(1);

        coin = 1000;
    }

    void Start()
    {
        GameObject player = Instantiate(PrefabManager.instance.player, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));

        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        waveText = GameObject.Find("WaveText").GetComponent<TextMeshProUGUI>();
        coinText = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();
        levelText = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();
        hintText = GameObject.Find("HintText").GetComponent<Text>();
        playerMenu = GameObject.Find("PlayerMenu");
        shootModeImageObj = GameObject.Find("ShootModeImage");
        swordModeImageObj = GameObject.Find("SwordModeImage");
        shootModeImage = shootModeImageObj.GetComponent<Image>();
        swordModeImage = swordModeImageObj.GetComponent<Image>();
        
        UpdateCursorTexture();
        UpdateAttackTypeUI();
        
        // IncrementCoinBy(1000);
        coinText.text = "Coin: " + coin;
    }

    void Update()
    {
        if (hintText.enabled) {
            UpdateHintTextPosition();
        }
        
        // for level up testing use
        if (Input.GetKeyDown("i")) {
            playerLevelManager.LevelUpVitalityBy(1);
            Debug.Log("Level up vitality by 1, current vitality: " + playerLevelManager.vitality);
        }
        if (Input.GetKeyDown("o")) {
            playerLevelManager.LevelUpSkillBy(1);
            Debug.Log("Level up skill by 1, current skill: " + playerLevelManager.skill);
        }
        if (Input.GetKeyDown("p")) {
            playerLevelManager.LevelUpStrengthBy(1);
            Debug.Log("Level up strength by 1, current strength: " + playerLevelManager.strength);
        }
        if (Input.GetKeyDown("[")) {
            Debug.Log("maxHp: " + playerStatus.maxHp);
            Debug.Log("maxChargeBarValue: " + playerStatus.maxChargeBarValue);
            Debug.Log("chargeCoolDownDuration: " + playerStatus.chargeCoolDownDuration);
            Debug.Log("chargeBarIncreaseRate: " + playerStatus.chargeBarIncreaseRate);
            Debug.Log("chargeBarDecreaseRate: " + playerStatus.chargeBarDecreaseRate);
            Debug.Log("shootCoolDownDuration: " + playerStatus.shootCoolDownDuration);
            Debug.Log("bulletDamage: " + weaponStatus.bulletDamage);
            Debug.Log("bulletSpeed: " + weaponStatus.bulletSpeed);
            Debug.Log("swordDamage: " + weaponStatus.swordDamage);
            Debug.Log("swordRepelForce: " + weaponStatus.swordRepelForce);
            Debug.Log("chargeBulletDamage: " + weaponStatus.chargeBulletDamage);
            Debug.Log("chargeBulletSpeed: " + weaponStatus.chargeBulletSpeed);
            Debug.Log("chargeSwordDamage: " + weaponStatus.chargeSwordDamage);
            Debug.Log("chargeSwordSpeed: " + weaponStatus.chargeSwordSpeed);
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

    public void SetPlayerLevelText(int level)
    {
        levelText.text = "Level: " + level;
    }

    // do state checking inside method
    public void UpdateCursorTexture()
    {
        Texture2D cursorType = null;
        CursorMode cursorMode = CursorMode.Auto;
        switch (mouseInputStatus) {
            case MouseInputState.Attack:
                cursorType = PrefabManager.instance.crosshairCursorType;
                Cursor.SetCursor(cursorType, new Vector2(cursorType.width/2, cursorType.height/2), cursorMode);
                break;
            case MouseInputState.AddTower:
                cursorType = PrefabManager.instance.addTowerCursorType;
                cursorMode = CursorMode.ForceSoftware;
                Cursor.SetCursor(cursorType, new Vector2(cursorType.width/2, cursorType.height/2), cursorMode);
                break;
            case MouseInputState.UpgradeTower:
                cursorType = PrefabManager.instance.upgradeTowerCursorType;
                cursorMode = CursorMode.ForceSoftware;
                Cursor.SetCursor(cursorType, new Vector2(cursorType.width/2, cursorType.height/2), cursorMode);
                break;
            case MouseInputState.InteractUI:
            default:
                cursorType = PrefabManager.instance.pickerCursorType;
                Cursor.SetCursor(cursorType, new Vector2(cursorType.width/3, 0), cursorMode);
                break;
        }
    }

    public void UpdateAttackTypeUI() {
        switch (playerStatus.currentAttackType) {
            case PlayerAttackType.Shoot: {
                shootModeImageObj.transform.localScale = new Vector2(1, 1);
                swordModeImageObj.transform.localScale = new Vector2(0.7f, 0.7f);
                Color tmp = shootModeImage.color;
                tmp.a = 1f;
                shootModeImage.color = tmp;
                tmp = swordModeImage.color;
                tmp.a = 0.7f;
                swordModeImage.color = tmp;
                break;
            }
            case PlayerAttackType.Slice: {
                swordModeImageObj.transform.localScale = new Vector2(1, 1);
                shootModeImageObj.transform.localScale = new Vector2(0.7f, 0.7f);
                Color tmp = shootModeImage.color;
                tmp.a = 0.7f;
                shootModeImage.color = tmp;
                tmp = swordModeImage.color;
                tmp.a = 1f;
                swordModeImage.color = tmp;
                break;
            }
        }
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