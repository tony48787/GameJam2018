using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {

	public float recoverHp = 20f;
	private CircleCollider2D circleCollider2D;
    private GameManager gm;
    private PlayerStatus playerStatus;
	// Use this for initialization
	void Start () {
		circleCollider2D = GetComponent<CircleCollider2D>();
        gm = GameManager.instance;
        playerStatus = gm.playerStatus;
    }
	
	void OnTriggerEnter2D(Collider2D other) {
		if (!other.isTrigger && other.CompareTag("Player")) {
            playerStatus.currentHp += recoverHp;
			if (playerStatus.currentHp >= playerStatus.maxHp) {
                playerStatus.currentHp = playerStatus.maxHp;
            }
            Debug.Log("Current: " + playerStatus.currentHp);
			Destroy(gameObject);
		}
	}
}
