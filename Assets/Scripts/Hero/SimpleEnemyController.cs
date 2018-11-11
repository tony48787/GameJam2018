using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleEnemyController : MonoBehaviour {

	public Text hpText;
	public float hp = 100;
	// Use this for initialization
	void Start () {
		UpdateHpText();
	}
	
	// Update is called once per frame
	void Update () {
		// UpdateHpText();
	}

	void UpdateHpText() {
		hpText.text = "Enemy HP: " + hp.ToString();
	}

	void Damage(float damage) {
		hp -= damage;
		UpdateHpText();
	}
}
