using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpMenuController : MonoBehaviour {

	public int playerLevel;
	public int vitality;
	public int skill;
	public int strength;
	public long playerCoin;
	private long coinDeducted;

	private Text levelText;
	private Text coinText;
	private Text reqCoinText;
	private Button incVitalBtn;
	private Button decVitalBtn;
	private Button incSklBtn;
	private Button decSklBtn;
	private Button incStrBtn;
	private Button decStrBtn;
	private Text vitalText;
	private Text sklText;
	private Text strText;
	private Button confirmBtn;
	private Button cancelBtn;
	
	private GameManager gm;
	
	// Use this for initialization
	void Start () {
		gm = GameManager.instance;
		levelText = transform.Find("PlayerLevelText").GetComponent<Text>();
		coinText = transform.Find("PlayerCoinText").GetComponent<Text>();
		reqCoinText = transform.Find("RequiredCoinText").GetComponent<Text>();
		incVitalBtn = transform.Find("VitalRow/PlusVitalBtn").GetComponent<Button>();
		decVitalBtn = transform.Find("VitalRow/MinusVitalBtn").GetComponent<Button>();
		incSklBtn = transform.Find("SklRow/PlusSklBtn").GetComponent<Button>();
		decSklBtn = transform.Find("SklRow/MinusSklBtn").GetComponent<Button>();
		incStrBtn = transform.Find("StrRow/PlusStrBtn").GetComponent<Button>();
		decStrBtn = transform.Find("StrRow/MinusStrBtn").GetComponent<Button>();
		vitalText = transform.Find("VitalRow/BG/VitalLevel").GetComponent<Text>();
		sklText = transform.Find("SklRow/BG/SklLevel").GetComponent<Text>();
		strText = transform.Find("StrRow/BG/StrLevel").GetComponent<Text>();
		confirmBtn = transform.Find("ConfirmBtn").GetComponent<Button>();
		cancelBtn = transform.Find("CancelBtn").GetComponent<Button>();

		incVitalBtn.onClick.AddListener(() => ChangeVitalityBy(1));
		decVitalBtn.onClick.AddListener(() => ChangeVitalityBy(-1));
		incSklBtn.onClick.AddListener(() => ChangeSkillBy(1));
		decSklBtn.onClick.AddListener(() => ChangeSkillBy(-1));
		incStrBtn.onClick.AddListener(() => ChangeStrengthBy(1));
		decStrBtn.onClick.AddListener(() => ChangeStrengthBy(-1));
		confirmBtn.onClick.AddListener(() => CommitLevelUp());

		RetrieveDataFromGM();
		coinDeducted = 0;
		UpdatePanel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable() {
		// menu opened
		if (gm) {
			RetrieveDataFromGM();
			UpdatePanel();
		}
	}

	void RetrieveDataFromGM() {
		playerLevel = gm.playerLevelManager.playerLevel;
		vitality = gm.playerLevelManager.vitality;
		skill = gm.playerLevelManager.skill;
		strength = gm.playerLevelManager.strength;
		playerCoin = gm.coin;
	}

	void UpdatePanel() {
		levelText.text = "Player Level: " + playerLevel.ToString();
		coinText.text = "Coin: " + playerCoin.ToString();
		reqCoinText.text = "Required Coin: " + gm.playerLevelManager.GetCoinToLevelUpToLevel(playerLevel+1).ToString();
		vitalText.text = vitality.ToString();
		sklText.text = skill.ToString();
		strText.text = strength.ToString();
	}

	void ChangeVitalityBy(int level) {
		int newVitalLevel = vitality + level;
		int newPlayerLevel = playerLevel + level;
		if (level > 0) {
			long cost = gm.playerLevelManager.GetCoinToLevelUpToLevel(newPlayerLevel);
			if (cost <= playerCoin) {
				playerCoin -= cost;
				coinDeducted += cost;
				vitality = newVitalLevel;
				playerLevel = newPlayerLevel;
				UpdatePanel();
			}
		}
		else {
			// prevent level down
			if (newVitalLevel >= gm.playerLevelManager.vitality) {
				long cost = gm.playerLevelManager.GetCoinToLevelUpToLevel(playerLevel);
				playerCoin += cost;
				coinDeducted -= cost;
				vitality = newVitalLevel;
				playerLevel = newPlayerLevel;
				UpdatePanel();
			}
		}
	}

	void ChangeSkillBy(int level) {
		int newSklLevel = skill + level;
		int newPlayerLevel = playerLevel + level;
		if (level > 0) {
			long cost = gm.playerLevelManager.GetCoinToLevelUpToLevel(newPlayerLevel);
			if (cost <= playerCoin) {
				playerCoin -= cost;
				coinDeducted += cost;
				skill = newSklLevel;
				playerLevel = newPlayerLevel;
				UpdatePanel();
			}
		}
		else {
			// prevent level down
			if (newSklLevel >= gm.playerLevelManager.skill) {
				long cost = gm.playerLevelManager.GetCoinToLevelUpToLevel(playerLevel);
				playerCoin += cost;
				coinDeducted -= cost;
				skill = newSklLevel;
				playerLevel = newPlayerLevel;
				UpdatePanel();
			}
		}
	}

	void ChangeStrengthBy(int level) {
		int newStrLevel = strength + level;
		int newPlayerLevel = playerLevel + level;
		if (level > 0) {
			long cost = gm.playerLevelManager.GetCoinToLevelUpToLevel(newPlayerLevel);
			if (cost <= playerCoin) {
				playerCoin -= cost;
				coinDeducted += cost;
				strength = newStrLevel;
				playerLevel = newPlayerLevel;
				UpdatePanel();
			}
		}
		else {
			// prevent level down
			if (newStrLevel >= gm.playerLevelManager.strength) {
				long cost = gm.playerLevelManager.GetCoinToLevelUpToLevel(playerLevel);
				playerCoin += cost;
				coinDeducted -= cost;
				strength = newStrLevel;
				playerLevel = newPlayerLevel;
				UpdatePanel();
			}
		}
	}

	void CommitLevelUp() {
		gm.playerLevelManager.SetVitalityToLevel(vitality);
		gm.playerLevelManager.SetSkillToLevel(skill);
		gm.playerLevelManager.SetStrengthToLevel(strength);
		gm.coin = playerCoin;
		gm.IncrementCoinBy(-(gm.coin - playerCoin));
		gm.SetPlayerLevelText(playerLevel);
	}
}
